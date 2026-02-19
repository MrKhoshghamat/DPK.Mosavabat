using System;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CreateCompleteContractInfoProcessDto(
    long CaseId,
    string RadNumber,
    DateTime ProcessDate,
    string Message);
