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

        private string _message;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        public float Sector1Float {get;set;}
        public float Sector2Float {get;set;}
        public float Sector3Float {get;set;}

        public void UpdateValues(RaceModel model)
        {

            float bestLapTime = float.MaxValue;
            float previousSectors = 0;
            int bestLapIndex = 0;
            Sector3Float = float.MaxValue;
            for (int i = 1; i < model.PreviousLapTime.Count; i++)
            {
                if (model.Lap[i] != model.Lap[i-1])
                {
                    float value = model.PreviousLapTime[i] - previousSectors;
                    if (model.PreviousLapTime[i] < bestLapTime)
                    {
                        bestLapTime = model.PreviousLapTime[i];
                        Sector3Float = model.PreviousLapTime[i] - model.TimeSector1[i - 1] - model.TimeSector2[i - 1];
                    }
                    bestLapTime = Math.Min(bestLapTime, model.PreviousLapTime[i]);
                    bestLapIndex = i-1;
                }
                else
                {
                    previousSectors = model.TimeSector1[i] + model.TimeSector2[i];
                }
            }

            Sector1Float = model.TimeSector1[bestLapIndex];
            Sector2Float = model.TimeSector2[bestLapIndex];
            Track = RaceModel.DistancesToNames[model.TrackLength[0]];

            BestLap = bestLapTime.ToString();
            Sector1 = Sector1Float.ToString();
            Sector2 = Sector2Float.ToString();
            Sector3 = Sector3Float.ToString();
        }
    }
}
