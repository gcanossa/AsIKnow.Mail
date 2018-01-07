using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.IO;

namespace AsIKnow.Mail
{
    public static class MailTemplateHelper
    {
        public static IApplicationBuilder UseMailTemplates(this IApplicationBuilder ext, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            return ext;
        }

        private static IServiceProvider ServiceProvider { get; set; }

        public static async Task<string> RenderInView(this ViewDataDictionary ext, string path)
        {
            return await RenderView(path, ext);
        }

        public static async Task<string> RenderView(string path, ViewDataDictionary viewDataDictionary)
        {
            if (ServiceProvider == null)
                throw new InvalidOperationException($"In order to use the facade, {nameof(UseMailTemplates)} must be called in Startup.Configure method. ");

            using (IServiceScope scope = ServiceProvider.CreateScope())
            {
                MailOptions options = scope.ServiceProvider.GetRequiredService<IOptions<MailOptions>>().Value;
                ICompositeViewEngine viewEngine = scope.ServiceProvider.GetRequiredService<ICompositeViewEngine>();
                ITempDataProvider tempDataProvider = scope.ServiceProvider.GetRequiredService<ITempDataProvider>();
                IHttpContextAccessor httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                ActionContext actionContext = new ActionContext(httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());
                viewDataDictionary.Model = null;

                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = viewEngine.GetView($"{options.ViewTemplateBasePath}/{path}", $"{options.ViewTemplateBasePath}/{path}", true);

                    ViewContext viewContext = new ViewContext(actionContext, viewResult.View, viewDataDictionary, new TempDataDictionary(httpContextAccessor.HttpContext, tempDataProvider), sw, new HtmlHelperOptions());

                    await viewResult.View.RenderAsync(viewContext);
                    sw.Flush();

                    if (viewContext.ViewData != viewDataDictionary)
                    {
                        var keys = viewContext.ViewData.Keys.ToArray();
                        foreach (var key in keys)
                        {
                            viewDataDictionary[key] = viewContext.ViewData[key];
                        }
                    }

                    return sw.ToString();
                }
            }
        }
    }
}
