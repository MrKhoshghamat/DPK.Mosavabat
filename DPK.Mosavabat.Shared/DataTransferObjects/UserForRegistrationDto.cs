using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.DataTransferObjects;

public record UserForRegistrationDto
{
    [Required(ErrorMessage = "وارد کردن نام کاربر الزامی می باشد")]
    public string? FirstName { get; init; }

    [Required(ErrorMessage = "وارد کردن نام خانوادگی کاربر الزامی می باشد")]
    public string? LastName { get; init; }

    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی می باشد")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "وارد کردن رمز عبور الزامی می باشد")]
    public string? Password { get; init; }
    //public string? Email { get; init; }
    //public string? PhoneNumber { get; init; }
    public ICollection<string>? Roles { get; init; }
}