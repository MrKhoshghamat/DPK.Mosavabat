using DPK.Mosavabat.Entities.Models.RequestFacilities;

namespace DPK.Mosavabat.Services.Contracts.Submission;

public enum ReservationOutcome
{
    Acquired = 1,
    Existing = 2,
    PayloadMismatch = 3
}

public sealed record SubmissionReservation(
    ReservationOutcome Outcome,
    LcpRequestFacilitySubmission Submission)
{
    public bool IsOwner =>
        Outcome == ReservationOutcome.Acquired;
}