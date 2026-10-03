using System.Security.Cryptography;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using Newtonsoft.Json;
using System.Text;

namespace DPK.Mosavabat.Services.Helper;

public static class RequestFacilityHashCalculator
{
    public static string Calculate(
        RequestFacilityUpdateDataModel request)
    {
        var normalized = new
        {
            request.LcpId,
            request.RequestNo,
            request.CustomerNo,
            request.AccountNumber,
            request.BranchCode,
            request.LegalName,
            request.RequestDate,
            request.SubjectFacilities,
            request.Collaterals,
            request.Guarantors
        };

        var json = JsonConvert.SerializeObject(
            normalized,
            Formatting.None);

        var bytes = Encoding.UTF8.GetBytes(json);

        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}
