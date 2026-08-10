using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects;
using DPK.Mosavabat.Shared.ResponseModels.CreditProcesses;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using Newtonsoft.Json;
using System.Xml;

namespace DPK.Mosavabat.Services
{
    public class CreditProcessesService : ICreditProcessesService
    {
        private readonly ICreditProcessesDatabase _creditProcessesDatabase;
        private readonly IWorkflowEngineServices _workflowEngine;
        private readonly IUserDatabase _userDatabase;
        private readonly ILoggerManager _logger;

        public CreditProcessesService(ILoggerManager logger, ICreditProcessesDatabase creditProcessesDatabase, IWorkflowEngineServices workflowEngine, IUserDatabase userDatabase)
        {
            _logger = logger;
            _creditProcessesDatabase = creditProcessesDatabase;
            _userDatabase = userDatabase;
            _workflowEngine = workflowEngine;
        }

        // سرویس اول
        /// <summary>
        /// کنترل امکان صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.PaymentPermissionIssuanceControl(dataModel);
        }

        // سرویس دوم
        /// <summary>
        /// به روز رسانی شماره قرارداد c13
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            // فراخوانی روال مربوط به بروز رسانی شماره قرار داد سی سیزده
            // فراخوانی سرویس ایجاد فرآیند تکمیل اطلاعات قرارداد
            return await _creditProcessesDatabase.CheckAndUpdateC13InPaymentPermission(dataModel);
        }

        // سرویس سوم
        /// <summary>
        /// دریافت لیست مفاد و شروط مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<List<GetListTermsAndConditionsApprovalRequestResponseModel>?> GetListTermsAndConditionsApprovalRequest(GetListTermsAndConditionsApprovalRequestDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.GetListTermsAndConditionsApprovalRequest(dataModel);
        }

        // سرویس چهارم
        /// <summary>
        /// ورود اطلاعات صدور مجوز پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            // فراخوانی سرویس کنترل امکان صدور مجوز پرداخت
            // فراخوانی سرویس بیرونی استعلام چک
            // فراخوانی روال دیتابیس مربوط به سرویس ورود اطلاعات صدور مجوز پرداخت
            return await _creditProcessesDatabase.InsertPaymentPermission(dataModel);
        }

        // سرویس پنجم
        /// <summary>
        /// ایجاد یک نمونه از فرآیند تکمیل اطلاعات قرارداد
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        public async Task<MosavabatResponseDataModel?> CreateCompleteContractInfoProcess(CreateCompleteContractInfoProcessDataModel dataModel,string branchCode)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            var userName = await _userDatabase.GetUserName(Convert.ToInt32(branchCode), MosavabatClassName.CompleteContractInfo);

            if (string.IsNullOrEmpty(userName))
                return new MosavabatResponseDataModel(default, default);

            var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(dataModel, userName, "CompleteContractInfo");

            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "CU_Contract");

            var xDoc = new XmlDocument();

            xDoc.LoadXml(data);

            var response = await _workflowEngine.CreateCasesAsync(xDoc);

            return response;
        }

        // سرویس ششم
        /// <summary>
        /// محاسبه مانده آزاد مصوبه
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.CalculateApprovalRequestUnusedBalance(dataModel);
        }

        // سرویس هفتم
        /// <summary>
        /// ایجاد یک نمونه از فرآیند بررسی تخلف
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<MosavabatResponseDataModel?> CreateViolationInvestigationProcess(CreateViolationInvestigationProcessDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            var userName = await _userDatabase.GetUserName(Convert.ToInt32(dataModel.Branches), MosavabatClassName.ViolationInvestigation);

            if (string.IsNullOrEmpty(userName))
                return new MosavabatResponseDataModel(default, default);

            var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(dataModel, userName, "ViolationInvestigation");

            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "Infraction");

            var xDoc = new XmlDocument();

            xDoc.LoadXml(data);

            var response = await _workflowEngine.CreateCasesAsync(xDoc);

            return response;
        }

        // سرویس هشتم
        /// <summary>
        /// محاسبه مانده بدهی مشتری
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> CalculationCustomerDebtBalance(CalculationCustomerDebtBalanceDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.CalculationCustomerDebtBalance(dataModel);
        }

        // سرویس نهم
        /// <summary>
        /// آپدیت وضعیت پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<string?> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.UpdatePaymentPermission(dataModel);
        }

        // سرویس دهم
        /// <summary>
        /// دریافت لیست پرداخت ها
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public async Task<List<GetListOfPaymentPermissionResponseModel>?> GetListOfPaymentPermission(GetPaymentPermissionListDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return await _creditProcessesDatabase.GetListOfPaymentPermission(dataModel);
        }
    }
}
