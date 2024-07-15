using Phorcys.Data;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Phorcys.Services
{
	public class MyCertificationServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;

		public MyCertificationServices(PhorcysContext context, ILogger<MyCertificationServices> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<vwMyCertification> GetMyCerts(int userId)
		{
			try
			{
				var myCerts = _context.vwMyCertifications
					.Where(c => c.UserId == userId)
					.OrderBy(c => c.Certified).ToList();

				return myCerts;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retreiving Certifications for user {userId}" + ex.Message, userId);
				throw new Exception("Can't connect to database");
			}
		}
	}
}
