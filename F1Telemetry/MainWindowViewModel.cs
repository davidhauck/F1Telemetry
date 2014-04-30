using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using F1Telemetry.Core;
using F1Telemetry.Track;
using F1Telemetry.Forces;
using F1Telemetry.Upload;

namespace F1Telemetry
{
    class MainWindowViewModel : BaseViewModel
    {
        private const int PORTNUM = 20777;
        private const string IP = "127.0.0.1";
        UdpClient _socket;
        IPEndPoint _senderIP;
        string _filepath;

        public Action<float, float, float, float, int> UploadLap;

        MemberInfo[] _members;

        string _output;
        public String Output
        {
            get
            {
                return _output;
            }
            set
            {
                SetProperty(ref _output, value);
            }
        }

        private bool _shouldUploadLapsToServer;
        public bool ShouldUploadLapsToServer
        {
            get
            {
                return _shouldUploadLapsToServer;
            }
            set
            {
                SetProperty(ref _shouldUploadLapsToServer, value);
            }
        }

        private ICommand _previousRaceClick;
        public ICommand PreviousRaceClick
        {
            get
            {
                if (_previousRaceClick == null)
                {
                    _previousRaceClick = new RelayCommand(p => this.OpenOldRace());
                }
                return _previousRaceClick;
            }
        }

        private ICommand _beginServerSyncClick;
        public ICommand BeginServerSyncClick
        {
            get
            {
                if (_beginServerSyncClick == null)
                {
                    _beginServerSyncClick = new RelayCommand(p => this.BeginServerSync());
                }
                return _beginServerSyncClick;
            }
        }

        private TrackView TrackView { get; set; }

        private ForcesView ForcesView { get; set; }

        public MainWindowViewModel()
        {
            _socket = new UdpClient();
            _socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.ExclusiveAddressUse = false;
            _socket.Client.Bind(new IPEndPoint(IPAddress.Any, PORTNUM));
            Output = "Bound to socket on " + IP + ":" + PORTNUM.ToString();

            storedRpms = new List<List<System.Windows.Point>>();
            for (int i = 0; i < NUM_GEARS; i++)
            {
                storedRpms.Add(new List<Point>());
            }

            try
            {
                EditConfigFile();
            }
            catch(Exception e)
            {
                Output = "Cannot find game config file. Telemetry may not be able to be receieved.";
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\F1Telemetry\\";
            string fileName = DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".csv";
            _filepath = System.IO.Path.Combine(path, fileName);

            bool isExists = System.IO.Directory.Exists(path);

            if (!isExists)
                System.IO.Directory.CreateDirectory(path);

            BeginListen();
        }

        void EditConfigFile()
        {
            string gameConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\FormulaOne2013\\hardwaresettings\\hardware_settings_config.xml";
            string[] lines = System.IO.File.ReadAllLines(gameConfigPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("motion"))
                {
                    int index = lines[i].IndexOf("extradata");
                    int nextIndex = lines[i].IndexOf("\"", index);
                    string newString = lines[i].Substring(0, nextIndex + 1);
                    newString += "3";
                    newString += lines[i].Substring(nextIndex + 2, lines[i].Length - nextIndex - 2);
                    lines[i] = newString;
                    int x = 0;
                }
            }

            System.IO.File.WriteAllLines(gameConfigPath, lines);
        }

        RaceModel _rm;

