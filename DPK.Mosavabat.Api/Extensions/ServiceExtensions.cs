using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Entities.ConfigurationModels;
using DPK.Mosavabat.Entities.Models;
using DPK.Mosavabat.External.Services.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.External.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Lcp;
using DPK.Mosavabat.External.Services.Mosavabat;
using DPK.Mosavabat.LoggerService;
using DPK.Mosavabat.Presentation;
using DPK.Mosavabat.Presentation.ActionFilters;
using DPK.Mosavabat.Repository;
using DPK.Mosavabat.Services;
using DPK.Mosavabat.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Lms;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.External.Services.Lms;
using DPK.Mosavabat.External.Services.Payam;
using DPK.Mosavabat.Shared.DataModels.Lms;
using ContractServices = DPK.Mosavabat.External.Services.Mosavabat.ContractServices;
using IContractServices = DPK.Mosavabat.External.Services.Contracts.Mosavabat.IContractServices;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;
using ServiceProvider = DPK.Mosavabat.External.Services.Lcp.ServiceProvider;
using DPK.Mosavabat.External.Services.Abrisham.Loan;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Jobs;
using FluentValidation.AspNetCore;
using DPK.Mosavabat.Shared.Validation.CreditProcesses;
using DPK.Mosavabat.Services.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Quartz;
using DPK.Mosavabat.Services.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Bsi;
using CreditProcessesService = DPK.Mosavabat.Services.CreditProcesses.CreditProcessesService;

namespace DPK.Mosavabat.Api.Extensions;

public static class ServiceExtensions
{
    public static IConfiguration? Configuration { get; set; }

