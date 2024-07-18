using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Domain;
using Telerik.SvgIcons;

namespace Phorcys.Services
{
    public class InstructorServices
    {
        private readonly PhorcysContext _context;
        private readonly ILogger _logger;

        public InstructorServices(PhorcysContext context, ILogger<InstructorServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Instructor> GetInstructors()
        {
            try
            {
                var instructors = _context.Instructors.Include(i => i.Contact).OrderBy(i => i.Contact.LastName)
                                                      .ThenBy(i => i.Contact.FirstName)
                                                      .ToList();
                return instructors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retreiving Instructors" + ex.Message);
                throw new Exception("Can't connect to database");
            }
        }
    }
}