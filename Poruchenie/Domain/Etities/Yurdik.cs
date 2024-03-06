using Poruchenie.Domain.Commons;

namespace Poruchenie.Domain.Etities;

public class Yurdik : Auditable
{
    public string Name { get; set; } = string.Empty;
    public int INN { get; set; }
    public string CountNumber { get; set; } = string.Empty;
    public string MFO { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string Rahbar {  get; set; } = string.Empty;
    public string BoshXisobchi {  get; set; } = string.Empty;
}
