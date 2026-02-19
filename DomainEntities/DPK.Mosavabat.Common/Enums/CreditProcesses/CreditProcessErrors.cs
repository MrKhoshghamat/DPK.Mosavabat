namespace DPK.Mosavabat.Common.Enums.CreditProcesses;

public enum CreditProcessErrors
{
    Unauthorized = 401,
    OperationFailed = 1000,

    InvalidParameter = 1100,
    ParameterRequired = 1101,
    SystemError = 1102,
    MosavabeNotFound = 1103,
    ContractAmountExceedsMosavabe = 1104,
    PrepayAmountLessThanMosavabe = 1105,
    MosavabeExpired = 1106,
    GroupMosavabeLimitReached = 1107,
    AverageNotMaintained = 1108,
    CustomerHasBouncedCheck = 1109,
    PendingPermissionExists = 1110,
    DuplicatePermissionNotAllowed = 1111,
    PendingMosavabePermissionExists = 1112,
    MosavabeMatured = 1113,
    MosavabeNoFreeBalance = 1114,
    MultiplePaymentPermissionsFound = 1115,
    PaymentPermissionNotFound = 1116,
    DuplicatePaymentPermissionForFacility = 1117,
    PendingPaymentPermissionForCustomer = 1122,

    DeleteForbiddenHasContract = 2001,
    CancelForbiddenNoContract = 2002,
    NotFoundPaymentPermission = 2003,
    DeleteAndCancelConflict = 2004,

    DebtCalculationServiceUnavailable = 1130
}