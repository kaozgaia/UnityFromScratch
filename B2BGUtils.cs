using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B2BG.Utils
{
    public struct CameraBounds
    {
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerLeft;
        public Vector3 lowerRight;
    }

    public struct SpawnPosition
    {
        public Vector3 spawnPoint;
        public Quaternion rotation;
    }


    public class Constants
    {

        public static int MAX_MULTIPLE_SHOOTS = 15;
        public static float MINIMUM_MOVING_SHOOT_PROB = 0.15f;

    }

    public enum TurretType{
        Light,
        Cannon,
        Heavy
    }

    public enum CharacterType { player, enemy };

    public enum AIFollowerTurretState { Idle, Following, Alert, Spoted }


    public class B2BGUtils
    {
        public static CameraBounds GetCameraBounds(Transform objTransform, Camera cam = null)
        {
            Camera currentCam = cam == null ? Camera.main : cam;
            float depth = (objTransform.position.z - currentCam.transform.position.z);
            Vector3 upperLeftScreen = new Vector3(0, Screen.height, depth);
            Vector3 upperRightScreen = new Vector3(Screen.width, Screen.height, depth);
            Vector3 lowerLeftScreen = new Vector3(0, 0, depth);
            Vector3 lowerRightScreen = new Vector3(Screen.width, 0, depth);

            CameraBounds bounds = new CameraBounds();

            //Corner locations in world coordinates
            bounds.upperLeft = Camera.main.ScreenToWorldPoint(upperLeftScreen);
            bounds.upperRight = Camera.main.ScreenToWorldPoint(upperRightScreen);
            bounds.lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeftScreen);
            bounds.lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);

            return bounds;
        }

        public static float GetRand01()
        {
            return Random.Range(0f, 1f);
        }

        public static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        public static SpawnPosition CreateStructFromTransform(Transform transform)
        {
            SpawnPosition spawnerPosition = new SpawnPosition();
            spawnerPosition.spawnPoint = transform.position;
            spawnerPosition.rotation = transform.rotation;
            return spawnerPosition;
        }

		public static T GetRandomObj<T>(List<T> source) where T : IGladiator
		{
			// Joust Selection Algorithm
			List<int> arena = new List<int>();
			for(int i = 0; i < source.Count; i++)
			{
				IGladiator item = source[i];
				for (int j = 0; j < item.GetProb(); j++) arena.Add(i);
			}
			int result = Random.Range(0, arena.Count);
			T selected =  source[result];
			return selected;
		}

		public static int GetRandomIdentifier<T>(List<T> source) where T : IGladiator
		{
			// Joust Selection Algorithm
			List<int> arena = new List<int>();
			for(int i = 1; i < source.Count; i++)
			{
				IGladiator item = source[i];
				for (int j = 0; j < item.GetProb(); j++) arena.Add(i);
			}
			return arena[Random.Range(0, arena.Count)];
		}

        public static void SetCanvasGroup(CanvasGroup c, bool open)
        {
            c.alpha = open ? 1f:0f;
            c.blocksRaycasts = open;
            c.interactable = open;
        }


        
    }
    
}





namespace PBUtils
{
    public struct CameraBounds
    {
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerLeft;
        public Vector3 lowerRight;
    }

    public struct SpawnPosition
    {
        public Vector3 spawnPoint;
        public Quaternion rotation;
    }


    public class Constants
    {

        public static int MAX_MULTIPLE_SHOOTS = 15;
        public static float MINIMUM_MOVING_SHOOT_PROB = 0.15f;

    }


    public class Utils
    {
        public static CameraBounds GetCameraBounds(Transform objTransform, Camera cam = null)
        {
            Camera currentCam = cam == null ? Camera.main : cam;
            float depth = (objTransform.position.z - currentCam.transform.position.z);
            Vector3 upperLeftScreen = new Vector3(0, Screen.height, depth);
            Vector3 upperRightScreen = new Vector3(Screen.width, Screen.height, depth);
            Vector3 lowerLeftScreen = new Vector3(0, 0, depth);
            Vector3 lowerRightScreen = new Vector3(Screen.width, 0, depth);

            CameraBounds bounds = new CameraBounds();

            //Corner locations in world coordinates
            bounds.upperLeft = Camera.main.ScreenToWorldPoint(upperLeftScreen);
            bounds.upperRight = Camera.main.ScreenToWorldPoint(upperRightScreen);
            bounds.lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeftScreen);
            bounds.lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);

            return bounds;
        }

        public static float GetRand01()
        {
            return Random.Range(0f, 1f);
        }

        public static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        public static SpawnPosition CreateStructFromTransform(Transform transform)
        {
            SpawnPosition spawnerPosition = new SpawnPosition();
            spawnerPosition.spawnPoint = transform.position;
            spawnerPosition.rotation = transform.rotation;
            return spawnerPosition;
        }
    }



}