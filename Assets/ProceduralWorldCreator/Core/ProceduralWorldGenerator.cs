using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using B2BG.Utils;

namespace B2BG.WorldCreator{

    public struct MapBounds{
		public int xStart;
		public int xLimit;
		public int yStart;
		public int yLimit;

	}

	public class ProceduralWorldGenerator : MonoBehaviour {

        // Inspector Assigned
        [Header("Origin Point")]
        [SerializeField]
		private Transform startPoint;
        [Header("New NavMesh Surface")]
        [SerializeField]
        private NavMeshSurface navMeshSurface;
        [Header("World Dimension Settings")]
		public int width = 4;
		public int height = 4;


        // Private

		private DecoratorCellState[,] _map;
		private GameObject[,] _cellsMap;
		private ProceduralDecorator[] _decorators;

        // Public

		public GameObject[,] GroundCellsMap{
			get{
				return _cellsMap;
			}
		}

        public GameObject NavMeshFloor
        {
            get
            {
                return navMeshSurface.gameObject;
            }
        }

        public Transform OriginPoint
        {
            get
            {
                return startPoint;
            }
        }

		// Use this for initialization
		void Start () 
		{
			
			_decorators = GetComponents<ProceduralDecorator>();
			for(int i = 0; i < _decorators.Length;i++)
			{
				_decorators[i].SetWorldCreator(this);
			}
		}

        #region WORLD CREATOR

        public void StartWorldGeneration(){
			ResetBaseFloor();
            DeleteCurrentModel();
            _map = CleanMap(_map);
            CreateWorld();
		}

        private DecoratorCellState[,] CleanMap(DecoratorCellState[,] map)
        {
            map = new DecoratorCellState[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = DecoratorCellState.True;
                }
            }
            return map;
        }

        private void CreateWorld(){
			if(startPoint != null){
				_cellsMap = new GameObject[width, height];
				DecorateWorld();
                if (navMeshSurface != null)
                {
                    navMeshSurface.BuildNavMesh();
                }
            } 
		}

        #endregion

        void DeleteCurrentModel(){
            foreach(ProceduralDecorator d in _decorators)
            {
                d.DeleteCurrentContent();
            }
        }

		private void DecorateWorld(){
            // Decoration Steps
            for (int i = 0; i < _decorators.Length; i++)
            {
                _map = _decorators[i].CreateMap(_map);
            }
            //RotateBaseFloor(-45f);
		}

        private void DebugLogMap(DecoratorCellState[,] map)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Debug.Log(string.Format("On :{0} {1} the value is {2}", x, y,map[x,y]));
                }
            }
        }

		private void ResetBaseFloor(){
			
			Vector3 rot = navMeshSurface.transform.rotation.eulerAngles;
			rot = new Vector3(rot.x,0f,rot.z);
			navMeshSurface.transform.rotation = Quaternion.Euler(rot);
		}

		private void RotateBaseFloor(float angle){
			
			Vector3 rot = navMeshSurface.transform.rotation.eulerAngles;
			rot = new Vector3(rot.x,rot.y+angle,rot.z);
			navMeshSurface.transform.rotation = Quaternion.Euler(rot);
		}        


    }
}

