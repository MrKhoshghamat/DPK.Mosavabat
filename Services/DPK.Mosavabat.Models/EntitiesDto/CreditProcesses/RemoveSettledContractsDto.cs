using System.Collections.Generic;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record RemoveSettledContractsDto(
    List<CustomerCurrentFacilityDto> Contracts);
