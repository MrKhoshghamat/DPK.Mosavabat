using System;

namespace DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

public record RemoveRequestFromCartableDto(
    bool IsRemoved,
    DateTime ProcessDate,
    string Message);
