using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;

namespace AsIKnow.Mail
{
    public class MailTemplatesController : Controller
    {
        public IHostingEnvironment Env { get; set; }
        public MailTemplatesController(IHostingEnvironment env)
        {
            Env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ViewTest(string viewTemplatePath)
        {
            if (!Env.IsProduction())
            {
                return Content(
                    await MailTemplateHelper.RenderView(viewTemplatePath, 
                    Request.Query.Where(p => p.Key != "viewTemplatePath").ToDictionary(p=>p.Key, p=>p.Value).AsViewData()), 
                    "text/html");
            }
            else
            {
                return NotFound();
            }
        }
    }
}