using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B2BG.WorldCreator;
using B2BG.Utils;

public class RoadDecorator : ProceduralDecorator
{
    // Inspector Assigned

    [Header("Algorithm Parameters")]
    [SerializeField]
    private GameObject _roadStripeChunk;
    [SerializeField]
    private GameObject _roadSmoothChunk;
    [SerializeField]
    private int _maxDirectionalSteps = 50;
    [SerializeField]
    [Range(15, 40)]
    private int _selectableEdge = 30;

    // Private



    // Public 

    
    // Methods
    public override DecoratorCellState[,] ApplyDecorationRules(DecoratorCellState[,] map)
    {
        // Road Map Logic
        map = ModMap(map, DiagonalValidation);
        map = EdgeRoads(map);
        map = EdgeRoads(map);
        map = EdgeRoads(map);
        map = EdgeRoads(map);
        return map;
    }

    private DecoratorCellState[,] EdgeRoads(DecoratorCellState[,] map)
    {
        // Variable that determines if 
        bool onXAxis = B2BGUtils.GetRand01() > 0.5;
        int startEdge;
        do
        {
            startEdge = Random.Range(12, _selectableEdge);
        } while (onXAxis ? map[0, startEdge] != DecoratorCellState.True : map[startEdge, 0] != DecoratorCellState.True);
        for(int i = 0; i < _maxDirectionalSteps;  i++)
        {
            
            if (onXAxis)
            {
                GameObject g;
                // Instantiate road stripe Chunk
                if (map[i, startEdge] == DecoratorCellState.RoadDiagonal)
                {
                    g = Instantiate(_roadSmoothChunk);
                    map[i, startEdge] = DecoratorCellState.RoadSmooth;
                }
                else
                {
                    g = Instantiate(_roadStripeChunk);
                    map[i, startEdge] = DecoratorCellState.RoadStripe;
                }
                
                g.transform.position = _gen.OriginPoint.position;
                Bounds b = g.GetComponentInChildren<Renderer>().bounds;
                g.transform.position += new Vector3(startEdge * b.size.x, 0.02f, i*b.size.z);
                g.transform.Rotate(new Vector3(0, -90, 0));
                SaveToGroundData(startEdge, i, g);
            }
            else
            {
                GameObject g;
                // Instantiate road stripe Chunk
                if (map[startEdge, i] != DecoratorCellState.True)
                {
                    g = Instantiate(_roadSmoothChunk);
                }
                else
                {
                    g = Instantiate(_roadStripeChunk);
                }
                map[startEdge, i] = DecoratorCellState.RoadStripe;
                g.transform.position = _gen.OriginPoint.position;
                Bounds b = g.GetComponentInChildren<Renderer>().bounds;
                g.transform.position += new Vector3(i*b.size.x, 0.02f, startEdge * b.size.z);
                
                SaveToGroundData(startEdge, i, g);
            }
        }
        return map;
    }

    private DecoratorCellState[,] DiagonalValidation(int x, int y, DecoratorCellState[,] map)
    {
        // Diagonal Validation
        if(x == y && x > 4 && y > 4)
        {
            // If we are in the diagonal
            map[x, y] = DecoratorCellState.RoadDiagonal;
            // Instantiate road stripe Chunk
            GameObject g = Instantiate(_roadStripeChunk);
            g.transform.position = _gen.OriginPoint.position;
            Bounds b = g.GetComponentInChildren<Renderer>().bounds;
            g.transform.position += new Vector3(x*b.size.x, 0.01f , y*b.size.z);
            g.transform.Rotate(new Vector3(0, -45, 0));
            SaveToGroundData(x, y, g);
        }

        return map;
    }

}
