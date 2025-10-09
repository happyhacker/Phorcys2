using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs;

public class TanksOnDiveDto
{
	public int GearId { get; set; }
    public string? GearTitle { get; set; }
    public int StartingPressure { get; set; } = 0;
	public int EndingPressure { get; set; } = 0;
	public string? GasContentTitle { get; set; }
    public int OxygenPercent { get; set; } = 0;
    public int HeliumPercent { get; set; } = 0;
}

