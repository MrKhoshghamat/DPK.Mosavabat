using System;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record PaymentPermissionsDto(
    int Id,
    string Code,
    string MosavabeNo,
    string BranchCode,
    string CustomerNo,
    string CustomerName,
    string ContractType,
    string SubContractType,
    string C13,
    string C16,
    DateTime ExpireDate,
    bool IsCanceled,
    bool IsDeleted,
    long ContractAmount,
    long PrepaidAmount,
    DateTime CreationTime);