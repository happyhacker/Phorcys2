using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Phorcys.Data;

namespace Phorcys.Services
{
	public class InstructorServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;

		public InstructorServices(PhorcysContext context, ILogger logger)
		{
			_context = context;
			_logger = logger;
		}
	}
}
