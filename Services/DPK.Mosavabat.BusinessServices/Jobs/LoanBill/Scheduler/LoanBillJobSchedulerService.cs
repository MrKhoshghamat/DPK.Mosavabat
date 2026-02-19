using System.Threading.Tasks;
using DPK.Foundation.Quartz;
using DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Executor;
using Quartz;

namespace DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Scheduler;

/// <summary>
/// سرویس زمانبندی اجرای جاب
/// </summary>
public class LoanBillJobSchedulerService(
    IQuartzScheduleJobManager jobManager)
    : ILoanBillJobSchedulerService
{

    /// <summary>
    /// زمانبندی جاب
    /// </summary>
    public async Task ScheduleJobAsync()
    {
        await jobManager.ScheduleAsync<ProcessLoanBillJob>(
            job =>
            {
                job.WithIdentity("ProcessLoanBillJob", "LoanGroup")
                    .WithDescription("Processes pending Loan Bill requests periodically.");
            },
            trigger =>
            {
                trigger.StartNow()
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(60)
                            .RepeatForever()
                            .Build());
            });
    }
}