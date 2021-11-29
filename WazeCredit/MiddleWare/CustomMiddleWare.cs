using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WazeCredit.Service.LifeTimeExample;

namespace WazeCredit.MiddleWare
{
    public class CustomMiddleWare
    {

        /// <summary>
        /// RequestDelegate is a function that can process an HTTP request.
        /// </summary>
        private readonly RequestDelegate _next;
        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// add value to contain httpcontext
        /// </summary>
        /// <param name="context">A HttpContext object holds information about the current HTTP request,
        /// whenever we make a new HTTP request or response then the Httpcontext object is created </param>
        /// <param name="transientService"></param>
        /// <param name="scopedService"></param>
        /// <param name="singletonService"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, TransientService transientService
            , ScopedService scopedService, SingletonService singletonService)
        {
            context.Items.Add("CustomMiddleWareTransient", "Transient MiddleWare - "+ transientService.GetGuid());
            context.Items.Add("CustomMiddleWareSingleton", "Singleton MiddleWare - "+ singletonService.GetGuid());
            context.Items.Add("CustomMiddleWareScoped", "Scoped MiddleWare - "+ scopedService.GetGuid());

            await _next(context);
        }

    }
}
