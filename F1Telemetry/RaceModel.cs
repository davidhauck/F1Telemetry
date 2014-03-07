using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1Telemetry
{
    public class RaceModel
    {
        public RaceModel()
        {
            Time = new List<float>();
            LapTime = new List<float>();
            LapDistance = new List<float>();
            Distance = new List<float>();
            X = new List<float>();
            Y = new List<float>();
            Z = new List<float>();
            Speed = new List<float>();
            WorldSpeedX = new List<float>();
            WorldSpeedY = new List<float>();
            WorldSpeedZ = new List<float>();
            XR = new List<float>();
            Roll = new List<float>();
            ZR = new List<float>();
            XD = new List<float>();
            Pitch = new List<float>();
            ZD = new List<float>();
            SuspensionPositionRearLeft = new List<float>();
            SuspensionPositionRearRight = new List<float>();
            SuspensionPositionFrontLeft = new List<float>();
            SuspensionPositionFrontRight = new List<float>();
            SuspensionVelocityRearLeft = new List<float>();
            SuspensionVelocityRearRight = new List<float>();
            SuspensionVelocityFrontLeft = new List<float>();
            SuspensionVelocityFrontRight = new List<float>();
            WheelSpeedBackLeft = new List<float>();
            WheelSpeedBackRight = new List<float>();
            WheelSpeedFrontLeft = new List<float>();
            WheelSpeedFrontRight = new List<float>();
            Throttle = new List<float>();
            Steer = new List<float>();
            Brake = new List<float>();
            Clutch = new List<float>();
            Gear = new List<float>();
            LateralAcceleration = new List<float>();
            LongitudinalAcceleration = new List<float>();
            Lap = new List<float>();
            EngineRevs = new List<float>();
            NewField1 = new List<float>();
            RacePosition = new List<float>();
            KersRemaining = new List<float>();
            KersRecharge = new List<float>();
            DrsStatus = new List<float>();
            Difficulty = new List<float>();
            Assists = new List<float>();
            FuelRemaining = new List<float>();
            SessionType = new List<float>();
            NewField10 = new List<float>();
            Sector = new List<float>();
            TimeSector1 = new List<float>();
            TimeSector2 = new List<float>();
            BrakeTemperatureRearLeft = new List<float>();
            BrakeTemperatureRearRight = new List<float>();
            BrakeTemperatureFrontLeft = new List<float>();
            BrakeTemperatureFrontRight = new List<float>();
            NewField18 = new List<float>();
            NewField19 = new List<float>();
            NewField20 = new List<float>();
            NewField21 = new List<float>();
            CompletedLapsInRace = new List<float>();
            TotalLapsInRace = new List<float>();
            TrackLength = new List<float>();
            PreviousLapTime = new List<float>();
            NewField26 = new List<float>();
            NewField27 = new List<float>();
            NewField28 = new List<float>();
            Empty = new List<string>();
            TurnSections = new List<int>();
        }

        public List<float> Time { get; set; }
        public List<float> LapTime { get; set; }
        public List<float> LapDistance { get; set; }
        public List<float> Distance { get; set; }
        public List<float> X { get; set; }
        public List<float> Y { get; set; }
        public List<float> Z { get; set; }
        public List<float> Speed { get; set; }
        public List<float> WorldSpeedX { get; set; }
        public List<float> WorldSpeedY { get; set; }
        public List<float> WorldSpeedZ { get; set; }
        public List<float> XR { get; set; }
        public List<float> Roll { get; set; }
        public List<float> ZR { get; set; }
        public List<float> XD { get; set; }
        public List<float> Pitch { get; set; }
        public List<float> ZD { get; set; }
        public List<float> SuspensionPositionRearLeft { get; set; }
        public List<float> SuspensionPositionRearRight { get; set; }
        public List<float> SuspensionPositionFrontLeft { get; set; }
        public List<float> SuspensionPositionFrontRight { get; set; }
        public List<float> SuspensionVelocityRearLeft { get; set; }
        public List<float> SuspensionVelocityRearRight { get; set; }
        public List<float> SuspensionVelocityFrontLeft { get; set; }
        public List<float> SuspensionVelocityFrontRight { get; set; }
        public List<float> WheelSpeedBackLeft { get; set; }
        public List<float> WheelSpeedBackRight { get; set; }
        public List<float> WheelSpeedFrontLeft { get; set; }
        public List<float> WheelSpeedFrontRight { get; set; }
        public List<float> Throttle { get; set; }
        public List<float> Steer { get; set; }
        public List<float> Brake { get; set; }
        public List<float> Clutch { get; set; }
        public List<float> Gear { get; set; }
        public List<float> LateralAcceleration { get; set; }
        public List<float> LongitudinalAcceleration { get; set; }
        public List<float> Lap { get; set; }
        public List<float> EngineRevs { get; set; }
        public List<float> NewField1 { get; set; }
        public List<float> RacePosition { get; set; }
        public List<float> KersRemaining { get; set; }
        public List<float> KersRecharge { get; set; }
        public List<float> DrsStatus { get; set; }
        public List<float> Difficulty { get; set; }
        public List<float> Assists { get; set; }
        public List<float> FuelRemaining { get; set; }
        public List<float> SessionType { get; set; }
        public List<float> NewField10 { get; set; }
        public List<float> Sector { get; set; }
        public List<float> TimeSector1 { get; set; }
        public List<float> TimeSector2 { get; set; }
        public List<float> BrakeTemperatureRearLeft { get; set; }
        public List<float> BrakeTemperatureRearRight { get; set; }
        public List<float> BrakeTemperatureFrontLeft { get; set; }
        public List<float> BrakeTemperatureFrontRight { get; set; }
        public List<float> NewField18 { get; set; }
        public List<float> NewField19 { get; set; }
        public List<float> NewField20 { get; set; }
        public List<float> NewField21 { get; set; }
        public List<float> CompletedLapsInRace { get; set; }
        public List<float> TotalLapsInRace { get; set; }
        public List<float> TrackLength { get; set; }
        public List<float> PreviousLapTime { get; set; }
        public List<float> NewField26 { get; set; }
        public List<float> NewField27 { get; set; }
        public List<float> NewField28 { get; set; }
        public List<string> Empty { get; set; }
        public List<int> TurnSections { get; set; }
    }
}
