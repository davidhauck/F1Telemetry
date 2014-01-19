using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using F1Telemetry.Core;

namespace F1Telemetry
{
    public struct TelemetryPacket
    {
        public float Time;
        public float LapTime;
        public float LapDistance;
        public float Distance;
        
        public float X;
        
        public float Y;
        
        public float Z;
        public float Speed;
        
        public float WorldSpeedX;
        
        public float WorldSpeedY;
        
        public float WorldSpeedZ;
        
        public float XR;
        
        public float Roll;
        
        public float ZR;
        
        public float XD;
        
        public float Pitch;
        
        public float ZD;
        
        public float SuspensionPositionRearLeft;
        
        public float SuspensionPositionRearRight;
        
        public float SuspensionPositionFrontLeft;
        
        public float SuspensionPositionFrontRight;
        
        public float SuspensionVelocityRearLeft;
        
        public float SuspensionVelocityRearRight;
        
        public float SuspensionVelocityFrontLeft;
        
        public float SuspensionVelocityFrontRight;
        
        public float WheelSpeedBackLeft;
        
        public float WheelSpeedBackRight;
        
        public float WheelSpeedFrontLeft;
        
        public float WheelSpeedFrontRight;
        
        public float Throttle;
        
        public float Steer;
        
        public float Brake;
        
        public float Clutch;
        
        public float Gear;
        
        public float LateralAcceleration;
        
        public float LongitudinalAcceleration;
        public float Lap;
        
        public float EngineRevs;

        /* New Fields in Patch 12 */
        
        public float NewField1;     // Always 1?
        
        public float RacePosition;     // Position in race
        
        public float KersRemaining;     // Kers Remaining
        
        public float KersRecharge;     // Always 400000? 
        
        public float DrsStatus;     // Drs Status
        
        public float Difficulty;     // 2 = Medium or Easy, 1 = Hard, 0 = Expert
        
        public float Assists;     // 0 = All assists are off.  1 = some assist is on.
        
        public float FuelRemaining;      // Not sure if laps or Litres?
        
        public float SessionType;   // 9.5 = race, 10 = time trail / time attack, 170 = quali, practice, championsmode
        
        public float NewField10;
        
        public float Sector;    // Sector (0, 1, 2)
        
        public float TimeSector1;    // Time Intermediate 1
        
        public float TimeSector2;    // Time Intermediate 2
        
        public float BrakeTemperatureRearLeft;
        
        public float BrakeTemperatureRearRight;
        
        public float BrakeTemperatureFrontLeft;
        
        public float BrakeTemperatureFrontRight;
        
        public float NewField18;    // Always 0?
        
        public float NewField19;    // Always 0?
        
        public float NewField20;    // Always 0?
        
        public float NewField21;    // Always 0?
        
        public float CompletedLapsInRace;    // Number of laps Completed (in GP only)
        
        public float TotalLapsInRace;    // Number of laps in GP (GP only)
        
        public float TrackLength;    // Track Length
        
        public float PreviousLapTime;    // Lap time of previous lap

        //  The next three fields are new for F1 2013

        
        public float NewField26;    // Always 0?
        
        public float NewField27;    // Always 0?

        public float NewField28;    // Always 0?

        /* End new Fields */

        
        public float SpeedInKmPerHour
        {
            get { return Speed * 3.60f; }
        }

        
        public bool IsSittingInPits
        {
            get { return Math.Abs(LapTime - 0) < Utils.Epsilon && Math.Abs(Speed - 0) < Utils.Epsilon; }
        }

        
        public bool IsInPitLane
        {
            get { return Math.Abs(LapTime - 0) < Utils.Epsilon; }
        }

        
        public string SessionTypeName
        {
            get
            {
                if (Math.Abs(this.SessionType - 9.5f) < 0.0001f)
                    return "Race";
                if (Math.Abs(this.SessionType - 10f) < 0.0001f)
                    return "Time Trial";
                if (Math.Abs(this.SessionType - 170f) < 0.0001f)
                    return "Qualifying or Practice";
                return "Other";
            }
        }
    }
}
