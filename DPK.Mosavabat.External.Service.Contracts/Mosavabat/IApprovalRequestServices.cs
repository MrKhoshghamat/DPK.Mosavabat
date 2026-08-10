namespace DPK.Mosavabat.External.Services.Contracts.Mosavabat;

public interface IApprovalRequestServices
{
    string? InitApprovalRequestModel(int idLcpRequestFacility);
    string? CreateApprovalsRequest(int idCase, int initialRadNumber, int idLcpRequestFacility);
    string? GetRequestFacilityCaseId(int idCaseApprovalRequest);
}