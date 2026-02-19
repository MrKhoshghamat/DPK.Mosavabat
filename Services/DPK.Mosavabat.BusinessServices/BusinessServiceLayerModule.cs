using DPK.Foundation.Threading;
using DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Scheduler;
using DPK.Mosavabat.DomainEntities;

namespace DPK.Mosavabat.BusinessServices;

/// <summary>
/// Business service layer module
/// </summary>
[DependsOn(
    typeof(DpkDomainLayerModule),
    typeof(ModelLayerModule),
    typeof(ValidationServiceLayerModule),
    typeof(GeneralServiceLayerModule),
    typeof(SqlBusinessServiceLayerModule),
    typeof(DpkAgGridModule),
    typeof(ExternalServiceLayerModule)
)]
public class BusinessServiceLayerModule : DpkModule
{
    /// <summary>
    /// Pre initialize
    /// </summary>
    public override void PreInitialize()
    {

        Configuration.UnitOfWork.IsolationLevel = IsolationLevel.ReadCommitted;
        Configuration.UnitOfWork.Timeout = TimeSpan.FromMilliseconds(1);
        //IocManager.IocContainer.Register(Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(MosavabatRepositoryBase<,>)));
    }

    /// <summary>
    /// Initialize
    /// </summary>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BusinessServiceLayerModule).GetAssembly());
    }

    /// <summary>
    /// Post Initialize
    /// </summary>
    public override void PostInitialize()
    {
        var scheduler = IocManager.Resolve<ILoanBillJobSchedulerService>();

        AsyncHelper.RunSync(() => scheduler.ScheduleJobAsync());
    }
}