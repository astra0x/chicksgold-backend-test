namespace WaterJugChallenge.API.Models;

public class WaterJugResponse
{
    public bool HasSolution { get; set; }
    public string? Message { get; set; }
    public List<WaterJugStep>? Solution { get; set; }
}

public class WaterJugStep
{
    public int Step { get; set; }
    public int BucketX { get; set; }
    public int BucketY { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Status { get; set; }
}
