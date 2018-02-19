
namespace Web1.Controllers
{
    using System;
    using IStatelessBackendService.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Fabric;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.ServiceFabric.Services.Remoting.Client;
    using Microsoft.ServiceFabric.Services.Remoting;

    using System.Net.Http;
    using System.Diagnostics;
    using Newtonsoft.Json;
    public class HomeController : Controller
    {

        private readonly ConfigSettings configSettings;
        private readonly StatelessServiceContext serviceContext;
       // private readonly IServiceProxyFactory serviceProxyFactory;

        public HomeController(StatelessServiceContext serviceContext, ConfigSettings settings)
        {
            this.serviceContext = serviceContext;
            this.configSettings = settings;
           // this.serviceProxyFactory = new ServiceProxyFactory();
        }
        public async Task<IActionResult> Index()
        {
            string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/" + this.configSettings.StatelessBackendServiceName;
            IStatelessBackendService proxy = ServiceProxy.Create<IStatelessBackendService>(new Uri(serviceUri));

            ServiceEventSource.Current.ServiceMessage(this.serviceContext, "In the web service about to call the backend!");

            long result = 0;
            int timeout = 10000;
            var task = proxy.GetCountAsync();
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                result = task.GetAwaiter().GetResult();
            }
            else
            {
                throw new TimeoutException("This is taking unusually long. Something is wrong");
            }


            ViewBag.Number = result;



            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            //ViewBag.CorrelationId = Activity.Current.RootId;

            return View();
        }
    }
}