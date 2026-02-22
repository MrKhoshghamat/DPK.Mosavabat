using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.Contracts;

public record UpdateContractFileLinkDto(
    [CanBeNull] bool IsUpdated);