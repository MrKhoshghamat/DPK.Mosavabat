using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.Payam;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس استعلام چک برگشتی مشتری
/// </summary>
/// <param name="nationalId"> کد ملی / شناسه ملی </param>
public class GetCheckInquiryQuery(
    string nationalId)
    : QueryBase<CheckInquiryDto>
{
    /// <summary>
    /// کد ملی / شناسه ملی
    /// </summary>
    public string NationalId { get; } = nationalId;
}