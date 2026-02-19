using System.Threading.Tasks;

namespace DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Scheduler;

/// <summary>
/// سرویس زمانبندی جاب
/// </summary>
public interface ILoanBillJobSchedulerService
    : IApplicationService
{
    /// <summary>
    /// زمانبندی جاب
    /// </summary>
    /// <returns></returns>
    Task ScheduleJobAsync();
}