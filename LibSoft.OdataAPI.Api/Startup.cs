using LibSoft.OdataAPI.Api.Model;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibSoft.OdataAPI.Api
{
    public class Startup
    {



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
         
        }

        public IConfiguration Configuration { get; }

      

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["zd3ymSJUIyArMsPlTkvKhg==:JtaIi97a0dtoR52hlj67Vg=="]);
            });

            services.AddOData();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Category>("Categories"); // Controllers ismi verilmelidir. �lgili Class'� belirtiyoruz.
            builder.EntitySet<Product>("Products");

            //../odata/categories(1)/totalProductPrice // opsiyonel url'i burada belirtebiliriz.
            builder.EntityType<Category>().Action("TotalProductPrice").Returns<int>();
           
            // Collection �eklinde D�ner geriye d�n�� de�eri <int> tipindedir.  
            builder.EntityType<Category>().Collection.Action("TotalProductPriceCollect").Returns<int>();


            // ../odata/categories/TotalProductPriceParam de�eri body den g�ndermek. Parametre Alan Action.
            builder.EntityType<Category>().Collection.Action("TotalProductPriceParam").Returns<int>().Parameter<int>("categoryId");

            // opsiyonel olarak birden fazla parametreyi toplatabiliriz.

            var actionTotal = builder.EntityType<Category>().Collection.Action("Total").Returns<int>();

            actionTotal.Parameter<int>("a");
            actionTotal.Parameter<int>("b");
            actionTotal.Parameter<int>("c");

            // Complex Type Parametreler


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                /* Ayr� Ayr�'da kullan�labilir. A�a��daki �ekilde extension metot �eklinde de kullan�labilir.
                 * 
                endpoints.Select().OrderBy() // belirtilen kolona g�re order by i�lemi yapar.

                endpoints.Select().Expand(); // �li�kili olan tablolar� belirtilen �ekilde birle�tirerek getirir.

                endpoints.Select().MaxTop(10); // Max 10 veri getirir. null verilirse herhangi bir k�s�tlama gerektirmez.

                 endpoints.Select().Count(); // Veri Say�s�n� bildirir. Query'de Count=false dersekbu �zelli�i bildirmez. Response'da @odata.count key'i alt�nda g�r�n�r.

                endpoints.Select().Filter(); // Filtreleme i�lemi yapar. eq �n ekini al�r.

                endpoints.Select(); // OData taraf�ndan Select sorgusunun kullan�lmas� i�in eklenmelidir.
                */

                endpoints.Select().Expand().OrderBy().MaxTop(null).Count().Filter();
                endpoints.MapODataRoute("odata", "odata", builder.GetEdmModel());
                endpoints.MapControllers();
            });
        }
    }
}