        private void OpenOldRace()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "sample";
                ofd.DefaultExt = ".csv";
                ofd.Filter = "Comma Separated Values (.csv)|*.csv";
                Nullable<bool> result = ofd.ShowDialog();
                if (result == true)
                {
                    string filename = ofd.FileName;
                    //RacingVisibility = Visibility.Collapsed;
                    //AnalyzingVisibility = Visibility.Visible;
                    try
                    {
                        _rm = ReadFile(filename);
                    }
                    catch (Exception e)
                    {
                        Output = "File is corrupt.";
                    }
                    //CollectionLaps.Clear();
                    //CollectionLaps.Add("All Laps");
                    //for (int i = 1; i < rm.CompletedLaps[rm.CompletedLaps.Count - 1] + 1; i++)
                    //{
                    //    CollectionLaps.Add(i.ToString());
                    //}
                    int numberOfSections;

                    List<List<float>> accelerations = new List<List<float>>() { _rm.LateralAcceleration, null, _rm.LongitudinalAcceleration };
                    List<List<float>> coordinates = new List<List<float>>() { _rm.X, null, _rm.Z };
                    List<int> completedLaps = new List<int>(_rm.CompletedLapsInRace.Count);
                    for (int i = 0; i < _rm.CompletedLapsInRace.Count; i++)
                    {
                        completedLaps.Add((int)_rm.CompletedLapsInRace[i]);
                    }

                    bool shouldCreateCharts = true;
                    if (completedLaps[0] > completedLaps[completedLaps.Count - 1] - 2)
                    {
                        shouldCreateCharts = false;
                        //throw new Exception("not enough laps to accurately gather data");
                    }

                    _rm.TurnSections = Utils.FindTurnsBasedOnLap(completedLaps, accelerations, coordinates, completedLaps[0] + 1, out numberOfSections);
                    //CollectionSections.Clear();
                    //CollectionSections.Add("All Sections");
                    //for (int i = 1; i <= numberOfSections; i++)
                    //{
                    //    CollectionSections.Add(i.ToString());
                    //}
                    UpdateScreen(shouldCreateCharts);
                }
            }
            catch (Exception e)
            {
                Output = "File is open in another program. Probably this one actually. Close this app, reopen it, and try to load that file again. If that doesn't work, im out of ideas.";
            }
        }


        private void BeginServerSync()
        {
            ServicePointManager
                .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            string server = "https://racingleaguecharts.com/sessions/register";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(server);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = String.Format("{{ \"driver\": {0}, \"track\": {1}, \"type\": {2} }}", "\"jetiger\"", "4444.94727", "10.0");
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    int x = 0;
                }
            }
        }

        private void UpdateScreen(bool shouldCreateCharts)
        {
            if (_rm != null)
            {
                CreateUploadWindow();

                if (shouldCreateCharts)
                {
                    DrawForces();
                    DrawTrack();
                }
            }
        }

        private void CreateUploadWindow()
        {
            UploadView uploadview = new UploadView();
            uploadview.UploadModel.UpdateValues(_rm);
            uploadview.Topmost = true;
            uploadview.Show();
        }

        private void DrawForces()
        {
            List<float> xAcc = null;
            List<float> yAcc = null;
            int selectedlap;
            int selectedsection;

            //if (SelectedLap == "All Laps")
                selectedlap = -1;
            //else
            //    selectedlap = int.Parse(SelectedLap);

            //if (SelectedSection == "All Sections")
                selectedsection = -1;
            //else
            //    selectedsection = int.Parse(SelectedSection);

            xAcc = new List<float>();
            yAcc = new List<float>();

            for (int i = 0; i < _rm.LateralAcceleration.Count; i++)
            {
                if ((_rm.CompletedLapsInRace[i] == selectedlap - 1 || selectedlap == -1) && (_rm.TurnSections[i] == selectedsection - 1 || selectedsection == -1))
                {
                    xAcc.Add(_rm.LateralAcceleration[i]);
                    yAcc.Add(_rm.LongitudinalAcceleration[i]);
                }
            }
            if (ForcesView == null)
            {
                ForcesView = new ForcesView();
            }
            ForcesView.Forces.DrawAccelerationMap(xAcc, yAcc);
            ForcesView.Topmost = true;
            ForcesView.Show();
        }

        private void DrawTrack()
        {
            List<float> xCoor = null;
            List<float> yCoor = null;
            List<int> turnSections = null;
            int selectedlap;
            int selectedsection;

            //if (SelectedLap == "All Laps")
                selectedlap = -1;
            //else
            //    selectedlap = int.Parse(SelectedLap);

            //if (SelectedSection == "All Sections")
                selectedsection = -1;
            //else
            //    selectedsection = int.Parse(SelectedSection);

            xCoor = new List<float>();
            yCoor = new List<float>();
            turnSections = new List<int>();

            for (int i = 0; i < _rm.LateralAcceleration.Count; i++)
            {
                if ((_rm.CompletedLapsInRace[i] == selectedlap - 1 || selectedlap == -1) && (_rm.TurnSections[i] == selectedsection - 1 || selectedsection == -1))
                {
                    xCoor.Add(_rm.X[i]);
                    yCoor.Add(_rm.Z[i]);
                    turnSections.Add(_rm.TurnSections[i]);
                }
            }
            if (TrackView == null)
            {
                TrackView = new TrackView();
            }
            TrackView.Track.DrawTrack(xCoor, yCoor, turnSections);
            TrackView.Topmost = true;
            TrackView.Show();
        }

        private RaceModel ReadFile(string filename)
        {
            RaceModel model = new RaceModel();

            var fields = typeof(RaceModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            System.IO.StreamReader filestream = new System.IO.StreamReader(filename);
            string line = filestream.ReadLine();
            //string[] instancenames = line.Split(',');
            while ((line = filestream.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                for (int i = 0; i < values.Count(); i++)
                {
                    var oldvalue = fields[i].GetValue(model);
                    if (oldvalue.GetType() == typeof(List<float>))
                    {
                        var array = oldvalue as List<float>;
                        array.Add(float.Parse(values[i]));
                        fields[i].SetValue(model, array);
                    }
                    else if (oldvalue.GetType() == typeof(List<string>))
                    {
                        var array = oldvalue as List<string>;
                        array.Add(values[i]);
                        fields[i].SetValue(model, array);
                    }
                }
            }
            return model;
        }

        void BeginListen()
        {
            _rm = new RaceModel();
            Thread t = new Thread(() =>
                {
                    while (true)
                    {
                        Byte[] received = _socket.Receive(ref _senderIP);
                        GCHandle handle = GCHandle.Alloc(received, GCHandleType.Pinned);
                        var packet = (TelemetryPacket)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TelemetryPacket));
                        handle.Free();

                        AppendToFile(packet);
                        _rm.Add(packet);
                        UpdateScreen(packet);
                    }
                });
            t.IsBackground = true;
            t.Start();
        }

        private ImageSource _throttleImage;
        public ImageSource ThrottleImage
        {
            get
            {
                return _throttleImage;
            }
            set
            {
                SetProperty(ref _throttleImage, value);
            }
        }

        private ImageSource _brakeImage;
        public ImageSource BrakeImage
        {
            get
            {
                return _brakeImage;
            }
            set
            {
                SetProperty(ref _brakeImage, value);
            }
        }

        private ImageSource _turnImage;
        public ImageSource TurnImage
        {
            get
            {
                return _turnImage;
            }
            set
            {
                SetProperty(ref _turnImage, value);
            }
        }

        private ImageSource _rpmsGeometry;
        public ImageSource Rpms
        {
            get
            {
                return _rpmsGeometry;
            }
            set
            {
                SetProperty(ref _rpmsGeometry, value);
            }
        }

        int _previousLap;
        private void UpdateScreen(TelemetryPacket packet)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int height = 100;
                if (UploadLap != null)
                {
                    if (_previousLap != packet.Lap)
                    {
                        if (_previousLap == 0)
                        {
                            if (packet.Lap == 1)
                                SubmitLapOnline(packet);
                        }
                        else
                            SubmitLapOnline(packet);
                        _previousLap = (int)packet.Lap;
                    }
                }
                DrawThrottle(packet, height);
                DrawBrake(packet, height);
                DrawTurning(packet, height);
                DrawRpms(packet);
            });
        }

        private void SubmitLapOnline(TelemetryPacket packet)
        {
            float totalLapTime = packet.PreviousLapTime;
            float Sector1Time = _rm.TimeSector1[_rm.TimeSector1.Count - 2];
            float Sector2Time = _rm.TimeSector2[_rm.TimeSector2.Count - 2];
            float Sector3Time = totalLapTime - Sector1Time - Sector2Time;
            int trackNum = (int)Enum.Parse(typeof(F1Telemetry.Upload.UploadView.TrackName), RaceModel.DistancesToNames[packet.TrackLength]);
            UploadLap(totalLapTime, Sector1Time, Sector2Time, Sector3Time, trackNum);
        }

        int RpmHeight = 150;
        int RpmWidth = 400;
        
        List<List<System.Windows.Point>> storedRpms;
        List<LineGeometry> _lines;
        int NUM_GEARS = 7;
        int _counter = 0;
        private void DrawRpms(TelemetryPacket packet)
        {
            if (packet.Gear <= 0 || packet.Gear == 10 || packet.SpeedInKmPerHour < 50)
                return;
            double y = RpmHeight - packet.EngineRevs / 2000 * RpmHeight;
            double x = packet.SpeedInKmPerHour / 350 * RpmWidth;
            storedRpms[(int)packet.Gear - 1].Add(new Point(x, y));

            if (_counter % 20 == 0 || _lines != null)
            {
                if (_lines == null)
                {
                    if (Rpms == null)
                    {
                        GeometryGroup background = new GeometryGroup();
                        background.Children.Add(new RectangleGeometry(new Rect(0, 0, RpmWidth, RpmHeight)));

                        GeometryGroup foreground = new GeometryGroup();
                        foreground.Children.Add(new System.Windows.Media.EllipseGeometry(new Point(x, y), 1, 1));

                        GeometryDrawing aGeometryDrawing = new GeometryDrawing();
                        aGeometryDrawing.Geometry = background;

                        GeometryDrawing otherGeometryDrawing = new GeometryDrawing();
                        otherGeometryDrawing.Geometry = foreground;

                        aGeometryDrawing.Brush = Brushes.Transparent;
                        aGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

                        otherGeometryDrawing.Brush = Brushes.Black;
                        otherGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

                        DrawingGroup dg = new DrawingGroup();
                        dg.Children.Add(aGeometryDrawing);
                        dg.Children.Add(otherGeometryDrawing);

                        DrawingImage geometryImage = new DrawingImage(dg);

                        Rpms = geometryImage;
                    }
                    else
                    {
                        (((((Rpms as DrawingImage).Drawing as DrawingGroup).Children[0] as GeometryDrawing).Geometry) as GeometryGroup).Children.Add(new System.Windows.Media.EllipseGeometry(new Point(x, y), 1, 1));
                    }

                    if (storedRpms[0].Count > 1000 &&
                        storedRpms[1].Count > 1000 &&
                        storedRpms[2].Count > 1000 &&
                        storedRpms[3].Count > 1000 &&
                        storedRpms[4].Count > 1000 &&
                        storedRpms[5].Count > 1000 &&
                        storedRpms[6].Count > 1000)
                    {

                        float previousX = 50;
                        _lines = new List<LineGeometry>();
                        foreach (var points in storedRpms)
                        {
                            float meanX = points.Average(point => (float)point.X);
                            float meanY = points.Average(point => (float)point.Y);

                            float sumXSquared = points.Sum(point => (float)(point.X * point.X));
                            float sumXY = points.Sum(point => (float)(point.X * point.Y));

                            float a = (sumXY / points.Count - meanX * meanY) / (sumXSquared / points.Count - meanX * meanX);
                            float b = (a * meanX - meanY);
                            
                            float nextX = (10 + b) / a;
                            LineGeometry geometry = new LineGeometry(new Point(previousX, a * previousX - b), new Point(nextX, a * nextX - b));
                            previousX = nextX;
                            _lines.Add(geometry);
                        }
                    }
                }
                else
                {
                    GeometryGroup background = new GeometryGroup();
                    background.Children.Add(new RectangleGeometry(new Rect(0, 0, RpmWidth, RpmHeight)));

                    GeometryGroup foreground = new GeometryGroup();
                    foreground.Children.Add(new System.Windows.Media.EllipseGeometry(new Point(x, y), 5, 5));
                    foreach (var line in _lines)
                    {
                        foreground.Children.Add(line);
                    }

                    GeometryDrawing aGeometryDrawing = new GeometryDrawing();
                    aGeometryDrawing.Geometry = background;

                    GeometryDrawing otherGeometryDrawing = new GeometryDrawing();
                    otherGeometryDrawing.Geometry = foreground;

                    aGeometryDrawing.Brush = Brushes.Transparent;
                    aGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

                    otherGeometryDrawing.Brush = Brushes.Black;
                    otherGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

                    DrawingGroup dg = new DrawingGroup();
                    dg.Children.Add(aGeometryDrawing);
                    dg.Children.Add(otherGeometryDrawing);

                    DrawingImage geometryImage = new DrawingImage(dg);
                    geometryImage.Freeze();

                    Rpms = geometryImage;
                }
            }
            _counter++;
        }

        private void DrawThrottle(TelemetryPacket packet, int height)
        {
            GeometryGroup background = new GeometryGroup();
            background.Children.Add(new RectangleGeometry(new Rect(0, 0, 20, 100)));

            GeometryGroup throttle = new GeometryGroup();
            throttle.Children.Add(new RectangleGeometry(new Rect(1, height - 100 * packet.Throttle, 18, 100 * packet.Throttle)));

            GeometryDrawing otherGeometryDrawing = new GeometryDrawing();
            otherGeometryDrawing.Geometry = throttle;
            otherGeometryDrawing.Brush = Brushes.Red;

            GeometryDrawing aGeometryDrawing = new GeometryDrawing();
            aGeometryDrawing.Geometry = background;

            aGeometryDrawing.Brush = Brushes.Transparent;
            aGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(aGeometryDrawing);
            dg.Children.Add(otherGeometryDrawing);

            DrawingImage geometryImage = new DrawingImage(dg);
            //geometryImage.Freeze();

            ThrottleImage = geometryImage;
        }

        private void DrawBrake(TelemetryPacket packet, int height)
        {
            GeometryGroup background = new GeometryGroup();
            background.Children.Add(new RectangleGeometry(new Rect(0, 0, 20, 100)));

            GeometryGroup throttle = new GeometryGroup();
            throttle.Children.Add(new RectangleGeometry(new Rect(1, height - 100 * packet.Brake, 18, 100 * packet.Brake)));

            GeometryDrawing otherGeometryDrawing = new GeometryDrawing();
            otherGeometryDrawing.Geometry = throttle;
            otherGeometryDrawing.Brush = Brushes.Blue;

            GeometryDrawing aGeometryDrawing = new GeometryDrawing();
            aGeometryDrawing.Geometry = background;

            aGeometryDrawing.Brush = Brushes.Transparent;
            aGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(aGeometryDrawing);
            dg.Children.Add(otherGeometryDrawing);

            DrawingImage geometryImage = new DrawingImage(dg);
            //geometryImage.Freeze();

            BrakeImage = geometryImage;
        }

        private void DrawTurning(TelemetryPacket packet, int height)
        {
            GeometryGroup background = new GeometryGroup();
            background.Children.Add(new RectangleGeometry(new Rect(0, 0, height, 20)));

            GeometryGroup throttle = new GeometryGroup();
            throttle.Children.Add(new RectangleGeometry(new Rect( height / 2.0 + packet.Steer / 2 * height, 0, 2, 20)));

            GeometryDrawing otherGeometryDrawing = new GeometryDrawing();
            otherGeometryDrawing.Geometry = throttle;
            otherGeometryDrawing.Brush = Brushes.Black;

            GeometryDrawing aGeometryDrawing = new GeometryDrawing();
            aGeometryDrawing.Geometry = background;

            aGeometryDrawing.Brush = Brushes.Transparent;
            aGeometryDrawing.Pen = new Pen(Brushes.Black, 1);

            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(aGeometryDrawing);
            dg.Children.Add(otherGeometryDrawing);

            DrawingImage geometryImage = new DrawingImage(dg);
            geometryImage.Freeze();

            TurnImage = geometryImage;
        }

        bool fileCreated = false;
        private void AppendToFile(TelemetryPacket packet)
        {
            if (!fileCreated)
            {
                Type type = typeof(TelemetryPacket);
                _members = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                using (StreamWriter f = File.AppendText(_filepath))
                {
                    foreach (var physMember in _members)
                    {
                        f.Write(physMember.Name + ",");
                    }
                    f.Write("\r\n");
                }
            }
            using (StreamWriter f = File.AppendText(_filepath))
            {
                foreach (var x in _members)
                {
                    Object propvalue = null;
                    if (x is FieldInfo)
                    {
                        propvalue = (x as FieldInfo).GetValue(packet);
                    }
                    f.Write(propvalue.ToString() + ",");
                }

                f.Write("\r\n");
            }
        }
    }
}
