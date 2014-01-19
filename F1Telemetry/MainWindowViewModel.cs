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

namespace F1Telemetry
{
    class MainWindowViewModel : BaseViewModel
    {
        private const int PORTNUM = 20777;
        private const string IP = "127.0.0.1";
        UdpClient _socket;
        IPEndPoint _senderIP;
        string _filepath;

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

        public MainWindowViewModel()
        {
            _socket = new UdpClient();
            _socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.ExclusiveAddressUse = false;
            _socket.Client.Bind(new IPEndPoint(IPAddress.Any, PORTNUM));
            Output = "Bound to socket on " + IP + ":" + PORTNUM.ToString();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\F1Telemetry\\";
            string fileName = DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".csv";
            _filepath = System.IO.Path.Combine(path, fileName);

            bool isExists = System.IO.Directory.Exists(path);

            if (!isExists)
                System.IO.Directory.CreateDirectory(path);

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
            BeginListen();
        }

        void BeginListen()
        {
            new Thread(() =>
                {
                    while (true)
                    {
                        Byte[] received = _socket.Receive(ref _senderIP);
                        GCHandle handle = GCHandle.Alloc(received, GCHandleType.Pinned);
                        var packet = (TelemetryPacket)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TelemetryPacket));
                        handle.Free();

                        AppendToFile(packet);

                        UpdateScreen(packet);
                    }
                }).Start();
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

        private void UpdateScreen(TelemetryPacket packet)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int height = 100;

                DrawThrottle(packet, height);
                DrawBrake(packet, height);
                DrawTurning(packet, height);
                DrawRpms(packet);
            });
        }

        int RpmHeight = 150;
        int RpmWidth = 400;


        int _counter = 0;
        private void DrawRpms(TelemetryPacket packet)
        {
            if (_counter++ % 20 == 0)
            {
                double y = RpmHeight - packet.EngineRevs / 2000 * RpmHeight;
                double x = packet.SpeedInKmPerHour / 350 * RpmWidth;

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
                    //geometryImage.Freeze();

                    Rpms = geometryImage;
                }
                else
                {
                    (((((Rpms as DrawingImage).Drawing as DrawingGroup).Children[0] as GeometryDrawing).Geometry) as GeometryGroup).Children.Add(new System.Windows.Media.EllipseGeometry(new Point(x, y), 1, 1));
                }
            }
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

        private void AppendToFile(TelemetryPacket packet)
        {
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
