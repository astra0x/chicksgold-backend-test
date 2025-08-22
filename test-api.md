# API Testing Guide

## Prerequisites
- .NET 9.0 SDK installed
- API running on https://localhost:7001

## Quick Test Commands

### 1. Health Check
```bash
curl -k https://localhost:7001/api/WaterJug/health
```

### 2. Basic Water Jug Problem (X=2, Y=10, Z=4)
```bash
curl -k -X POST https://localhost:7001/api/WaterJug/solve \
  -H "Content-Type: application/json" \
  -d "{\"xCapacity\": 2, \"yCapacity\": 10, \"zAmountWanted\": 4}"
```

### 3. Large Numbers Test (X=2, Y=100, Z=96)
```bash
curl -k -X POST https://localhost:7001/api/WaterJug/solve \
  -H "Content-Type: application/json" \
  -d "{\"xCapacity\": 2, \"yCapacity\": 100, \"zAmountWanted\": 96}"
```

### 4. No Solution Case (X=2, Y=6, Z=5)
```bash
curl -k -X POST https://localhost:7001/api/WaterJug/solve \
  -H "Content-Type: application/json" \
  -d "{\"xCapacity\": 2, \"yCapacity\": 6, \"zAmountWanted\": 5}"
```

### 5. Invalid Input Test (Negative X Capacity)
```bash
curl -k -X POST https://localhost:7001/api/WaterJug/solve \
  -H "Content-Type: application/json" \
  -d "{\"xCapacity\": -1, \"yCapacity\": 10, \"zAmountWanted\": 4}"
```

### 6. Target Too Large Test (Z > max(X,Y))
```bash
curl -k -X POST https://localhost:7001/api/WaterJug/solve \
  -H "Content-Type: application/json" \
  -d "{\"xCapacity\": 2, \"yCapacity\": 10, \"zAmountWanted\": 15}"
```

## Expected Results

1. **Health Check**: Should return "Water Jug Challenge API is running!"
2. **Basic Problem**: Should return a 4-step solution
3. **Large Numbers**: Should return a 4-step solution efficiently
4. **No Solution**: Should return hasSolution: false with explanation
5. **Invalid Input**: Should return HTTP 400 with validation error
6. **Target Too Large**: Should return hasSolution: false with explanation

## Web Interface

Open https://localhost:7001/swagger in your browser to test the API interactively.
