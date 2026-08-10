namespace DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;

public class CaseForAbortCaseDto
{
    public int IdCase { get; set; }
    public string AbortReason { get; set; }
    public string RadNumber { get; set; }
    public string UserName { get; set; }
    public int IdUser { get; set; }
}