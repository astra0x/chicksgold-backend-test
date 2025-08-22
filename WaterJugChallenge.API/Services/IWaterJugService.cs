using WaterJugChallenge.API.Models;

namespace WaterJugChallenge.API.Services;

public interface IWaterJugService
{
    WaterJugResponse SolveWaterJugProblem(WaterJugRequest request);
}
