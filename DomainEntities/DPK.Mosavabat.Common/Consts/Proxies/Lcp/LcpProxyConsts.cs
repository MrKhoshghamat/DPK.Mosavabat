namespace DPK.Mosavabat.Common.Consts.Proxies.Lcp;

public class LcpProxyConsts
{
    public class Urls
    {
        public class AgentService
        {
            public const string AcceptAgent =
                "/Service/Account/AcceptAgent";
        }

        public class BoardMemberService
        {
            public const string GetBoardMemberForMosavabat =
                "/Service/BoardMemberInfo/GetBoardMemberForMosavabat";
        }

        public class ContractService
        {
            public const string Create =
                "/Service/Mosavabat/Create";
        }

        public class RequestFacilityService
        {
            public const string UpdateStatus =
                "/Service/RequestFacility/UpdateStatus";

            public const string UpdateRadNumber =
                "/Service/RequestFacility/UpdateRadNumber";
        }

        public class TicketService
        {
            public const string Create =
                "/Service/Ticket/Create";
        }

        public class AccountService
        {
            public const string Login =
                "/Service/Account/Login";
        }
    }
}