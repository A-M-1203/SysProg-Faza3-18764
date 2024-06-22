namespace SysProg_faza3.Models;

public class RestourantModel
{
    public required string Name { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsClosed { get; set; }
    public required string Price { get; set; }
}
