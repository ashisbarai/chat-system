using System;
using System.IO;
using System.Reflection;
using Chat.Api.Web.Hubs;
using Chat.Api.Web.Middlewares;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chat.Api.Web
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
            //IdentityModelEventSource.ShowPII = true;
            Bootstrapper.Build(services, Configuration);
            services.AddCors();

            services.AddControllers();
            services.AddOptions();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddSignalR();

            services.AddHttpContextAccessor();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44368";
                    //options.ApiName = "infra-doc-chng-mgmt-sys";
                    //options.RequireHttpsMetadata = false;
                });

            //services.AddMvc(option =>
            //    {
            //        var policy = new AuthorizationPolicyBuilder()
            //            .RequireAuthenticatedUser()
            //            .Build();

            //        option.Filters.Add(new AuthorizeFilter(policy));

            //        option.EnableEndpointRouting = false;
            //    });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("ChatOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Chat API",
                        Version = "1"
                    });

                setupAction.OperationFilter<ApiKeyHeaderFilter>();

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.InjectStylesheet("/assets/custom-ui.css");
                setupAction.IndexStream = () => GetType().Assembly.GetManifestResourceStream("Chat.Api.Web.EmbeddedAssets.index.html");

                setupAction.SwaggerEndpoint("/swagger/Chat/swagger.json", "Chat API");
                setupAction.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatsocket");
            });
        }
    }
}
