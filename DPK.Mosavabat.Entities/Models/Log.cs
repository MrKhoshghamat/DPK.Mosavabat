using System.ComponentModel.DataAnnotations.Schema;

namespace DPK.Mosavabat.Entities.Models;

public class Log
{
    [Column(Order = 0)]
    public int Id { get; set; }
    [Column(Order = 1)]
    public string? Level { get; set; }
    [Column(Order = 2)]
    public string? Message { get; set; }
    [Column(Order = 3)]
    public DateTime CreationTime { get; set; }
    [Column(Order = 4)]
    public string? StackTrace { get; set; }
    [Column(Order = 5)]
    public string? ProcessTime { get; set; }
    [Column(Order = 6)]
    public string? LocalIp { get; set; }
    [Column(Order = 7)]
    public Guid Guid { get; set; }
}