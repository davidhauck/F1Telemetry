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
            TotalDistance = new List<float>();
            X = new List<float>();      // World space position
            Y = new List<float>();      // World space position
            Z = new List<float>();      // World space position
            Speed = new List<float>();
            X_Velocity = new List<float>();      // Velocity in world space
            Y_Velocity = new List<float>();      // Velocity in world space
            Z_Velocity = new List<float>();      // Velocity in world space
            X_RightDirection = new List<float>();      // World space right direction
            Y_RightDirection = new List<float>();      // World space right direction
            Z_RightDirection = new List<float>();      // World space right direction
            X_ForwardDirection = new List<float>();      // World space forward direction
            Y_ForwardDirection = new List<float>();      // World space forward direction
            Z_ForwardDirection = new List<float>();      // World space forward direction
            m_susp_pos_bl = new List<float>();
            m_susp_pos_br = new List<float>();
            m_susp_pos_fl = new List<float>();
            m_susp_pos_fr = new List<float>();
            m_susp_vel_bl = new List<float>();
            m_susp_vel_br = new List<float>();
            m_susp_vel_fl = new List<float>();
            m_susp_vel_fr = new List<float>();
            m_wheel_speed_bl = new List<float>();
            m_wheel_speed_br = new List<float>();
            m_wheel_speed_fl = new List<float>();
            m_wheel_speed_fr = new List<float>();
            Throttle = new List<float>();
            Steer = new List<float>();
            Brake = new List<float>();
            Clutch = new List<float>();
            Gear = new List<float>();
            LateralAcceleration = new List<float>();
            LongitudinalAcceleration = new List<float>();
            Lap = new List<float>();
            EngineRevs = new List<float>();
            m_sli_pro_native_support = new List<float>(); // SLI Pro support
            m_car_position = new List<float>();   // car race position
            m_kers_level = new List<float>();    // kers energy left
            m_kers_max_level = new List<float>();   // kers maximum energy
            Drs = new List<float>();     // 0 = off, 1 = on
            TractionControl = new List<float>();  // 0 (off) - 2 (high)
            m_anti_lock_brakes = new List<float>();  // 0 (off) - 1 (on)
            m_fuel_in_tank = new List<float>();   // current fuel mass
            m_fuel_capacity = new List<float>();   // fuel capacity
            m_in_pits = new List<float>();    // 0 = none, 1 = pitting, 2 = in pit area
            Sector = new List<float>();     // 0 = sector1, 1 = sector2; 2 = sector3
            TimeSector1 = new List<float>();   // time of sector1 (or 0)
            TimeSector2 = new List<float>();   // time of sector2 (or 0)
            m_brakes_temp0 = new List<float>();   // brakes temperature (centigrade)
            m_brakes_temp1 = new List<float>();   // brakes temperature (centigrade)
            m_brakes_temp2 = new List<float>();   // brakes temperature (centigrade)
            m_brakes_temp3 = new List<float>();   // brakes temperature (centigrade)
            m_wheels_pressure0 = new List<float>();  // wheels pressure PSI
            m_wheels_pressure1 = new List<float>();  // wheels pressure PSI
            m_wheels_pressure2 = new List<float>();  // wheels pressure PSI
            m_wheels_pressure3 = new List<float>();  // wheels pressure PSI
            m_team_info = new List<float>();    // team ID 
            CompletedLapsInRace = new List<float>();    // total number of laps in this race
            TrackLength = new List<float>();    // track size meters
            PreviousLapTime = new List<float>();   // last lap time
            m_max_rpm = new List<float>();    // cars max RPM, at which point the rev limiter will kick in
            m_idle_rpm = new List<float>();    // cars idle RPM
            m_max_gears = new List<float>();    // maximum number of gears
            SessionType = new List<float>();   // 0 = unknown, 1 = practice, 2 = qualifying, 3 = race
            m_drsAllowed = new List<float>();    // 0 = not allowed, 1 = allowed, -1 = invalid / unknown
            m_track_number = new List<float>();   // -1 for unknown, 0-21 for tracks
            m_vehicleFIAFlags = new List<float>();  // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red

            TurnSections = new List<int>();
        }

        public List<float> Time { get; set; }
        public List<float> LapTime { get; set; }
        public List<float> LapDistance { get; set; }
        public List<float> TotalDistance { get; set; }
        public List<float> X { get; set; }      // World space position
        public List<float> Y { get; set; }      // World space position
        public List<float> Z { get; set; }      // World space position
        public List<float> Speed { get; set; }
        public List<float> X_Velocity { get; set; }      // Velocity in world space
        public List<float> Y_Velocity { get; set; }      // Velocity in world space
        public List<float> Z_Velocity { get; set; }      // Velocity in world space
        public List<float> X_RightDirection { get; set; }      // World space right direction
        public List<float> Y_RightDirection { get; set; }      // World space right direction
        public List<float> Z_RightDirection { get; set; }      // World space right direction
        public List<float> X_ForwardDirection { get; set; }      // World space forward direction
        public List<float> Y_ForwardDirection { get; set; }      // World space forward direction
        public List<float> Z_ForwardDirection { get; set; }      // World space forward direction
        public List<float> m_susp_pos_bl { get; set; }
        public List<float> m_susp_pos_br { get; set; }
        public List<float> m_susp_pos_fl { get; set; }
        public List<float> m_susp_pos_fr { get; set; }
        public List<float> m_susp_vel_bl { get; set; }
        public List<float> m_susp_vel_br { get; set; }
        public List<float> m_susp_vel_fl { get; set; }
        public List<float> m_susp_vel_fr { get; set; }
        public List<float> m_wheel_speed_bl { get; set; }
        public List<float> m_wheel_speed_br { get; set; }
        public List<float> m_wheel_speed_fl { get; set; }
        public List<float> m_wheel_speed_fr { get; set; }
        public List<float> Throttle { get; set; }
        public List<float> Steer { get; set; }
        public List<float> Brake { get; set; }
        public List<float> Clutch { get; set; }
        public List<float> Gear { get; set; }
        public List<float> LateralAcceleration { get; set; }
        public List<float> LongitudinalAcceleration { get; set; }
        public List<float> Lap { get; set; }
        public List<float> EngineRevs { get; set; }
        public List<float> m_sli_pro_native_support { get; set; } // SLI Pro support
        public List<float> m_car_position { get; set; }   // car race position
        public List<float> m_kers_level { get; set; }    // kers energy left
        public List<float> m_kers_max_level { get; set; }   // kers maximum energy
        public List<float> Drs { get; set; }     // 0 = off, 1 = on
        public List<float> TractionControl { get; set; }  // 0 (off) - 2 (high)
        public List<float> m_anti_lock_brakes { get; set; }  // 0 (off) - 1 (on)
        public List<float> m_fuel_in_tank { get; set; }   // current fuel mass
        public List<float> m_fuel_capacity { get; set; }   // fuel capacity
        public List<float> m_in_pits { get; set; }    // 0 = none, 1 = pitting, 2 = in pit area
        public List<float> Sector { get; set; }     // 0 = sector1, 1 = sector2 { get; set; } 2 = sector3
        public List<float> TimeSector1 { get; set; }   // time of sector1 (or 0)
        public List<float> TimeSector2 { get; set; }   // time of sector2 (or 0)
        public List<float> m_brakes_temp0 { get; set; }   // brakes temperature (centigrade)
        public List<float> m_brakes_temp1 { get; set; }   // brakes temperature (centigrade)
        public List<float> m_brakes_temp2 { get; set; }   // brakes temperature (centigrade)
        public List<float> m_brakes_temp3 { get; set; }   // brakes temperature (centigrade)
        public List<float> m_wheels_pressure0 { get; set; }  // wheels pressure PSI
        public List<float> m_wheels_pressure1 { get; set; }  // wheels pressure PSI
        public List<float> m_wheels_pressure2 { get; set; }  // wheels pressure PSI
        public List<float> m_wheels_pressure3 { get; set; }  // wheels pressure PSI
        public List<float> m_team_info { get; set; }    // team ID 
        public List<float> CompletedLapsInRace { get; set; }    // total number of laps in this race
        public List<float> TrackLength { get; set; }    // track size meters
        public List<float> PreviousLapTime { get; set; }   // last lap time
        public List<float> m_max_rpm { get; set; }    // cars max RPM, at which point the rev limiter will kick in
        public List<float> m_idle_rpm { get; set; }    // cars idle RPM
        public List<float> m_max_gears { get; set; }    // maximum number of gears
        public List<float> SessionType { get; set; }   // 0 = unknown, 1 = practice, 2 = qualifying, 3 = race
        public List<float> m_drsAllowed { get; set; }    // 0 = not allowed, 1 = allowed, -1 = invalid / unknown
        public List<float> m_track_number { get; set; }   // -1 for unknown, 0-21 for tracks
        public List<float> m_vehicleFIAFlags { get; set; }  // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red

#region Custom Properties
        public List<int> TurnSections { get; set; }
#endregion

        public void Add(TelemetryPacket packet)
        {
            Time.Add(packet.Time);
            TimeSector1.Add(packet.TimeSector1);
            TimeSector2.Add(packet.TimeSector2);
        }
    }
}
