using Phorcys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Phorcys.Domain;
using Microsoft.EntityFrameworkCore;
using Telerik.SvgIcons;

namespace Phorcys.Services
{
	public class AgencyServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;

		public AgencyServices(PhorcysContext context, ILogger<AgencyServices> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<DiveAgency> GetAgencies()
		{
			try
			{
				var agencies = _context.DiveAgencies.Include(da => da.Contact)
													.Include(da => da.Certifications)
					.OrderBy(da => da.Contact.Company).ToList();

				return agencies;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving Dive Agencies" + ex.Message);
				throw new Exception("Can't connect to database");
			}
		}

		public DiveAgency GetAgency(int id)
		{
			var agency = _context.DiveAgencies.FirstOrDefault(a => a.DiveAgencyId == id);
			return agency;
		}
	}
}
