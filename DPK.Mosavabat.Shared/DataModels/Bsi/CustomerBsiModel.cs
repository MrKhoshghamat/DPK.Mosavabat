using DPK.Mosavabat.Common.Enums.Customer;
using DPK.Mosavabat.Common.Exceptions;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;

namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class CustomerBsiModel
{
    public decimal CustomerNumber { get; set; }// شماره مشتری
    public short CustomerType { get; set; }// نوع مشتری
    public decimal? NationalIdNumber { get; set; }// کد ملی حقیقی ایرانی
    public string ForeignerIdNumber { get; set; }// شناسه اتباع بیگانه-کد فراگیر(حقیقی)
    public decimal? CorporateCustomerIdNumber { get; set; }// شناسه حقوقی
    public string? IdentificationNumber { get; set; }// شماره مدرک شناسایی
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

    public void SetBranch(IEnumerable<SelectedItems<int>> branches)
    {
        Branches = branches;
        Token = StringExtension.RandomString();
    }

    public void Time()
    {
        var stringProperties = GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string));

        foreach (var stringProperty in stringProperties)
        {
            var currentValue = (string)stringProperty.GetValue(this, null)!;
            stringProperty.SetValue(this, currentValue.Trim(), null);
        }
    }
    /// <summary>
    /// سریال شناسنامه
    /// </summary>
    /// <returns></returns>
    public string GetSerial()
    {
        var result = string.Empty;

        if (GetCustomerType() == Common.Enums.Customer.CustomerType.Foreigners)
            return result;

        if (IdentityCertificateSequence.IsNullOrWhiteSpace() || IdentityCertificateSerial.IsNullOrWhiteSpace())
            return result;

        if (IdentityCertificateSequence.Length < 2 || IdentityCertificateSerial.Length < 6)
            return result;
        try
        {
            var sequence = IdentityCertificateSequence.Trim().Replace("ا", "الف");
            result = $"{sequence[..^2]}-{sequence.Substring(sequence.Length - 2, 2)}{IdentityCertificateSerial.Trim()}";
        }
        catch
        {
            // ignored
        }

        return result;
    }

    public bool ValidData()
    {
        return LastName != null && Name != null;
    }

    public CustomerType GetCustomerType()
    {
        if (CorporateCustomerIdNumber > 0)
            return Common.Enums.Customer.CustomerType.Legal;
        return ForeignerIdNumber.Replace("0", "").Length > 0
            ? Common.Enums.Customer.CustomerType.Foreigners
            : Common.Enums.Customer.CustomerType.Real;


    }
    public decimal GetNationalId()
    {
        if (NationalIdNumber == null) return 0;
        if (CorporateCustomerIdNumber != null)
            return GetCustomerType() switch
            {
                Common.Enums.Customer.CustomerType.Foreigners => ForeignerIdNumber.ToDecimal(),
                Common.Enums.Customer.CustomerType.Real => NationalIdNumber.Value,
                Common.Enums.Customer.CustomerType.Legal => CorporateCustomerIdNumber.Value,
                _ => throw new DpkException("شناسه / کد ملی یافت نشد")
            };

        return 0;
    }

    public string? GetShNo()
    {
        return GetCustomerType() == Common.Enums.Customer.CustomerType.Real ? IdentificationNumber : null;
    }

    public string GetBirthDate()
    {
        var type = GetCustomerType();
        var birthDate = type == Common.Enums.Customer.CustomerType.Legal ? RegistrationDate : BirthDate;
        var res = birthDate.Replace(" ", "");
        if (type == Common.Enums.Customer.CustomerType.Foreigners && res.IsGregorianDate())
        {
            res = res.MiladiToShamsi();
        }

        return res;
    }
}