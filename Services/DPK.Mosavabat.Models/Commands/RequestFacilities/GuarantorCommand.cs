namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class GuarantorCommand(
    string country,
    string description,
    string firstName,
    string guarantorType,
    string lastName,
    string legalGuarantorType,
    string nationalCode,
    string nationality,
    long lcpId,
    bool isDeleted)
{
    public string Country { get; } = country;
    public string Description { get; } = description;
    public string FirstName { get; } = firstName;
    public string GuarantorType { get; } = guarantorType;
    public string LastName { get; } = lastName;
    public string LegalGuarantorType { get; } = legalGuarantorType;
    public string NationalCode { get; } = nationalCode;
    public string Nationality { get; } = nationality;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
}