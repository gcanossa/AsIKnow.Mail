using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsIKnow.Mail;
using AsIKnow.XUnitExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

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
            var appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.SetupGet<IServiceProvider>(p => p.ApplicationServices).Returns(services.BuildServiceProvider());
            services.AddSingleton<IApplicationBuilder>(appBuilderMock.Object);

            services.AddLogging(p => p.AddDebug());

            services.AddOptions();
            
            services.Configure<MailOptions>(Configuration.GetSection("Mail"));
            
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddMailTemplates();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #endregion

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMailTemplates();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