    public static void Initialize()
    {
        Configuration = new ConfigurationManager();

    }
    //public static string? SecretKey = Configuration?["Jwt:Secret"];
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
            // .WithOrigins("https://example.com")
            // .WithMethods("POST" , "GET")
            // .WithHeaders("accept", "content-type")
        });
    }

    public static void ConfigureMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    public static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddScoped<ProcessLoanBillJobScopedService>();

        services.AddQuartz(q =>
        {
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();

            var jobKey = new JobKey("ProcessLoanBillJob", "LoanBills");
            q.AddJob<ProcessLoanBillJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("ProcessLoanBillTrigger", "LoanBills")
                .WithCronSchedule("*/5 * * * * ?"));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.StartDelay = TimeSpan.FromSeconds(5);
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }

    // ReSharper disable once InconsistentNaming
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        Initialize();
        var jwtConfiguration = new JwtConfiguration();
        configuration.Bind(jwtConfiguration.Section, jwtConfiguration);
        //var jwtSettings = configuration.GetSection("JwtSettings");
        //var secretKey = Environment.GetEnvironmentVariable("SECRET");
        const string secretKey = "239ff13f6b64c1cb118fe26df5df9bc3ce542172a40d20b2b6d4529df21020d3";
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
        }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.EnableAnnotations();
            s.SwaggerDoc("in-lcp-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های ارایه شده توسط پرتال حقوقی مشتریان",
                    Version = "v1"
                });
            s.SwaggerDoc("bsi-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های ارائه شده توسط بانک صادرات",
                    Version = "v1"
                });
            s.SwaggerDoc("for-lcp-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های مربوط به مشتریان حقوقی ورژن 1",
                    Version = "v1"
                });
            s.SwaggerDoc("for-lcp-v2",
                new OpenApiInfo
                {
                    Title = "سرویس های مربوط به مشتریان حقوقی ورژن 2",
                    Version = "v2"
                });
            s.SwaggerDoc("in-mosavabat-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های مربوط به مصوبات",
                    Version = "v1"
                });
            s.SwaggerDoc("for-credit-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های مربوط به کنترل پرداخت",
                    Version = "v1"
                });

            s.SwaggerDoc("for-abrisham-v1",
                new OpenApiInfo
                {
                    Title = "سرویس های مربوط به ابریشم",
                    Version = "v1"
                });

            s.DocumentFilter<OrderTagsDocumentFilter>();

            var xmlFile = $"{typeof(AssemblyReference).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            s.IncludeXmlComments(xmlPath);

            s.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void ConfigFluentValidation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation(cfg =>
            cfg.RegisterValidatorsFromAssemblyContaining<GetTermsAndConditionsApprovalRequestListValidator>());
    }

    public static void ConfigureCreditProcessesDatabase(this IServiceCollection services) => services.AddScoped<ICreditProcessService, CreditProcessesService>();

    public static void ConfigureCreditProcessesService(this IServiceCollection services) => services.AddScoped<ICreditProcessesDatabase, CreditProcessesDatabase>();

    // ReSharper disable once InconsistentNaming
    public static void ConfigureIISIntegration(this IServiceCollection services) => services.Configure<IISOptions>(_ => { });

    public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureMosavabatApprovalRequestService(this IServiceCollection services) => services.AddSingleton<IApprovalRequestServices, ApprovalRequestServices>();

    public static void ConfigureMosavabatContractService(this IServiceCollection services) => services.AddSingleton<IContractServices, ContractServices>();

    public static void ConfigureLcpContractService(this IServiceCollection services) => services.AddSingleton<External.Services.Contracts.Lcp.IContractServices, External.Services.Lcp.ContractServices>();

    public static void ConfigureContractService(this IServiceCollection services) => services.AddTransient<Services.Contracts.Mosavabat.IContractService, Services.Mosavabat.ContractService>();

    public static void ConfigureLcpAgentService(this IServiceCollection services) => services.AddSingleton<IAgentServices, AgentServices>();

    public static void ConfigureLcpRequestFacilityService(this IServiceCollection services) => services.AddSingleton<IRequestFacilityServices, RequestFacilityServices>();

    public static void ConfigureLcpServiceProviderService(this IServiceCollection services) => services.AddSingleton<IServiceProvider, ServiceProvider>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureTicketService(this IServiceCollection services) => services.AddSingleton<ITicketServices, TicketServices>();

    public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSamatServices(this IServiceCollection services) => services.AddScoped<ISamatServices, SamatServices>();

    public static void ConfigureSamatService(this IServiceCollection services) => services.AddScoped<ISamatService, SamatService>();

    public static void ConfigureMosavabatServicesWorkflowEngine(this IServiceCollection services) => services.AddScoped<IWorkflowEngineServices, WorkflowEngineServices>();

    public static void ConfigureMosavabatServicesEntityManager(this IServiceCollection services) => services.AddScoped<IEntityManagerServices, EntityManagerServices>();

    public static void ConfigureMosavabatRequestFacilityDatabaseServices(this IServiceCollection services) => services.AddScoped<IRequestFacilityDatabase, RequestFacilityDatabase>();

    public static void ConfigureMosavabatCollateralTypeDatabaseServices(this IServiceCollection services) => services.AddScoped<ICollateralTypeDatabase, CollateralTypeDatabase>();

    public static void ConfigureMosavabatApprovalRequestDatabaseServices(this IServiceCollection services) => services.AddScoped<IApprovalRequestDatabase, ApprovalRequestDatabase>();

    public static void ConfigureMosavabatBranchDatabaseServices(this IServiceCollection services) => services.AddScoped<IBranchDatabase, BranchDatabase>();

    public static void ConfigureMosavabatAccountBalanceAverageDatabaseServices(this IServiceCollection services) => services.AddScoped<IAccountBalanceAverageDatabase, AccountBalanceAverageDatabase>();

    public static void ConfigureMosavabatAccountBalanceAverageServices(this IServiceCollection services) => services.AddScoped<Services.Contracts.Mosavabat.IAccountBalanceAverageService, Services.Mosavabat.AccountBalanceAverageService>();

    public static void ConfigureMosavabatMainDatabaseServices(this IServiceCollection services) => services.AddScoped<IMainDatabase, MainDatabase>();

    public static void ConfigureMosavabatUserDatabaseServices(this IServiceCollection services) => services.AddScoped<IUserDatabase, UserDatabase>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlConnection"));

    public static void ConfigureFacilityGroupDatabaseServices(this IServiceCollection services) => services.AddScoped<IFacilityGroupDatabase, FacilityGroupDatabase>();

    public static void ConfigureSubContractTypeDatabaseServices(this IServiceCollection services) => services.AddScoped<ISubContractTypeDatabase, SubContractTypeDatabase>();

    public static void ConfigureExchangeCommercialDocDatabaseServices(this IServiceCollection services) => services.AddScoped<IExchangeCommercialDocDatabase, ExchangeCommercialDocDatabase>();

    public static void ConfigureLmsServices(this IServiceCollection services) => services.AddScoped<ILmsService, LmsService>();

    public static void ConfigureLmsProxyServices(this IServiceCollection services) => services.AddScoped<ILmsServiceProxy, LmsServiceProxy>();
    public static void ConfigureLmsSetting(this IServiceCollection services, IConfiguration configuration) => services.Configure<LmsServiceSetting>(configuration.GetSection("LmsSetting"));
    public static void ConfigureBsiSetting(this IServiceCollection services, IConfiguration configuration) => services.Configure<BsiServiceSetting>(configuration.GetSection("BsiSetting"));
    public static void ConfigureLcpServices(this IServiceCollection services) => services.AddScoped<ILcpService, LcpService>();
    public static void ConfigureLcpProxyServices(this IServiceCollection services) => services.AddScoped<ILcpServiceProxy, LcpServiceProxy>();
    public static void ConfigureLcpSetting(this IServiceCollection services, IConfiguration configuration) => services.Configure<LcpServiceSetting>(configuration.GetSection("LcpSetting"));

    public static void ConfigureContractDatabaseService(this IServiceCollection services) => services.AddScoped<IContractDatabaseService, ContractDatabaseService>();
    public static void ConfigureAgentDatabaseService(this IServiceCollection services) => services.AddScoped<IAgentDatabaseService, AgentDatabaseService>();

    public static void ConfigureBizagiDatabaseServices(this IServiceCollection services) => services.AddScoped<IBizagiDatabaseService, BizagiDatabaseService>();

    public static void ConfigureBizagiServices(this IServiceCollection services) => services.AddScoped<IBizagiService, BizagiService>();

    public static void ConfigureLoanExternalService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoanExternalService, LoanExternalService>();
        services.Configure<LoanExternalService>(configuration.GetSection("Abrisham"));
    }

    public static void ConfigureContractTransactionServices(this IServiceCollection services) =>
        services.AddScoped<IContractTransactionService, ContractTransactionService>();
    public static void ConfigureSupervisorDatabaseServices(this IServiceCollection services) => services.AddScoped<ISupervisorDatabaseService, SupervisorDatabaseService>();
    public static void ConfigureSupervisorServices(this IServiceCollection services) => services.AddScoped<ISupervisorService, SupervisorService>();
    public static void ConfiguresBeneficiaryService(this IServiceCollection services) => services.AddScoped<IBeneficiaryService, BeneficiaryService>();

    public static void ConfigureRelatedPersonsServices(this IServiceCollection services) => services.AddScoped<IRelatedPersonsDatabaseService, RelatedPersonsDatabaseService>();
    public static void ConfigureBoardMemberService(this IServiceCollection services) => services.AddScoped<IBoardMemberService, BoardMemberService>();
    public static void ConfigureBoardMemberDatabaseService(this IServiceCollection services) => services.AddScoped<IBoardMemberDataBaseService, BoardMemberDataBaseService>();
    public static void ConfigurationContractGroupDatabaseService(this IServiceCollection services) => services.AddScoped<IContractGroupDatabaseService, ContractGroupDatabaseService>();

}