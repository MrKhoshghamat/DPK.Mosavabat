namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CustomerCurrentFacilityDto(
    long CustomerNo,
    long FacilityNo,
    long FacilityStatusCode,
    long CurrentInstallmentBalance,
    long NonCurrentInstallmentBalance,
    long TotalDebtBalance);
