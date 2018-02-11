using AsIKnow.Mail;
using AsIKnow.XUnitExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest
{
    public interface Dependency
    { }
    public class DependencyFixture : DependencyInjectionBaseFixture<Dependency>
    {
        protected override void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);


            #region general

            services.AddSingleton<IHostingEnvironment>(new Mock<IHostingEnvironment>().Object);
            services.AddSingleton<IApplicationBuilder>(new Mock<IApplicationBuilder>().Object);

            services.AddLogging(p => p.AddDebug());

            services.AddOptions();
            
            services.Configure<MailOptions>(Configuration.GetSection("Mail"));
            
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            
            #endregion

        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseMailTemplates(serviceProvider);

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
