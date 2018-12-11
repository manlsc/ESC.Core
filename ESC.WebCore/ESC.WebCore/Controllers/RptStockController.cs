using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ESC.WebCore.Controllers
{
    /// <summary>
    /// 库存报表 出入库
    /// </summary>
    public class RptStockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}