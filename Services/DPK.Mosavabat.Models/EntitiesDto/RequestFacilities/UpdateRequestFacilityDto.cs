using System;

namespace DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

public record UpdateRequestFacilityDto(
    long CaseId,
    string RadNumber,
    DateTime ProcessDate,
    string Message);