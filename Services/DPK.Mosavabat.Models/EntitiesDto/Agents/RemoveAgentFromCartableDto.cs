using System;

namespace DPK.Mosavabat.Models.EntitiesDto.Agents;

public record RemoveAgentFromCartableDto(
    bool IsRemoved,
    DateTime ProcessDate,
    string Message);
