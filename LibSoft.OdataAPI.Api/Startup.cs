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
            builder.EntitySet<Category>("Categories"); // Controllers ismi verilmelidir. Ýlgili Class'ý belirtiyoruz.
            builder.EntitySet<Product>("Products");

            //../odata/categories(1)/totalProductPrice // opsiyonel url'i burada belirtebiliriz.
            builder.EntityType<Category>().Action("TotalProductPrice").Returns<int>();
           
            // Collection Þeklinde Döner geriye dönüþ deðeri <int> tipindedir.  
            builder.EntityType<Category>().Collection.Action("TotalProductPriceCollect").Returns<int>();


            // ../odata/categories/TotalProductPriceParam deðeri body den göndermek. Parametre Alan Action.
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
                /* Ayrý Ayrý'da kullanýlabilir. Aþaðýdaki þekilde extension metot þeklinde de kullanýlabilir.
                 * 
                endpoints.Select().OrderBy() // belirtilen kolona göre order by iþlemi yapar.

                endpoints.Select().Expand(); // Ýliþkili olan tablolarý belirtilen þekilde birleþtirerek getirir.

                endpoints.Select().MaxTop(10); // Max 10 veri getirir. null verilirse herhangi bir kýsýtlama gerektirmez.

                 endpoints.Select().Count(); // Veri Sayýsýný bildirir. Query'de Count=false dersekbu özelliði bildirmez. Response'da @odata.count key'i altýnda görünür.

                endpoints.Select().Filter(); // Filtreleme iþlemi yapar. eq ön ekini alýr.

                endpoints.Select(); // OData tarafýndan Select sorgusunun kullanýlmasý için eklenmelidir.
                */

                endpoints.Select().Expand().OrderBy().MaxTop(null).Count().Filter();
                endpoints.MapODataRoute("odata", "odata", builder.GetEdmModel());
                endpoints.MapControllers();
            });
        }
    }
}
