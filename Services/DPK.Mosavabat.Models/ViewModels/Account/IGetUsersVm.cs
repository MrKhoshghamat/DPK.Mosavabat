using System.Collections.Generic;

namespace DPK.Mosavabat.Models.ViewModels.Account;

public interface IGetUsersVm
{
    public string Filter { get; set; }

    public List<string> Permissions { get; set; }

    public int? Role { get; set; }

    public bool OnlyLockedUsers { get; set; }
}