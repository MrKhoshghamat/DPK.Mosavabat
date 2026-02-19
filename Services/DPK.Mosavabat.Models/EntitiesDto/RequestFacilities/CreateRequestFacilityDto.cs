using System;

namespace DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

public record CreateRequestFacilityDto(
    long CaseId,
    string RadNumber,
    DateTime ProcessDate,
    string Message);