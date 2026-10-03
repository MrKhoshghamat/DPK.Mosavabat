namespace DPK.Mosavabat.Common.Enums.Customer;

public enum RuleType
{
    /// <summary>
    /// امکان ویرایش تمامی موارد
    /// </summary>
    EditableAll = 1,
    /// <summary>
    /// نمایش نام و نام خانوادگی و امکان ویرایش تمامی اقلام
    /// </summary>
    EditableWithoutName,
    /// <summary>
    /// نمایش نام و نام خانوادگی و امکان ویرایش کد پستی و آدرس
    /// </summary>
    EditableAddressAndPostalCode,
    /// <summary>
    /// نمایش نام و نام خانوادگی و عدم اجازه ویرایش
    /// </summary>
    AvoidEditing,
    /// <summary>
    /// عدم امکان نمایش اطلاعات و ثبت
    /// </summary>
    AvoidAllData,
}