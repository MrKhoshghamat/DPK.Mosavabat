using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.DataTransferObjects;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی می باشد")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "وارد کردن رمز عبور الزامی می باشد")]
    public string? Password { get; set; }
}