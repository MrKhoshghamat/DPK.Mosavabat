namespace DPK.Mosavabat.Entities.Exceptions;

public class LoanBillDataNotReadyException : BadRequestException
{
    public LoanBillDataNotReadyException()
        : base("اطلاعات تسهیلات موردنظر در مرکز فرآوری داده بروز نمیباشد؛ لطفاً چند دقیقه دیگر مجدداً تلاش کنید.")
    {
    }
}