using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B2BG.Utils;

namespace B2BG.WorldCreator{
	public abstract class ProceduralDecorator : MonoBehaviour {

        // Events 
        protected delegate DecoratorCellState[,] DecorationCellStep(int x, int y, DecoratorCellState[,] map);
        protected delegate void DefaultLoop(int x, int y, DecoratorCellState[,] map);
        [SerializeField]
		[Range(0f, 1f)]
		protected float _decorationProbability;
        // Main ProceduralWorldGenerator component
		protected ProceduralWorldGenerator _gen;
        // Parent Ground Cells
		protected GameObject[,] _terrainData;
        // Decorator Spawned Gameobjects
		protected GameObject[,] _decorationData;
		protected int width;
		protected int height;

		public virtual void SetWorldCreator(ProceduralWorldGenerator gen){
			_gen = gen;
			_decorationData = new GameObject[_gen.width,_gen.height];
		}

        // Current World State Setup
		protected void SetDimensionsData(){
            
            _terrainData = _gen.GroundCellsMap;
			width = _gen.width;
			height = _gen.height;
			
		}

		public virtual DecoratorCellState[,] CreateMap(DecoratorCellState[,] mainMap){
			SetDimensionsData();
			return ApplyDecorationRules(mainMap);
		}
		public virtual DecoratorCellState[,] ApplyDecorationRules(DecoratorCellState[,] map)
        {
            return ModMap(map, OnCellStep);
        }

		public virtual void DeleteCurrentContent(){
			for(int i=0; i<width; i++){
				for(int j=0; j<height; j++){
					Destroy(_decorationData[i,j]);
				}
			}
            _decorationData = new GameObject[_gen.width, _gen.height];
        }

        public virtual DecoratorCellState[,] OnCellStep(int x, int y, DecoratorCellState[,] map)
        {
            return map;
        }

		protected GameObject RotateRandomNSEW(GameObject g){
			float rotProb = B2BGUtils.GetRand01();
			if(rotProb < 0.25f){
				g.transform.GetChild(0).Rotate(0,90f,0);
			}
			if(rotProb >= 0.25f && rotProb < 0.5f){
				g.transform.GetChild(0).Rotate(0,180f,0);
			}
			if(rotProb >= 0.5f && rotProb < 0.75f){
				g.transform.GetChild(0).Rotate(0,-90f,0);
			}
			return g;
		}

        protected virtual DecoratorCellState[,] ModMap(DecoratorCellState[,] map, DecorationCellStep callback = null)
        {
            if (callback == null)
            {
                return map;
            }
            else
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        map = callback(i, j, map);
                    }
                }
                return map;
            }
        }

        protected virtual void DefaultMapLoop(DecoratorCellState[,] map, DefaultLoop callback = null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    callback(i, j, map);
                }
            }  
        }

        protected virtual void SaveToGroundData(int x, int y,GameObject g)
        {
            g.transform.SetParent(_gen.NavMeshFloor.transform);
            _gen.GroundCellsMap[x, y] = g;
            _decorationData[x, y] = g;
        }

    }
}


