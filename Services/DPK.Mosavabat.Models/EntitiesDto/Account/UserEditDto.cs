using System.ComponentModel.DataAnnotations;
using DPK.Foundation.Auditing;
using DPK.Foundation.Authorization.Users;
using DPK.Foundation.Domain.Entities;

namespace DPK.Mosavabat.Models.EntitiesDto.Account;

public class UserEditDto : IPassivable
{
    /// <summary>
    /// Set null to create a new user. Set user's Id to update a user
    /// </summary>
    public long? Id { get; set; }

    [Required]
    [StringLength(DpkUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(DpkUserBase.MaxSurnameLength)]
    public string Surname { get; set; }

    [Required]
    [StringLength(DpkUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(DpkUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    [StringLength(DpkUserBase.MaxPhoneNumberLength)]
    public string PhoneNumber { get; set; }

    // Not used "Required" attribute since empty value is used to 'not change password'
    [StringLength(DpkUserBase.MaxPlainPasswordLength)]
    [DisableAuditing]
    public string Password { get; set; }

    public bool IsActive { get; set; }

    public bool ShouldChangePasswordOnNextLogin { get; set; }

    public virtual bool IsTwoFactorEnabled { get; set; }

    public virtual bool IsLockoutEnabled { get; set; }

}