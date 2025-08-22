using WaterJugChallenge.API.Models;

namespace WaterJugChallenge.API.Services;

public class WaterJugService : IWaterJugService
{
    public WaterJugResponse SolveWaterJugProblem(WaterJugRequest request)
    {
        // Validate that Z is achievable
        if (request.ZAmountWanted > Math.Max(request.XCapacity, request.YCapacity))
        {
            return new WaterJugResponse
            {
                HasSolution = false,
                Message = $"Target amount {request.ZAmountWanted} is greater than the maximum jug capacity. No solution possible."
            };
        }

        // Check if Z is achievable using Bézout's identity
        if (!IsSolvable(request.XCapacity, request.YCapacity, request.ZAmountWanted))
        {
            return new WaterJugResponse
            {
                HasSolution = false,
                Message = $"No solution possible for the given jug capacities and target amount."
            };
        }

        var solution = FindOptimalSolution(request.XCapacity, request.YCapacity, request.ZAmountWanted);
        
        return new WaterJugResponse
        {
            HasSolution = true,
            Solution = solution
        };
    }

    private bool IsSolvable(int x, int y, int z)
    {
        // Using Bézout's identity: ax + by = gcd(x,y)
        // If z is divisible by gcd(x,y), then a solution exists
        return z % GCD(x, y) == 0;
    }

    private int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private List<WaterJugStep> FindOptimalSolution(int xCapacity, int yCapacity, int target)
    {
        var visited = new HashSet<string>();
        var queue = new Queue<WaterJugState>();
        var parent = new Dictionary<string, WaterJugState>();
        
        // Start with both jugs empty
        var initialState = new WaterJugState(0, 0);
        queue.Enqueue(initialState);
        visited.Add(initialState.ToString());
        
        WaterJugState? targetState = null;
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            // Check if we've reached the target
            if (current.X == target || current.Y == target)
            {
                targetState = current;
                break;
            }
            
            // Generate all possible next states
            var nextStates = GenerateNextStates(current, xCapacity, yCapacity);
            
            foreach (var nextState in nextStates)
            {
                var key = nextState.ToString();
                if (!visited.Contains(key))
                {
                    visited.Add(key);
                    queue.Enqueue(nextState);
                    parent[key] = current;
                }
            }
        }
        
        if (targetState == null)
        {
            return new List<WaterJugStep>();
        }
        
        // Reconstruct the path
        return ReconstructPath(initialState, targetState, parent, xCapacity, yCapacity);
    }
    
    private List<WaterJugState> GenerateNextStates(WaterJugState current, int xCapacity, int yCapacity)
    {
        var states = new List<WaterJugState>();
        
        // Fill jug X
        states.Add(new WaterJugState(xCapacity, current.Y));
        
        // Fill jug Y
        states.Add(new WaterJugState(current.X, yCapacity));
        
        // Empty jug X
        states.Add(new WaterJugState(0, current.Y));
        
        // Empty jug Y
        states.Add(new WaterJugState(current.X, 0));
        
        // Transfer from X to Y
        int transferToY = Math.Min(current.X, yCapacity - current.Y);
        if (transferToY > 0)
        {
            states.Add(new WaterJugState(current.X - transferToY, current.Y + transferToY));
        }
        
        // Transfer from Y to X
        int transferToX = Math.Min(current.Y, xCapacity - current.X);
        if (transferToX > 0)
        {
            states.Add(new WaterJugState(current.X + transferToX, current.Y - transferToX));
        }
        
        return states;
    }
    
    private List<WaterJugStep> ReconstructPath(WaterJugState start, WaterJugState end, 
        Dictionary<string, WaterJugState> parent, int xCapacity, int yCapacity)
    {
        var path = new List<WaterJugState>();
        var current = end;
        
        while (!current.Equals(start))
        {
            path.Add(current);
            var key = current.ToString();
            if (parent.ContainsKey(key))
            {
                current = parent[key];
            }
            else
            {
                break;
            }
        }
        path.Add(start);
        path.Reverse();
        
        return ConvertToSteps(path, xCapacity, yCapacity);
    }
    
    private List<WaterJugStep> ConvertToSteps(List<WaterJugState> path, int xCapacity, int yCapacity)
    {
        var steps = new List<WaterJugStep>();
        
        for (int i = 0; i < path.Count - 1; i++)
        {
            var current = path[i];
            var next = path[i + 1];
            
            var step = new WaterJugStep
            {
                Step = i + 1,
                BucketX = next.X,
                BucketY = next.Y,
                Action = DetermineAction(current, next, xCapacity, yCapacity)
            };
            
            // Check if this step solves the problem
            if (next.X == 0 && next.Y == 0)
            {
                step.Status = "Solved";
            }
            
            steps.Add(step);
        }
        
        return steps;
    }
    
    private string DetermineAction(WaterJugState current, WaterJugState next, int xCapacity, int yCapacity)
    {
        if (next.X == xCapacity && current.X != xCapacity)
        {
            return "Fill bucket X";
        }
        if (next.Y == yCapacity && current.Y != yCapacity)
        {
            return "Fill bucket Y";
        }
        if (next.X == 0 && current.X != 0)
        {
            return "Empty bucket X";
        }
        if (next.Y == 0 && current.Y != 0)
        {
            return "Empty bucket Y";
        }
        if (next.X > current.X)
        {
            return "Transfer from bucket Y to X";
        }
        if (next.Y > current.Y)
        {
            return "Transfer from bucket X to Y";
        }
        
        return "Unknown action";
    }
}

public class WaterJugState
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public WaterJugState(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public override string ToString()
    {
        return $"{X},{Y}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is WaterJugState other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
