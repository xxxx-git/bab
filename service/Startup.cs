using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services;
using Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace bab
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            BabServices(services);
            BabSettings(services);
            BabAuthentication(services);
            services.AddCors(o => o.AddPolicy("AllowCorsPolicy", builder => {
                
                builder
                .WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowCorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void BabAuthentication(IServiceCollection services) 
        {
            // // services.AddIdentity<, >()
            // services.AddAuthentication(auth => {
            //     auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //     auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //     auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     auth.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            // .AddCookie(options => {
            //     // options.
            // })
            // .AddJwtBearer(options =>
            // {
            //     var signingKey = System.Text.Encoding.UTF8.GetBytes(Configuration["Authorization:Secret"]);
            //     options.RequireHttpsMetadata = false;
            //     // options.SaveToken = true;
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         // ValidIssuer = Configuration["Authorization:Issuer"],
            //         ValidateIssuer = false,
            //         ValidateAudience = false,
            //         IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            //     };
            // });
        }

        private IServiceCollection BabServices(IServiceCollection container) 
        {
            // Scoped - Initial per session
            container.AddScoped<ISecurityTokenService, SynTokenService>();

            // Transient - Initial per dependency
            container.AddTransient<IJsonCovnertService, SynJsonConvertService>();

            return container;
        }

        private IServiceCollection BabSettings(IServiceCollection container)
        {
            var settings = new Settings();
            Configuration.Bind("JWTSettings:Header", settings.Headers);
            Configuration.Bind("JWTSettings:Payload", settings.Claims);
            Configuration.Bind("JWTSettings:Http", settings.Http);
            Configuration.Bind("JWTSettings", settings);
            container.AddSingleton<ITokenSettings>(settings);
            container.AddSingleton<ITokenHttpSettings>(settings.Http);

            return container;
        }
    }
}
