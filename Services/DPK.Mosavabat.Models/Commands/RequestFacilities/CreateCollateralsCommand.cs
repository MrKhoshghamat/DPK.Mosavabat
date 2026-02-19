using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateCollateralsCommand(
    List<CollateralCommand> collaterals)
{
    public List<CollateralCommand> Collaterals { get; } = collaterals;
}