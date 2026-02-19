using DPK.Mosavabat.Models.Commands.RequestFacilities;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Models;

public sealed record CollateralLookupResult(
    CollateralCommand Source,
    long CollateralTypeId);