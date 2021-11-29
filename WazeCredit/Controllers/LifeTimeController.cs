using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WazeCredit.Service.LifeTimeExample;

namespace WazeCredit.Controllers
{
    public class LifeTimeController : Controller
    {
        private readonly TransientService _transientService;
        private readonly ScopedService _scopedService;
        private readonly SingletonService _singletonService;

        public LifeTimeController(TransientService transientService
            , ScopedService scopedService, SingletonService singletonService)
        {
            _transientService = transientService;
            _scopedService = scopedService; 
            _singletonService = singletonService;   
        }

        public IActionResult Index()
        {
            List<string> messages = new List<string>
            {
                HttpContext.Items["CustomMiddleWareTransient"].ToString(),
                $"Transient Controller - {_transientService.GetGuid()}",
                HttpContext.Items["CustomMiddleWareSingleton"].ToString(),
                $"Singleton Controller - {_singletonService.GetGuid()}",
                HttpContext.Items["CustomMiddleWareScoped"].ToString(),
                $"Scoped Controller - {_scopedService.GetGuid()}"
            };

            return View(messages);
        }
    }
}
