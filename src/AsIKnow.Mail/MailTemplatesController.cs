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
        public async Task<IActionResult> ViewTest(string path)
        {
            if (!Env.IsProduction())
            {
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

                foreach (var item in Request.Query)
                {
                    viewDataDictionary.Add(item.Key, item.Value);
                }

                return Content(await MailTemplateHelper.RenderView(path, viewDataDictionary), "text/html");
            }
            else
            {
                return NotFound();
            }
        }
    }
}