using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs;

public class TanksOnDiveDto
{
	public int GearId { get; set; }
	public int? StartingPressure { get; set; }
	public int? EndingPressure { get; set; }
	public string? GasContentTitle { get; set; }
}

