using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B2BG.WorldCreator;
using B2BG.Utils;

public class BuildingsDecorator : ProceduralDecorator
{
    [SerializeField]
    private GameObject[] _buildings;

    // Body of instructions to apply in every step
    public override DecoratorCellState[,] OnCellStep(int x, int y, DecoratorCellState[,] map)
    {
        if (map[x, y] == DecoratorCellState.False && x + 1 < map.GetLength(0) && y + 1 < map.GetLength(1) && x > 0 && y > 0)
        {
            int emptyNeighboursCells = checkNeighboursForBuildings(map, x, y);
            if (emptyNeighboursCells > 3 && B2BGUtils.GetRand01() < _decorationProbability)
            {
                map[x, y] = DecoratorCellState.Origin;
                map[x + 1, y] = DecoratorCellState.Fragment;
                map[x, y + 1] = DecoratorCellState.Fragment;
                map[x + 1, y + 1] = DecoratorCellState.Fragment;

                GameObject g = Instantiate(_buildings[Random.Range(0, _buildings.Length)]);
                GameObject parentCell = _terrainData[x, y];
                Bounds bounds;
                if (g.GetComponent<Renderer>() == null)
                {
                    bounds = g.GetComponentInChildren<Renderer>().bounds;
                }
                else
                {
                    bounds = g.GetComponent<Renderer>().bounds;
                }
                g.transform.position = parentCell.transform.position;
                g.transform.position += new Vector3(1, 0, 1);
                g = RotateRandomNSEW(g);
                g.transform.SetParent(parentCell.transform);
                _decorationData[x, y] = g;
            }
        }
        return map;
    }

	private int checkNeighboursForBuildings(DecoratorCellState[,] map, int x, int y){
		int count = 0;
		for(int i=0; i<2; i++){
			for(int j=0; j<2; j++){
				int neighbour_x = x+i;
				int neighbour_y = y+j;
				if(map[neighbour_x,neighbour_y] == DecoratorCellState.False){
					count = count + 1;
				}
			}
		}
		return count;
	}

}
