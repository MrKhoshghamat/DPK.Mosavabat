namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Models;

public sealed record SubjectFacilityLookupResult(
    long SubjectId,
    long FacilityId,
    long FacilityGroupId);