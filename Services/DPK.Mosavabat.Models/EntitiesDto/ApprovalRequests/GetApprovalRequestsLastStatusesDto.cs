using System.Collections.Generic;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

public record GetApprovalRequestsLastStatusesDto(
    [ItemCanBeNull] Dictionary<int, string> Statuses);