using DPK.Foundation;
using DPK.Mosavabat.Common.Enums.CreditProcesses;

namespace DPK.Mosavabat.Common.Exceptions;

public class BusinessRuleException : DpkException
{
    public int Code { get; }
    public string ErrorMessage { get; }

    public BusinessRuleException(CreditProcessErrors code, string message) : base(message)
    {
        Code = (int)code;
        ErrorMessage = message;
    }

    public BusinessRuleException(int code, string message) : base(message)
    {
        Code = code;
        ErrorMessage = message;
    }
}