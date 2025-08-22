using System.ComponentModel.DataAnnotations;

namespace WaterJugChallenge.API.Models;

public class WaterJugRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "X capacity must be a positive integer")]
    public int XCapacity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Y capacity must be a positive integer")]
    public int YCapacity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Z amount wanted must be a positive integer")]
    public int ZAmountWanted { get; set; }
}
