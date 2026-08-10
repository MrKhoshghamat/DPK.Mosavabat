using DPK.Mosavabat.Shared.Lcp.Enums;
using DPK.Mosavabat.Shared.Lcp.Models.General;

namespace DPK.Mosavabat.Shared.Lcp.Constants;

public class LcpConstants
{
    //public static string BaseAddress = "http://172.16.20.35:8109/";
    public static string BaseAddress = "https://localhost:5005/";

    public static LcpInfo GetInfo(LcpMethod method)
    {
        return method switch
        {
            LcpMethod.AcceptAgent => new LcpInfo {Verb = Verb.POST.ToString(), Url = "api/Service/Account/AcceptAgent"},
            LcpMethod.UpdateRequestFacility => new LcpInfo {Verb = Verb.PUT.ToString(), Url = "api/Service/RequestFacility/UpdateStatus"},
            LcpMethod.UpdateRadNumberRequestFacility => new LcpInfo {Verb = Verb.PUT.ToString(), Url = "api/Service/RequestFacility/UpdateRadNumber"},
            LcpMethod.PrintContractForMorabehe => new LcpInfo {Verb = Verb.POST.ToString(), Url = "api/Service/Mosavabat/Create"},
            _ => throw new Exception("Info not found")
        };
    }
}