# Water Jug Challenge - .NET Core Solution

## Overview

This is a .NET Core Web API solution for the classic Water Jug Riddle. The application provides a RESTful API that can compute the optimal steps required to measure exactly Z gallons using two jugs of capacities X and Y gallons.

## Problem Description

The Water Jug Problem is a classic algorithmic puzzle where you have two jugs of different capacities and need to measure a specific amount of water using only these jugs. The allowed operations are:
- Fill a jug to its full capacity
- Empty a jug completely
- Transfer water from one jug to another

## Solution Architecture

### Technology Stack
- **.NET 9.0** - Latest version of .NET
- **ASP.NET Core Web API** - Modern, high-performance web framework
- **Swagger/OpenAPI** - API documentation and testing interface

### Project Structure
```
WaterJugChallenge/
├── WaterJugChallenge.API/           # Main Web API project
│   ├── Controllers/                 # API controllers
│   ├── Models/                      # Data models and DTOs
│   ├── Services/                    # Business logic and algorithm
│   ├── Program.cs                   # Application entry point
│   └── appsettings.json            # Configuration file
├── WaterJugChallenge.sln            # Solution file
├── run.bat                          # Windows batch file to run the project
└── README.md                        # This documentation file
```

## Algorithm Implementation

### Core Algorithm: Breadth-First Search (BFS)

The solution uses a **Breadth-First Search** algorithm to find the optimal (shortest) path to the target amount. This ensures that the solution found requires the minimum number of steps.

#### Key Features:
1. **Optimal Solution**: BFS guarantees the shortest path to the target
2. **Mathematical Validation**: Uses Bézout's identity to determine if a solution exists
3. **State Management**: Efficiently tracks jug states to avoid cycles
4. **Performance Optimized**: Uses HashSet for O(1) state lookup

#### Mathematical Foundation:
- **Bézout's Identity**: For any integers a and b, there exist integers x and y such that ax + by = gcd(a,b)
- **Solvability Check**: A target amount Z is achievable if and only if Z is divisible by gcd(X, Y)
- **GCD Calculation**: Uses Euclidean algorithm for efficient computation

## API Endpoints

### Base URL
```
https://localhost:7001/api/WaterJug
```

### 1. Solve Water Jug Problem
**POST** `/solve`

Solves the water jug problem with the given parameters.

#### Request Body
```json
{
  "xCapacity": 2,
  "yCapacity": 10,
  "zAmountWanted": 4
}
```

#### Response Format
**Success Response (200 OK)**
```json
{
  "hasSolution": true,
  "solution": [
    {
      "step": 1,
      "bucketX": 2,
      "bucketY": 0,
      "action": "Fill bucket X"
    },
    {
      "step": 2,
      "bucketX": 0,
      "bucketY": 2,
      "action": "Transfer from bucket X to Y"
    },
    {
      "step": 3,
      "bucketX": 2,
      "bucketY": 2,
      "action": "Fill bucket X"
    },
    {
      "step": 4,
      "bucketX": 0,
      "bucketY": 4,
      "action": "Transfer from bucket X to Y",
      "status": "Solved"
    }
  ]
}
```

**No Solution Response (200 OK)**
```json
{
  "hasSolution": false,
  "message": "No solution possible for the given jug capacities and target amount."
}
```

#### Validation Rules
- All parameters must be positive integers
- X and Y capacities must be greater than 0
- Z amount wanted must be greater than 0

### 2. Health Check
**GET** `/health`

Returns the health status of the API.

#### Response
```
Water Jug Challenge API is running!
```

## Setup and Installation

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd WaterJugChallenge
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Run the Application**
   ```bash
   cd WaterJugChallenge.API
   dotnet run
   ```

   **Or use the provided batch file on Windows:**
   ```bash
   run.bat
   ```

5. **Access the API**
    - API Base URL: `https://localhost:7001/`

## Example Usage

### Example 1: Basic Case
**Input:**
- Bucket X: 2 gallons
- Bucket Y: 10 gallons
- Target: 4 gallons

**Solution:**
1. Fill bucket X (2, 0)
2. Transfer from X to Y (0, 2)
3. Fill bucket X (2, 2)
4. Transfer from X to Y (0, 4) - Solved!

### Example 2: Large Numbers
**Input:**
- Bucket X: 2 gallons
- Bucket Y: 100 gallons
- Target: 96 gallons

**Solution:**
1. Fill bucket Y (0, 100)
2. Transfer from Y to X (2, 98)
3. Empty bucket X (0, 98)
4. Transfer from Y to X (2, 96) - Solved!

### Example 3: No Solution
**Input:**
- Bucket X: 2 gallons
- Bucket Y: 6 gallons
- Target: 5 gallons

**Result:** No solution possible (mathematically impossible)

## Performance Characteristics

- **Time Complexity**: O(X × Y) in worst case, but typically much better due to mathematical constraints
- **Space Complexity**: O(X × Y) for storing visited states
- **Scalability**: Handles large numbers efficiently through mathematical optimization

## Error Handling

### Validation Errors (400 Bad Request)
- Invalid input parameters
- Negative or zero values
- Missing required fields

### Business Logic Errors (200 OK with Error Message)
- Target amount too large
- Mathematically impossible scenarios

### System Errors (500 Internal Server Error)
- Unexpected exceptions
- System failures

