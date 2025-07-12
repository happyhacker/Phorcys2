using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Services
{
	public class DiveTypeServices
	{
		private readonly PhorcysContext _context;
		private readonly ILogger _logger;

		public DiveTypeServices(PhorcysContext context, ILogger logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<DiveType> GetDiveTypes(int userId) 
		{ 
			var diveTypes = _context.DiveTypes.Where(d => d.UserId == userId);
			return diveTypes;
		}

		public IEnumerable<DiveTypeDto> GetDiveTypeDtos(int userId)
		{
			var diveTypes = GetDiveTypes(userId);
			var diveTypeDtos = new List<DiveTypeDto>();
			foreach(var d in diveTypes) 
			{
				diveTypeDtos.Add(new DiveTypeDto
				{
					DiveTypeId = d.DiveTypeId,
					Title = d.Title
				});
			}
		
			return diveTypeDtos;
		}
	}
}
