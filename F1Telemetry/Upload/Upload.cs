using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace F1Telemetry.Upload
{
    public class Upload : BaseViewModel
    {
        private string _username;
        public String Username
        {
            get
            {
                return _username;
            }
            set
            {
                SetProperty(ref _username, value);
            }
        }

        private string _track;
        public String Track
        {
            get
            {
                return _track;
            }
            set
            {
                SetProperty(ref _track, value);
            }
        }

        private string _bestLap;
        public String BestLap
        {
            get
            {
                return _bestLap;
            }
            set
            {
                SetProperty(ref _bestLap, value);
            }
        }

        private string _sector1;
        public String Sector1
        {
            get
            {
                return _sector1;
            }
            set
            {
                SetProperty(ref _sector1, value);
            }
        }

        private string _sector2;
        public String Sector2
        {
            get
            {
                return _sector2;
            }
            set
            {
                SetProperty(ref _sector2, value);
            }
        }

        private string _sector3;
        public String Sector3
        {
            get
            {
                return _sector3;
            }
            set
            {
                SetProperty(ref _sector3, value);
            }
        }

        private float _sector1Float;
        private float _sector2Float;
        private float _sector3Float;


        private ICommand _uploadClick;
        public ICommand UploadClick
        {
            get
            {
                if (_uploadClick == null)
                {
                    _uploadClick = new RelayCommand(p => this.UploadRace());
                }
                return _uploadClick;
            }
        }

        private void UploadRace()
        {
            WebService service = new WebService();
            service.UploadRace(Username, Track, BestLap.ToString(), _sector1Float, _sector2Float, _sector3Float);
        }

        public void UpdateValues(RaceModel model)
        {
            _sector1Float = model.TimeSector1.Where(s => s != 0).Min();
            _sector2Float = model.TimeSector2.Where(s => s != 0).Min();

            float bestLapTime = float.MaxValue;
            float previousSectors = 0;
            _sector3Float = float.MaxValue;
            for (int i = 1; i < model.PreviousLapTime.Count; i++)
            {
                if (model.Lap[i] != model.Lap[i-1])
                {
                    float value = model.PreviousLapTime[i] - previousSectors;
                    _sector3Float = Math.Min(_sector3Float, value);
                    bestLapTime = Math.Min(bestLapTime, model.PreviousLapTime[i]);
                }
                else
                {
                    previousSectors = model.TimeSector1[i] + model.TimeSector2[i];
                }
            }

            Username = "jetiger";
            Track = "Spa";
            BestLap = bestLapTime.ToString();
            Sector1 = _sector1Float.ToString();
            Sector2 = _sector2Float.ToString();
            Sector3 = _sector3Float.ToString();
        }
    }
}
