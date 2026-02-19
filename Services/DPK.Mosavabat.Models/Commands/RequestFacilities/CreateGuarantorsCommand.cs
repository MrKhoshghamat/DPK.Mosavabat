using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateGuarantorsCommand(
    List<GuarantorCommand> guarantors)
{
    public List<GuarantorCommand> Guarantors { get; } = guarantors;
}