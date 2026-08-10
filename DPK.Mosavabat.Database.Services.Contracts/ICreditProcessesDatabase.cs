using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.ResponseModels.CreditProcesses;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface ICreditProcessesDatabase : IMainDatabase
{
    public Task<string?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel);

    public Task<string?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel);

    public Task<List<GetListTermsAndConditionsApprovalRequestResponseModel>?> GetListTermsAndConditionsApprovalRequest(GetListTermsAndConditionsApprovalRequestDataModel dataModel);

    public Task<string?> InsertPaymentPermission(InsertPaymentPermissionDataModel dataDataModel);

    //public Task<string?> CreatePaymentPermissionProcess(CreateCompleteContractInfoProcessDataModel dataModel);

    public Task<string?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceDataModel dataModel);

    //public Task<string?> CreateInspectionProcess(CreateViolationInvestigationProcessDataModel dataModel);

    public Task<string?> CalculationCustomerDebtBalance(CalculationCustomerDebtBalanceDataModel dataModel);

    public Task<string?> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel);
    public Task<List<GetListOfPaymentPermissionResponseModel>?> GetListOfPaymentPermission(GetPaymentPermissionListDataModel dataModel);
}