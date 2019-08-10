using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ILoggerFactory _factory = null;
        protected ILogger<BaseController> _ilogger = null;
        protected IServiceTest _serviceTest = null;

        public BaseController(ILoggerFactory factory, ILogger<BaseController> ilogger, IServiceTest serviceTest)
        {
            _factory = factory;
            _ilogger = ilogger;
            _serviceTest = serviceTest;
        }
    }
}
