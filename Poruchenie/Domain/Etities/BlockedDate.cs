using Poruchenie.Domain.Commons;

namespace Poruchenie.Domain.Etities;

public class BlockedDate:Auditable
{
    public string BlockDate { get; set; } = string.Empty;
    public string OldCode { get; set; } = string.Empty;
}
