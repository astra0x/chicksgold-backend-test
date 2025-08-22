@echo off
echo Water Jug Challenge - .NET 9.0 Solution
echo ======================================
echo.
echo Building the solution...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Running the API...
cd WaterJugChallenge.API
start https://localhost:7001/swagger
echo API is starting... Swagger UI will open automatically.
echo.
echo Test the API at: https://localhost:7001/swagger
echo.
dotnet run

pause
