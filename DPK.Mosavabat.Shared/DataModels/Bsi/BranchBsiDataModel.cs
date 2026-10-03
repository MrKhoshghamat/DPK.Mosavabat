namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class BranchBsiDataModel
{
    public int BranchCode { get; set; }
    public string ChangeDateStatus { get; set; } = string.Empty;
    public DateTime? ChangeDate { get; set; }
    public int AreaCode { get; set; }
    public int? DomainCode { get; set; }
    public string DomainName { get; set; } = string.Empty;
    public CodeDescription ChangeStatus { get; set; } = new();
    public int BranchIntegrationCode { get; set; }
    public string Degree { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string TelephoneCode { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string TelephoneInformation { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty; // Consider using DateTime if possible
    public string CityCode { get; set; } = string.Empty;
    public CodeDescription StatusCode { get; set; } = new();
    public CodeDescription VirtualCode { get; set; } = new();
    public CodeDescription CurrencyCode { get; set; } = new();
    public CodeDescription VigilanceCode { get; set; } = new();
    public CodeDescription InternalCode { get; set; } = new();
    public CodeDescription UrbanCode { get; set; } = new();
    public CodeDescription CounterCode { get; set; } = new();
    public CodeDescription PointTypeCode { get; set; } = new();
    public CodeDescription PlaceTypeCode { get; set; } = new();
}

public class CodeDescription
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
}