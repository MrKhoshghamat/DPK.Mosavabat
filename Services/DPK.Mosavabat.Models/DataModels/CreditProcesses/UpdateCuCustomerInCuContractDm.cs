namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class UpdateCuCustomerInCuContractDm(
    int idCuCustomer,
    string c13,
    string c16)
{
    public int IdCuCustomer { get; } = idCuCustomer;
    public string C13 { get; } = c13;
    public string C16 { get; } = c16;
}