using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.API.Models;
using WaterJugChallenge.API.Services;

namespace WaterJugChallenge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WaterJugController : ControllerBase
{
    private readonly IWaterJugService _waterJugService;
    private readonly ILogger<WaterJugController> _logger;

    public WaterJugController(IWaterJugService waterJugService, ILogger<WaterJugController> logger)
    {
        _waterJugService = waterJugService;
        _logger = logger;
    }

    /// <summary>
    /// Solves the water jug problem using the given jug capacities and target amount
    /// </summary>
    /// <param name="request">The water jug problem parameters</param>
    /// <returns>The solution steps or error message</returns>
    [HttpPost("solve")]
    [ProducesResponseType(typeof(WaterJugResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<WaterJugResponse> SolveWaterJugProblem([FromBody] WaterJugRequest request)
    {
        try
        {
            _logger.LogInformation("Solving water jug problem: X={XCapacity}, Y={YCapacity}, Z={ZAmountWanted}", 
                request.XCapacity, request.YCapacity, request.ZAmountWanted);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _waterJugService.SolveWaterJugProblem(request);

            if (response.HasSolution)
            {
                _logger.LogInformation("Solution found with {StepCount} steps", response.Solution?.Count ?? 0);
                return Ok(response);
            }
            else
            {
                _logger.LogInformation("No solution found: {Message}", response.Message);
                return Ok(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while solving water jug problem");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new WaterJugResponse 
                { 
                    HasSolution = false, 
                    Message = "An internal server error occurred while processing the request." 
                });
        }
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns>Health status</returns>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> Health()
    {
        return Ok("Water Jug Challenge API is running!");
    }
}
