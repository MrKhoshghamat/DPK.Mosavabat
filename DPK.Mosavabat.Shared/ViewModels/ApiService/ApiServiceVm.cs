namespace DPK.Mosavabat.Shared.ViewModels.ApiService;

public class ApiServiceVm<T>
{
    public string ApiUrl { get; set; }
    public HttpMethod HttpMethod { get; set; }
    public T RequestData { get; set; }
}