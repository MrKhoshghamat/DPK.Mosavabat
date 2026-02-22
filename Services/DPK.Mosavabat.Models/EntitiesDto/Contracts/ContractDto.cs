using System;

namespace DPK.Mosavabat.Models.EntitiesDto.Contracts;

public record ContractDto(
    string C13,
    string C16,
    long ContractAmount,
    DateTime DueDate);