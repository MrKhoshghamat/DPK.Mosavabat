using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateLcpFilesCommand(
    List<LcpFileCommand> lcpFiles)
{
    public List<LcpFileCommand> LcpFiles { get; } = lcpFiles;
}