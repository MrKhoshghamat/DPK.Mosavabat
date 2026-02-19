using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateGuarantorsCommand(
    List<GuarantorCommand> guarantorCommands)
{
    public List<GuarantorCommand> GuarantorCommands { get; } = guarantorCommands;
}