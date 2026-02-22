using System;

namespace DPK.Mosavabat.Models.EntitiesDto.Agents;

public record CreateAgentDto(
    long CaseId,
    string RadNumber,
    DateTime ProcessDate,
    string Message);