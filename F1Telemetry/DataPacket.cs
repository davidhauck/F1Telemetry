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
        public float TotalDistance;
        public float X;      // World space position
        public float Y;      // World space position
        public float Z;      // World space position
        public float Speed;
        public float X_Velocity;      // Velocity in world space
        public float Y_Velocity;      // Velocity in world space
        public float Z_Velocity;      // Velocity in world space
        public float X_RightDirection;      // World space right direction
        public float Y_RightDirection;      // World space right direction
        public float Z_RightDirection;      // World space right direction
        public float X_ForwardDirection;      // World space forward direction
        public float Y_ForwardDirection;      // World space forward direction
        public float Z_ForwardDirection;      // World space forward direction
        public float m_susp_pos_bl;
        public float m_susp_pos_br;
        public float m_susp_pos_fl;
        public float m_susp_pos_fr;
        public float m_susp_vel_bl;
        public float m_susp_vel_br;
        public float m_susp_vel_fl;
        public float m_susp_vel_fr;
        public float m_wheel_speed_bl;
        public float m_wheel_speed_br;
        public float m_wheel_speed_fl;
        public float m_wheel_speed_fr;
        public float Throttle;
        public float Steer;
        public float Brake;
        public float Clutch;
        public float Gear;
        public float m_gforce_lat;
        public float m_gforce_lon;
        public float Lap;
        public float EngineRevs;
        public float m_sli_pro_native_support; // SLI Pro support
        public float m_car_position;   // car race position
        public float m_kers_level;    // kers energy left
        public float m_kers_max_level;   // kers maximum energy
        public float Drs;     // 0 = off, 1 = on
        public float TractionControl;  // 0 (off) - 2 (high)
        public float m_anti_lock_brakes;  // 0 (off) - 1 (on)
        public float m_fuel_in_tank;   // current fuel mass
        public float m_fuel_capacity;   // fuel capacity
        public float m_in_pits;    // 0 = none, 1 = pitting, 2 = in pit area
        public float Sector;     // 0 = sector1, 1 = sector2; 2 = sector3
        public float TimeSector1;   // time of sector1 (or 0)
        public float TimeSector2;   // time of sector2 (or 0)
        public float m_brakes_temp0;   // brakes temperature (centigrade)
        public float m_brakes_temp1;   // brakes temperature (centigrade)
        public float m_brakes_temp2;   // brakes temperature (centigrade)
        public float m_brakes_temp3;   // brakes temperature (centigrade)
        public float m_wheels_pressure0;  // wheels pressure PSI
        public float m_wheels_pressure1;  // wheels pressure PSI
        public float m_wheels_pressure2;  // wheels pressure PSI
        public float m_wheels_pressure3;  // wheels pressure PSI
        public float m_team_info;    // team ID 
        public float CompletedLapsInRace;    // total number of laps in this race
        public float TrackLength;    // track size meters
        public float PreviousLapTime;   // last lap time
        public float m_max_rpm;    // cars max RPM, at which point the rev limiter will kick in
        public float m_idle_rpm;    // cars idle RPM
        public float m_max_gears;    // maximum number of gears
        public float SessionType;   // 0 = unknown, 1 = practice, 2 = qualifying, 3 = race
        public float m_drsAllowed;    // 0 = not allowed, 1 = allowed, -1 = invalid / unknown
        public float m_track_number;   // -1 for unknown, 0-21 for tracks
        public float m_vehicleFIAFlags;  // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red



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
                if (SessionType == 3)
                    return "Race";
                if (SessionType == 2)
                    return "Qualifying";
                if (SessionType == 1)
                    return "Practice";
                return "Unknown";
            }
        }
    }
}
