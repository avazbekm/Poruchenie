using Poruchenie.Domain.Commons;

namespace Poruchenie.Domain.Etities;

public class Jismoniy : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string PINFL {  get; set; } = string.Empty;
    public string CountNumber { get; set; } = string.Empty;
    public string MFO { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
}
