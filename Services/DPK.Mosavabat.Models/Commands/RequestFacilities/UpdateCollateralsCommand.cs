using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateCollateralsCommand(
    List<CollateralCommand> collateralCommands)
{
    public List<CollateralCommand> CollateralCommands { get; } = collateralCommands;
}