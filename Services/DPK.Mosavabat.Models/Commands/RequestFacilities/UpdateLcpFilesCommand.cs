using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateLcpFilesCommand(
    List<LcpFileCommand> lcpFileCommands)
{
    public List<LcpFileCommand> LcpFileCommands { get; } = lcpFileCommands;
}