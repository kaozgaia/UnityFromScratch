using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B2BG.WorldCreator;

/*  

Decorator of the Cellular Automata Logic

Performs operations over the map and determines what areas are walkable
     
*/
public class BaseDataDecorator : ProceduralDecorator
{
    [Header("Gameobjects To Spawn")]
    [SerializeField]
    private GameObject _trueChunk;
    [SerializeField]
    private GameObject _falseChunk;
    [Header("Celular Automata Settings")]
    public float chanceToStartAlive = 0.34f;
    public int birthLimit = 4;
    public int deathLimit = 3;
    public int numberOfSteps = 3;

    public override DecoratorCellState[,] CreateMap(DecoratorCellState[,] mainMap)
    {
        SetDimensionsData();
        DecoratorCellState[,] newMap = generateMap(mainMap);
        // Instancing
        DefaultMapLoop(newMap, OnStepLoopMap);


        return newMap;
    }

    private void OnStepLoopMap(int x, int y, DecoratorCellState[,] map)
    {
        
        if(map[x, y] == DecoratorCellState.True)
        {
            GameObject g = Instantiate(_trueChunk);
            g.transform.position = _gen.OriginPoint.position;
            Bounds b = g.GetComponentInChildren<Renderer>().bounds;
            g.transform.position += new Vector3(y * b.size.z, 0, x * b.size.x);
            g.transform.SetParent(_gen.NavMeshFloor.transform);
            _gen.GroundCellsMap[x, y] = g;
            _decorationData[x, y] = g;
        }
        else if(map[x, y] == DecoratorCellState.False)
        {
            GameObject g = Instantiate(_falseChunk);
            g.transform.position = _gen.OriginPoint.position;
            Bounds b = g.GetComponentInChildren<Renderer>().bounds;
            g.transform.position += new Vector3(y * b.size.z, 0, x * b.size.x);
            g.transform.SetParent(_gen.NavMeshFloor.transform);
            _gen.GroundCellsMap[x, y] = g;
            _decorationData[x, y] = g;
        }
        
    }

    public override DecoratorCellState[,] OnCellStep(int x, int y, DecoratorCellState[,] map)
    {
        
        if (Random.Range(0f, 1f) < chanceToStartAlive && map[x, y] == DecoratorCellState.True)
        {
            map[x, y] = DecoratorCellState.True;
        }
        else if(map[x, y] == DecoratorCellState.True)
        {
            map[x, y] = DecoratorCellState.False;
        }
        if (x <= 10 && y <= 10 || (x == y) || (((x > 0 && y > 0)) && x + 1 == y - 1) || (((x < map.GetLength(0) && y < map.GetLength(1))) && x - 1 == y + 1))
        {
            map[x, y] = DecoratorCellState.True;
        }
        
        
        return map;
    }

    private DecoratorCellState[,] DoSimulationStep(DecoratorCellState[,] oldMap)
    {
        DecoratorCellState[,] newMap = new DecoratorCellState[width, height];
        //Loop over each row and column of the map
        for (int x = 0; x < oldMap.GetLength(0); x++)
        {
            for (int y = 0; y < oldMap.GetLength(1); y++)
            {
                
                int nbs = countAliveNeighbours(oldMap, x, y);
                //The new value is based on our simulation rules
                //First, if a cell is alive but has too few neighbours, kill it.
                if (oldMap[x, y] == DecoratorCellState.True)
                {
                    if (nbs < deathLimit)
                    {
                        newMap[x, y] = DecoratorCellState.False;
                    }
                    else
                    {
                        newMap[x, y] = DecoratorCellState.True;
                    }
                }
                //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                else
                {
                    if (nbs > birthLimit)
                    {
                        newMap[x, y] = DecoratorCellState.True;
                    }
                    else
                    {
                        newMap[x, y] = DecoratorCellState.False;
                    }
                }
                
                if (oldMap[x, y] == DecoratorCellState.RoadDiagonal || oldMap[x, y] == DecoratorCellState.RoadSmooth || oldMap[x, y] == DecoratorCellState.RoadStripe)
                {
                    // Re write the new map with the road data
                    newMap[x, y] = oldMap[x, y];
                }
            }
        }
        
        
        return newMap;
    }

    //Returns the number of cells in a ring around (x,y) that are alive.
    private int countAliveNeighbours(DecoratorCellState[,] map, int x, int y)
    {
        int count = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbour_x = x + i;
                int neighbour_y = y + j;

                //If we're looking at the middle point
                if (i == 0 && j == 0)
                {
                    //Do nothing, we don't want to add ourselves in!
                }
                //In case the index we're looking at it off the edge of the map
                else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= map.GetLength(0) || neighbour_y >= map.GetLength(1))
                {
                    count = count + 1;
                }
                //Otherwise, a normal check of the neighbour
                else if (map[neighbour_x, neighbour_y] == DecoratorCellState.True)
                {
                    count = count + 1;
                }
            }
        }
        return count;
    }

    private DecoratorCellState[,] generateMap(DecoratorCellState[,] cellmap)
    {
        //Set up the map with random values
        cellmap = ApplyDecorationRules(cellmap);
        //And now run the simulation for a set number of steps
        for (int i = 0; i < numberOfSteps; i++)
        {
            cellmap = DoSimulationStep(cellmap);
        }
        return cellmap;
    }


}
