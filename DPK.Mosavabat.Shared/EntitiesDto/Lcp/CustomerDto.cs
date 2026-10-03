using DPK.Mosavabat.Common.Extensions;

namespace DPK.Mosavabat.Shared.EntitiesDto.Lcp;

public class CustomerDto
{
    public decimal CustomerNumber { get; set; }// شماره مشتری
    public short CustomerType { get; set; }// نوع مشتری
    public decimal? NationalIdNumber { get; set; }// کد ملی حقیقی ایرانی
    public string ForeignerIdNumber { get; set; }// شناسه اتباع بیگانه-کد فراگیر(حقیقی)
    public decimal? CorporateCustomerIdNumber { get; set; }// شناسه حقوقی
    public string IdentificationNumber { get; set; }// شماره مدرک شناسایی
    public string Name { get; set; }// نام
    public string LastName { get; set; }// نام خانوادگی
    public string FatherName { get; set; }// نام پدر
    public string IdentityCertificateSequence { get; set; }// سری شناسنامه
    public string IdentityCertificateSerial { get; set; }// سریال شناسنامه
    public string HomePhoneNumber { get; set; }// شماره تلفن منزل
    public string BusinessContactNumber { get; set; }// شماره تلفن محل کار
    public string FaxNumber { get; set; }// شماره فکس
    public string MobileNumber { get; set; }// شماره موبایل
    public string Address1 { get; set; }// آدرس 1
    public string Address2 { get; set; }// آدرس 2
    public string Address3 { get; set; }// آدرس 3
    public string Address4 { get; set; }// آدرس 4
    public string PostalCode { get; set; }// کد پستی
    public decimal ShahabCode { get; set; }// کد شهاب
    public string IssueOrRegistryPlace { get; set; }
    public string BirthDate { get; set; }
    public string CustomerDefinedDate { get; set; }//تاریخ ثبت حساب در بانک
    public string RegistrationDate { get; set; }
    public string RegistrationNumber { get; set; }
    public CustomerFlag DeletedCustomerFlag { get; set; }
    public IEnumerable<SelectedItems<int>> Branches { get; private set; }
    public string Token { get; set; }
    public decimal AccountNumber { get; set; }
    public int BranchCode { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public CommercialFlag CommercialFlag { get; set; }

    public void SetBranch(IEnumerable<SelectedItems<int>> branches)
    {
        Branches = branches;
        Token = StringExtension.RandomString();
    }
}

public class AccountStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
}

public class CurrencyType
{
    public int Id { get; set; }
    public string Description { get; set; }
}

public class CommercialFlag
{
    public string Id { get; set; }
    public string Description { get; set; }
}

public class CustomerFlag
{
    public int Id { get; set; }
    public string? Description { get; set; }
}

public class SelectedItems<T>
{
    /// <summary>
    /// عنوان یا نام
    /// </summary>
    public string? Title { get; set; }

}

public class SelectedConfig<T> : SelectedItems<T>
{
    public string? Config { get; set; }

    public int? FacilityGroupId { get; set; }
}

public class SelectedTree<T> : SelectedItems<T>
{
    public List<SelectedItems<T>> Child { get; set; }
}