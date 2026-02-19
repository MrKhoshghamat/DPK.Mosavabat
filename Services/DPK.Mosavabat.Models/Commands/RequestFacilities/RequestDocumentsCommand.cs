using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class RequestDocumentsCommand(
    List<RequestDocumentCommand> requestDocuments)
{
    public List<RequestDocumentCommand> RequestDocuments { get; } = requestDocuments;
}