using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Phorcys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phorcys.Web.Controllers
{
    public class GridController : Controller
    {
        public ActionResult Orders_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Enumerable.Range(0, 50).Select(i => new DiveViewModel
            {
                DiveId = i,
                DiveNumber = i * 10,
                DescentTime = new DateTime(2016, 9, 15).AddDays(i % 7),
                DivePlanTitle = "Warmup Dive " + i,
                DiveSite = "Jackson Blue ",
                Minutes = 65,
                MaxDepth = 95,
                UserName = "Larry Hack"
            });

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult);
        }
    }
}
