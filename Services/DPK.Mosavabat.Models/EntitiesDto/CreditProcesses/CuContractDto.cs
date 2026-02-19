using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CuContractDto(
    string C13,
    long ContractAmount,
    long PrepaidAmount,
    string MosavabeNo,
    [CanBeNull] string BranchCode,
    [CanBeNull] string CustomerNo,
    [CanBeNull] string ContractType,
    bool IsDeleted);
