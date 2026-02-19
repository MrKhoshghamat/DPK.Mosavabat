using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class LcpUpdateHistoryLogsCommand(
    List<LcpUpdateHistoryLogCommand> lcpUpdateHistoryLogCommands)
{
    public List<LcpUpdateHistoryLogCommand> LcpUpdateHistoryLogCommands { get; } = lcpUpdateHistoryLogCommands;
}