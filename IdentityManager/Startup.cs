using IdentityManager.Authorize;
using IdentityManager.Data;
using IdentityManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManager
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IEmailSender, MailJetEmailSender>();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireLowercase = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                opt.Lockout.MaxFailedAccessAttempts = 2;
            });
            services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Accessdenied");
            });
            services.AddAuthentication().AddFacebook(optoin =>
            {
                optoin.AppId = "784757506684192";
                optoin.AppSecret = "896be33fb7a7792ed9c332981ac6df64";
            });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                option.AddPolicy("UserandAdmin", policy => policy.RequireRole("Admin").RequireRole("User"));
                option.AddPolicy("Admin_CreateAccess", policy => policy.RequireRole("Admin").RequireClaim("create","True"));
                option.AddPolicy("Admin_Create_Edite_DeleteAccess", policy => policy.RequireRole("Admin").RequireClaim("create","True")
                .RequireClaim("edit","True")
                .RequireClaim("Delete","True"));
                option.AddPolicy("Admin_Create_Edite_DeleteAccess_SuperAdmin", policy => policy.RequireAssertion(context => 
                AuthorizeAdminWithClaimsOrSuperAdmin(context)));
                option.AddPolicy("OnlySuperAdminChecker", policy => policy.Requirements.Add(new OnlySuperAdminChecker()));
                option.AddPolicy("AdminWithMoreThan1000Days", policy => policy.Requirements.Add(new AdminWithMoreThan1000DaysRequirement(1000)));
                option.AddPolicy("FirstNameAuth", policy => policy.Requirements.Add(new FirstNameAuthRequirement("billy")));
            });
            services.AddScoped<IAuthorizationHandler , AdminWithMoreThan1000DaysHandler>();
            services.AddScoped<IAuthorizationHandler , FirsNameAuthHandler>();
            services.AddScoped<INumberofDaysForAccount, NumberofDaysForAccount>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private bool AuthorizeAdminWithClaimsOrSuperAdmin(AuthorizationHandlerContext context)
        {
            return (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == "Create" && c.Value == "True")
                && context.User.HasClaim(c => c.Type == "Edit" && c.Value == "True")
                && context.User.HasClaim(c => c.Type == "Delete" && c.Value == "True")
                ) || context.User.IsInRole("SuperAdmin");
        }
    }
}
