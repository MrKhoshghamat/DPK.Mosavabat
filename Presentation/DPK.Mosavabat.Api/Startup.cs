using DPK.Foundation.Castle.Logging.Log4Net;
using DPK.Foundation.Identity;
using DPK.Foundation.Identity.Authentication;
using DPK.Foundation.Identity.Extentions.IdentityExtentions;
using DPK.Mosavabat.Api.Extensions;
using DPK.Mosavabat.Common.Consts;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using DPK.Mosavabat.Api.Middlewares.Logging;
using DPK.Mosavabat.BusinessServices.Pipelines.Logging;
using DPK.Mosavabat.EntityFrameworkCore.Logging;
using DPK.Mosavabat.ExternalServices;
using DPK.Mosavabat.ExternalServices.Logging;
using MediatR;

namespace DPK.Mosavabat.Api;

/// <summary>
/// Start up class
/// </summary>
public class Startup
{

    private readonly IConfigurationRoot _appConfiguration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="env"></param>
    public Startup(IWebHostEnvironment env)
    {
        _hostingEnvironment = env;
        _appConfiguration = env.GetAppConfiguration();
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("fa");
    }

    /// <summary>
    /// Services configurations
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        services.AddTransient<ExternalServiceLoggingHandler>();

        services.AddSingleton<SqlServiceLoggingInterceptor>();
        services.AddScoped<IdentityErrorDescriber>();

        services.AddLcpProxyServices(_appConfiguration);
        services.AddLmsProxyServices(_appConfiguration);

        ConfigureSerilog();

        services.AddControllers()
            .AddNewtonsoftJson();

        IdentityRegistrar.Register(services);

        services.AddEndpointsApiExplorer();


        services.AddCorsPolicy(_appConfiguration);

        if (bool.Parse(_appConfiguration["KestrelServer:IsEnabled"] ?? string.Empty))
        {
            ConfigureKestrel(services);
        }

        AuthConfigurer.Configure(services, _appConfiguration);

        if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"] ?? string.Empty))
        {
            //IdentityServerRegistrar.Register(services, _appConfiguration, options =>
            //    options.UserInteraction = new UserInteractionOptions()
            //    {
            //        LoginUrl = "/UI/Login",
            //        LogoutUrl = "/UI/LogOut",
            //        ErrorUrl = "/Error"
            //    });
        }
        else
        {
            services.Configure<SecurityStampValidatorOptions>(opts =>
            {
                opts.OnRefreshingPrincipal = SecurityStampValidatorCallback.UpdatePrincipal;
            });
        }

        if (MosavabatConsts.SwaggerUiEnabled)
        {
            ConfigureSwagger(services);
        }

        return services.AddDpk<DpkApiLayerModule>(options =>
        {
            options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseDpkLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                    ? "log4net.config"
                    : "log4net.Production.config")
            );
            options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"),
                SearchOption.AllDirectories);
        });
    }

    /// <summary>
    /// Configure main method
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="options"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IOptions<IdentityOptions> options)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();

        //Initializes DPK framework.
        options.Value.ClaimsIdentity.SecurityStampClaimType = AuthConsts.SecurityStampKey;
        app.UseDpk(builderOptions =>
        {
            builderOptions.UseDpkRequestLocalization = false;
            //عدم نمایش Header  ها 
            builderOptions.UseSecurityHeaders = false;
        });
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
            app.UseExceptionHandler("/Error");
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        //Enable Cors
        app.UseCors(StartUpExtension.DefaultCorsPolicyName);

        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"] ?? string.Empty))
        {
            app.UseIdentityServer();
        }
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
        });

        if (MosavabatConsts.SwaggerUiEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
            });
        }
        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

    }

    private void ConfigureSerilog()
    {
        var connectionString = _appConfiguration.GetConnectionString("DefaultConnection");

        var columnOptions = new ColumnOptions
        {
            // تعریف ستون‌های سفارشی
            AdditionalColumns =
            [
                new SqlColumn { ColumnName = "Layer", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                new SqlColumn { ColumnName = "CorrelationId", DataType = SqlDbType.NVarChar, DataLength = 100, AllowNull = true },
                new SqlColumn { ColumnName = "RequestName", DataType = SqlDbType.NVarChar, DataLength = 200, AllowNull = true },
                new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 100, AllowNull = true },
                new SqlColumn { ColumnName = "ClientIP", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                new SqlColumn { ColumnName = "ExternalSystem", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                new SqlColumn { ColumnName = "ElapsedMilliseconds", DataType = SqlDbType.Float, AllowNull = true },
                new SqlColumn { ColumnName = "StatusCode", DataType = SqlDbType.Int, AllowNull = true },
                new SqlColumn { ColumnName = "ErrorType", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                new SqlColumn { ColumnName = "ErrorKey", DataType = SqlDbType.NVarChar, DataLength = 200, AllowNull = true },
                new SqlColumn { ColumnName = "BusinessErrors", DataType = SqlDbType.NVarChar, DataLength = int.MaxValue, AllowNull = true },
                new SqlColumn { ColumnName = "Metadata ", DataType = SqlDbType.NVarChar, DataLength = int.MaxValue, AllowNull = true }
            ]
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "AppLogs",
                    AutoCreateSqlTable = true
                },
                columnOptions: columnOptions)
            .CreateLogger();
    }

    private void ConfigureKestrel(IServiceCollection services)
    {
        services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
        {
            options.Listen(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 443),
                listenOptions =>
                {
                    var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
                    var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
                    if (certPath != null)
                    {
                        var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath,
                            certPassword);
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
                        {
                            ServerCertificate = cert
                        });
                    }
                });
        });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Infra API", Version = "v1" });
            options.DocInclusionPredicate((_, _) => true);
            options.ParameterFilter<SwaggerEnumParameterFilter>();
            options.SchemaFilter<SwaggerEnumSchemaFilter>();
            options.OperationFilter<SwaggerOperationIdFilter>();
            options.OperationFilter<SwaggerOperationFilter>();
            options.CustomDefaultSchemaIdSelector();

            //add summaries to swagger
            bool canShowSummaries = _appConfiguration.GetValue<bool>("Swagger:ShowSummaries");
            if (canShowSummaries)
            {
                var hostXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var hostXmlPath = Path.Combine(AppContext.BaseDirectory, hostXmlFile);
                options.IncludeXmlComments(hostXmlPath);
            }
        }).AddSwaggerGenNewtonsoftSupport();
    }
}