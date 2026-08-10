using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects;
using DPK.Mosavabat.Shared.ResponseModels.CreditProcesses;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts
{
    public interface ICreditProcessesService
    {
        // سرویس اول
        /// <summary>
        /// کنترل امکان صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel);
        // سرویس دوم
        /// <summary>
        /// به روز رسانی شماره قرارداد c13
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel);
        // سرویس سوم
        /// <summary>
        /// دریافت لیست مفاد و شروط مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<List<GetListTermsAndConditionsApprovalRequestResponseModel>?> GetListTermsAndConditionsApprovalRequest(GetListTermsAndConditionsApprovalRequestDataModel dataModel);
        // سرویس چهارم
        /// <summary>
        /// ورود اطلاعات صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel);
        // سرویس پنجم
        /// <summary>
        /// ایجاد یک نمونه از فرآیند تکمیل اطلاعات قرارداد
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        public Task<MosavabatResponseDataModel?> CreateCompleteContractInfoProcess(CreateCompleteContractInfoProcessDataModel dataModel, string branchCode);
        // سرویس ششم
        /// <summary>
        /// محاسبه مانده آزاد مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceDataModel dataModel);
        // سرویس هفتم
        /// <summary>
        /// ایجاد یک نمونه از فرآیند بررسی تخلف
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<MosavabatResponseDataModel?> CreateViolationInvestigationProcess(CreateViolationInvestigationProcessDataModel dataModel);
        // سرویس هشتم
        /// <summary>
        /// محاسبه مانده بدهی مشتری
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> CalculationCustomerDebtBalance(CalculationCustomerDebtBalanceDataModel dataModel);
        // سرویس نهم
        /// <summary>
        /// آپدیت وضعیت پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<string?> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel);
        // سرویس دهم
        /// <summary>
        /// دریافت لیست پرداخت ها
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Task<List<GetListOfPaymentPermissionResponseModel>?> GetListOfPaymentPermission(GetPaymentPermissionListDataModel dataModel);
    }
}
