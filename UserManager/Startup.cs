using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Data;
using UserManager.Models;
using UserManager.Policies;
using UserManager.Profiles;

namespace UserManager
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
            //Connect Db
            services.AddDbContext<UserContext>(option =>
            option.UseSqlServer(Configuration.GetConnectionString("UserContextConnection")));

            //Identity 
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()                
                .AddDefaultTokenProviders(); //Provide token;

            //Add policies to config userName, email
            services.AddTransient<IUserValidator<User>, CustomPolicy>();

            //Config Password
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true; //Unique email in Db
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            });

            //Auto Mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CRUDProfile());
                mc.AddProfile(new AuthProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Dependncy Injection
            services.AddScoped<IUserRepo, SqlUserRepo>();

            //JWT reference: https://medium.com/swlh/all-you-need-to-know-about-json-web-token-jwt-8a5d6131157f
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsThinhBoF"))
                };
            });

            //MailKit
            var mailKitOptions = Configuration.GetSection("Email").Get<MailKitOptions>();            
            services.AddMailKit(config => config.UseMailKit(mailKitOptions));

            //Add Controllers
            services.AddControllers();            
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
