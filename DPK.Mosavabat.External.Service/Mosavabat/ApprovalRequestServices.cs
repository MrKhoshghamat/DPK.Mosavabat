using DPK.Mosavabat.External.Services.Contracts.Mosavabat;

namespace DPK.Mosavabat.External.Services.Mosavabat;

// سرویس های اصلی مصوبات
public class ApprovalRequestServices : IApprovalRequestServices
{
    public string? InitApprovalRequestModel(int idLcpRequestFacility)
    {
        //// نام دامنه
        //const string domain = "srv-mosavabat";
        //// نام کاربر
        //var userName = UserDatabase.FindFacilityManagerOrSupervisor(idLcpRequestFacility);

        //if (userName == null || userName == "")
        //{
        //    return "مسئول تسهیلات شعبه یافت نشد";
        //}
        //// نام فرآیند
        //const string process = "ApprovalRequest2";
        //// 
        //var bizAgiWsParam = new BizAgiWSParam(domain: domain, userName: userName, cases: new Cases()
        //{
        //    Case = new Case(process: process, entities: new Entities()
        //    {
        //        approvalsRequest = new approvalsRequest(approvalRequestGroup: "1", requestTypeDetail: "1")
        //    })
        //});

        //return ConvertObjectToXml(bizAgiWsParam);
        return null;
    }

    // ساخت یک درخواست تسهیلات جدید
    public string? CreateApprovalsRequest(int idCase, int initialRadNumber, int idLcpRequestFacility)
    {
        //// ساخت مدل
        //var caseInfo = InitApprovalsRequestModel(idLcpRequestFacility);

        //if (caseInfo == "مسئول تسهیلات شعبه یافت نشد")
        //{
        //    return caseInfo;
        //}

        //// ارسال مدل به سرور مصوبات
        //CreateCasesAsString(caseInfo, out int idCaseApprovalRequest, out int radNumberApprovalRequest);

        //// فراخوانی سرویس Lcp برای به روز رسانی اطلاعات
        //RequestFacility.UpdateRadNumberRequestFacility(idCase, radNumberApprovalRequest, idCaseApprovalRequest);

        //// اگر شناسه ای برنگشته باشد
        //if (idCaseApprovalRequest == 0 || idCaseApprovalRequest <= 0) return "";

        //// به روز رسانی اطلاعات درخواست تسهیلات که از سمت سامانه تسهیلات حقوقی آمده است
        //RequestFacilityDatabase.UpdateLcpRequestFacility(idCase, initialRadNumber, idCaseApprovalRequest, radNumberApprovalRequest, idLcpRequestFacility);

        //// کپی اطلاعات درخواست تسهیلات اولیه به درخواست تسهیلات نهایی
        //var response = RequestFacilityDatabase.MapLcpRequestFacilityToApprovalRequest(idCase, idCaseApprovalRequest, idLcpRequestFacility);

        //return "";
        return null;
    }

    public string? GetRequestFacilityCaseId(int idCaseApprovalRequest)
    {
        //Database.RequestFacilityDatabase RequestFacilityDatabase2;
        //RequestFacilityDatabase2 = new Database.RequestFacilityDatabase();
        //var caseId = RequestFacilityDatabase2.GetRequestFacilityCaseId(idCaseApprovalRequest);

        //return caseId;
        return null;
    }
}