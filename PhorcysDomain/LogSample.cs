namespace Phorcys.Domain
{
    public class LogSample
    {
        public int LogSampleId { get; set; }
        public int DiveComputerLogId { get; set; }
        public int ElapsedSeconds { get; set; }
        public decimal Depth { get; set; }
        public decimal FirstDecoStopDepth { get; set; }
        public int TimeToSurfaceMinutes { get; set; }
        public decimal AvgPPO2 { get; set; }
        public decimal FractionO2 { get; set; }
        public decimal FractionHe { get; set; }
        public int FirstDecoStopMinutes { get; set; }
        public int NoDecoLimitMinutes { get; set; }
        public short CircuitMode { get; set; }
        public short CCRMode { get; set; }
        public int Temperature { get; set; }
        public bool GasSwitchNeeded { get; set; }
        public bool ExternalPPO2Active { get; set; }
        public short SetPointType { get; set; }
        public short CircuitSwitchType { get; set; }
        public int O2Sensor1Millivolts { get; set; }
        public int O2Sensor2Millivolts { get; set; }
        public int O2Sensor3Millivolts { get; set; }
        public decimal BatteryVoltage { get; set; }
        public decimal AscentRate { get; set; }
        public decimal SafeAscentDepth { get; set; }
        public int CO2Millibar { get; set; }

        public virtual DiveComputerLog? DiveComputerLog { get; set; }
    }
}
