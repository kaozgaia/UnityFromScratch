using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBUtils;

public class WayPointNetwork : MonoSingleton<WayPointNetwork> {

    [Range(0.2f, 2f)]public float hOffset = 1f;
    [Range(0.2f, 2f)]public float vOffset = 1f;
    public float bottomTreshold = 3f;
    public Transform bottomCheckpoint;

    public List<Vector3> waypoints = new List<Vector3>();

    private CameraBounds bounds;
    private int horizontalIterations;
    private int verticalIterations;

    // Use this for initialization
    void Awake () {
        bounds = Utils.GetCameraBounds(transform);

        // Calculate distance from left to right
        float hDistance = Vector3.Distance(bounds.lowerLeft, bounds.lowerRight);
        
        // Calculate distance from top to down minus a bottom treshold
        float vDistance = Vector3.Distance(bounds.upperLeft, bounds.lowerLeft);
        
        horizontalIterations = (int)(hDistance / hOffset);
        verticalIterations = (int)((vDistance-bottomTreshold) / vOffset);

        for(int i = 1; i <= verticalIterations; i++)
        {  
            for (int j = 1; j <= horizontalIterations; j++)
            {
                Vector3 position = new Vector3(bounds.upperLeft.x + (hOffset*j), bounds.upperLeft.y - (vOffset*i), bounds.upperLeft.z);
                waypoints.Add(position);
            }
        }
	}

    public Vector3 GetRandomPoint()
    {
        return waypoints[Random.Range(0, waypoints.Count)];
    }

    public Vector3 GetBottomWaypoint()
    {
        return bottomCheckpoint.position;
    }

    public Vector3 GetRandomTopPoint()
    {
        return waypoints[Random.Range(0, horizontalIterations - 1)];
    }

    public Vector3 GetMiddleTopPoint()
    {
        return waypoints[horizontalIterations / 2];
    }
	
    public List<Vector3> GetRandomDifferentPoints(int dataLenght)
    {
        List<Vector3> result = new List<Vector3>();
        if(dataLenght < waypoints.Count)
        {
            for(int i = 0; i < dataLenght; i++)
            {
                result.Add(GetRandomPoint());
            }            
        }
        return result;
    }

}
