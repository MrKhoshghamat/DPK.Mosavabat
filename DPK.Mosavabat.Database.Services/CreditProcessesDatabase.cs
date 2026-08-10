using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.ResponseModels.CreditProcesses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;

namespace DPK.Mosavabat.Database.Services
{
    public class CreditProcessesDatabase : MainDatabase, ICreditProcessesDatabase
    {
        private readonly ILoggerManager _logger;
        public CreditProcessesDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
        {
            _logger = logger;
        }

        // سرویس اول
        /// <summary>
        /// کنترل امکان صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel)
        {

            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.VarChar) {Value = dataModel.MosavabeNumber},             // شماره مصوبه
                new("@ContractAmount", SqlDbType.Float) {Value = dataModel.ContractAmount},               // مبلغ قرارداد
                new("@PrepayAmount", SqlDbType.Float) {Value = dataModel.CustomerSharePrice},       // مبلغ پیش دریافت
                new("@SubContractType", SqlDbType.Int) {Value = dataModel.SubContractType}  // زیر نوع عقد
            };
            // نتیجه
            var result = await ExecuteScalarAsync(DevProviderConnectionString, "[CP].[PaymentPermissionIssuanceControl_v2]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس دوم
        /// <summary>
        /// به روز رسانی شماره قرارداد c13
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) {Value = dataModel.CustomerNo},
                new("@BranchCode", SqlDbType.Int) {Value = dataModel.BranchCode},
                new("@ContractType", SqlDbType.Int) {Value = dataModel.ContractType},
                new("@SubContractType", SqlDbType.Int) {Value = dataModel.SubContractType},
                new("@ContractPrepaidAmountsSum", SqlDbType.BigInt) {Value = dataModel.ContractPrepaidAmountsSum},
                new("@C13", SqlDbType.BigInt) {Value = dataModel.C13}
            };
            // نتیجه
            var result = await ExecuteScalarAsync(MosavabatApiConnectionString, "[CP].[CheckAndUpdateC13InPaymentPermission]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس سوم
        /// <summary>
        /// دریافت لیست مفاد و شروط مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<List<GetListTermsAndConditionsApprovalRequestResponseModel>?> GetListTermsAndConditionsApprovalRequest(GetListTermsAndConditionsApprovalRequestDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.VarChar) {Value = dataModel.MosavabeNumber}
            };
            // نتیجه

            var result = await ExecuteAsListAsync<GetListTermsAndConditionsApprovalRequestResponseModel>(DevProviderConnectionString, "[CP].[GetListTermsAndConditionsApprovalRequest]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result;
        }

        // سرویس چهارم
        /// <summary>
        /// ورود اطلاعات صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel)
        {
            var today = DateTime.Now;
            var code = $"{today.Year}{today.Month}{today.Day}{today.Hour}{today.Minute}{today.Second}{today.Millisecond}{dataModel.ContractType}{dataModel.BranchCode}";
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@Code", SqlDbType.NVarChar) {Value = code},
                new("@MosavabeNo", SqlDbType.NVarChar) {Value = dataModel.MosavabeNo},
                new("@BranchCode", SqlDbType.NVarChar) {Value = dataModel.BranchCode},
                new("@CustomerNo", SqlDbType.NVarChar) {Value = dataModel.CustomerNo},
                new("@ContractType", SqlDbType.NVarChar) {Value = dataModel.ContractType},
                new("@SubContractType", SqlDbType.NVarChar) {Value = dataModel.SubContractType},
                new("@ContractAmount", SqlDbType.Decimal) {Value = dataModel.ContractAmount},
                new("@PrepaidAmount", SqlDbType.Decimal) {Value = dataModel.PrepaidAmount}
            };
            // نتیجه
            var result = await ExecuteScalarAsync(MosavabatApiConnectionString, "[CP].[InsertPaymentPermission]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس ششم
        /// <summary>
        /// محاسبه مانده آزاد مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.VarChar) {Value = dataModel.MosavabeNumber}
            };
            // نتیجه
            var result = await ExecuteScalarAsync(DevProviderConnectionString, "[CP].[CalculateApprovalRequestUnusedBalance]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس هشتم
        /// <summary>
        /// محاسبه مانده بدهی مشتری
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CalculationCustomerDebtBalance(CalculationCustomerDebtBalanceDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.NVarChar) {Value = dataModel.MosavabeNumber}
            };
            // نتیجه
            var result = await ExecuteScalarAsync(DevProviderConnectionString, "[CP].[CalculationCustomerDebtBalance]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس نهم
        /// <summary>
        /// آپدیت وضعیت پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@Id", SqlDbType.NVarChar) {Value = dataModel.Id},
                new("@IsCancel", SqlDbType.Bit) {Value = dataModel.IsCancel},
                new("@IsDelete", SqlDbType.Bit) {Value = dataModel.IsDelete},
            };
            // نتیجه
            var result = await ExecuteScalarAsync(MosavabatApiConnectionString, "[CP].[UpdatePaymentPermission]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result?.ToString();
        }

        // سرویس دهم
        /// <summary>
        /// دریافت لیست پرداخت ها
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<List<GetListOfPaymentPermissionResponseModel>?> GetListOfPaymentPermission(GetPaymentPermissionListDataModel dataModel)
        {
            // پارامترها
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.NVarChar) {Value = dataModel.MosavabeNumber}
            };
            // نتیجه
            var result = await ExecuteAsListAsync<GetListOfPaymentPermissionResponseModel>(MosavabatApiConnectionString, "[CP].[GetListOfPaymentPermission]", parameters);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result;
        }
    }
}
