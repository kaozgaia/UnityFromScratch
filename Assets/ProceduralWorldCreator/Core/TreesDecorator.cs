using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B2BG.WorldCreator;
using B2BG.Utils;

public class TreesDecorator : ProceduralDecorator {
	[SerializeField]
	private GameObject[] _treesArray;

    public override DecoratorCellState[,] OnCellStep(int x, int y, DecoratorCellState[,] map)
    {
        if (map[x, y] == DecoratorCellState.False && x + 1 < map.GetLength(0) && y + 1 < map.GetLength(1) && x > 0 && y > 0)
        {
            if (B2BGUtils.GetRand01() < _decorationProbability)
            {
                map[x, y] = DecoratorCellState.Tree;
                GameObject parentCell = _terrainData[x, y];
                GameObject g = Instantiate(_treesArray[Random.Range(0, _treesArray.Length)]);
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
                g.transform.SetParent(parentCell.transform);
                _decorationData[x, y] = g;
            }
        }
        return map;
    }
}
