using Api.Services.Abstractions;
using Api.Services.Auth0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Api
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
            services.AddControllers();

            services.AddHttpClient<IUserService, UserService>(c =>
            {
                c.BaseAddress = new Uri(Configuration["Auth0:Authority"]);
            });
            services.AddHttpClient<TokenService>(c =>
            {
                c.BaseAddress = new Uri(Configuration["Auth0:Authority"]);
            });

            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                Configuration.Bind("Auth0", options);
                
                //uncomment to disable audience validation 
                //options.TokenValidationParameters = new TokenValidationParameters()
                //{
                //    ValidateAudience = false,
                //};
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["Client:Origin"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
