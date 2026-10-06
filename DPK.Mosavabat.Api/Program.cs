using DPK.Mosavabat.Api.Extensions;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Presentation;
using DPK.Mosavabat.Presentation.ActionFilters;
using DPK.Mosavabat.Repository;
using DPK.Mosavabat.Services.Contracts.Submission;
using DPK.Mosavabat.Services.Jobs;
using DPK.Mosavabat.Services.Submission;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using CustomMapper = DPK.Mosavabat.Api.CustomMapper;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services
    .AddScoped<ILcpRequestFacilitySubmissionRepository,
    LcpRequestFacilitySubmissionRepository>();

builder.Services
    .AddScoped<ILcpRequestFacilitySubmissionService,
    LcpRequestFacilitySubmissionService>();

builder.Services.AddHostedService<SubmissionReconciliationHostedService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureMemoryCache();
builder.Services.ConfigureQuartz();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(CustomMapper).Assembly);
builder.Services.ConfigureVersioning();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSamatServices();
builder.Services.ConfigureSamatService();
builder.Services.ConfigureBizagiDatabaseServices();
builder.Services.ConfigureBizagiServices();

builder.Services.ConfigureSupervisorDatabaseServices();
builder.Services.ConfigureSupervisorServices();

builder.Services.ConfigServices();
builder.Services.ConfigureContractTransactionServices();

// اضافه کردن سرویس های مصوبات
builder.Services.ConfigureMosavabatServicesWorkflowEngine();
builder.Services.ConfigureMosavabatServicesEntityManager();

// سرویس های دیتابیس
builder.Services.ConfigureMosavabatMainDatabaseServices();
builder.Services.ConfigureContractDatabaseService();
builder.Services.ConfigureAgentDatabaseService();
builder.Services.ConfigureCreditProcessesDatabase();
builder.Services.ConfigureCreditProcessesService();
builder.Services.ConfigureMosavabatRequestFacilityDatabaseServices();
builder.Services.ConfigureMosavabatBranchDatabaseServices();
builder.Services.ConfigureMosavabatCollateralTypeDatabaseServices();
builder.Services.ConfigureMosavabatUserDatabaseServices();
builder.Services.ConfigureMosavabatApprovalRequestDatabaseServices();
builder.Services.ConfigureMosavabatAccountBalanceAverageDatabaseServices();
builder.Services.ConfigureBoardMemberDatabaseService();
builder.Services.ConfigurationContractGroupDatabaseService();


builder.Services.ConfigureFacilityGroupDatabaseServices();
builder.Services.ConfigureSubContractTypeDatabaseServices();
builder.Services.ConfigureExchangeCommercialDocDatabaseServices();
builder.Services.ConfigureRelatedPersonsServices();

// سرویس های مصوبات
builder.Services.ConfigureMosavabatApprovalRequestService();
builder.Services.ConfigureMosavabatContractService();
builder.Services.ConfiguresBeneficiaryService();

builder.Services.Configure<TransactionSettings>(builder.Configuration.GetSection("TransactionSettings"));

// سرویس های سامانه تسهیلات حقوقی مشتریان
builder.Services.ConfigureLcpServiceProviderService();
builder.Services.ConfigureLcpContractService();
builder.Services.ConfigureTicketService();
builder.Services.ConfigureLcpAgentService();
builder.Services.ConfigureLcpRequestFacilityService();
builder.Services.ConfigureMosavabatAccountBalanceAverageServices();
builder.Services.ConfigureContractService();
builder.Services.ConfigureBoardMemberService();
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddScoped<ValidationFilterAttribute>();

//LMS
builder.Services.ConfigureLmsServices();
builder.Services.ConfigureLmsProxyServices();
builder.Services.ConfigureLmsSetting(builder.Configuration);

// LCP
builder.Services.ConfigureLcpServices();
builder.Services.ConfigureLcpProxyServices();
builder.Services.ConfigureLcpSetting(builder.Configuration);

// Abrisham | ابریشم
builder.Services.ConfigureLoanExternalService(builder.Configuration);

builder.Services.ConfigFluentValidation();

// Content negotiation | XML
builder.Services.AddControllers(
        config =>
        {
            config.RespectBrowserAcceptHeader = true;
            //config.ReturnHttpNotAcceptable = true;
            config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            config.Filters.Add<SamatServiceExceptionFilter>();
        })
    .AddXmlDataContractSerializerFormatters()
    //.AddCustomCSVFormatter()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);


NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
{
    return new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services
        .BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value
        .InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
}



var app = builder.Build();


var logger = app.Services.GetRequiredService<ILoggerManager>();

app.ConfigureExceptionHandler(logger);
if (app.Environment.IsProduction()) app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(s =>
//    {
//        s.SwaggerEndpoint("in-mosavabat-v1/swagger.json", "سرویس های مربوط به مصوبات");
//        s.SwaggerEndpoint("for-credit-v1/swagger.json", "سرویس های  مربوط به کنترل پرداخت");
//        s.SwaggerEndpoint("for-lcp-v1/swagger.json", "سرویس های مربوط به سامانه پرتال حقوقی مشتریان ورژن 1");
//        s.SwaggerEndpoint("for-lcp-v2/swagger.json", "سرویس های مربوط به سامانه پرتال حقوقی مشتریان ورژن 2");
//        s.SwaggerEndpoint("in-lcp-v1/swagger.json", "سرویس های ارائه شده توسط پرتال حقوقی مشتریان");
//        s.SwaggerEndpoint("for-abrisham-v1/swagger.json", "سرویس های مربوط به ابریشم");

//    });
//}

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("in-mosavabat-v1/swagger.json", "سرویس های مربوط به مصوبات");
    s.SwaggerEndpoint("for-credit-v1/swagger.json", "سرویس های  مربوط به کنترل پرداخت");
    s.SwaggerEndpoint("for-lcp-v1/swagger.json", "سرویس های مربوط به سامانه پرتال حقوقی مشتریان ورژن 1");
    s.SwaggerEndpoint("for-lcp-v2/swagger.json", "سرویس های مربوط به سامانه پرتال حقوقی مشتریان ورژن 2");
    s.SwaggerEndpoint("in-lcp-v1/swagger.json", "سرویس های ارائه شده توسط پرتال حقوقی مشتریان");
    s.SwaggerEndpoint("for-abrisham-v1/swagger.json", "سرویس های مربوط به ابریشم");

});


app.MapControllers();

app.Run();