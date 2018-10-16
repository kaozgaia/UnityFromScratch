using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossStateMachine : AIStateMachine
{
    // Inspector Assigned
    
    // Protected
    [SerializeField]
    protected int _currentWaypoint = -1;

    // Private


    // Public Properties
    public BaseShootSpawner[] shootersList;
    public BossEnemyShooterEntity enemyEntity;


    protected override void Start()
    {
        
        // ShootersList is used before, so we initialize it
        shootersList = GetComponentsInChildren<BaseShootSpawner>();
        enemyEntity = GetComponent<BossEnemyShooterEntity>();
        if (enemyEntity) enemyEntity.enemyStateMachine = this;
        base.Start();

    }


    // -----------------------------------------------------------------------------
    // Name	:	GetWaypointPosition
    // Desc	:	Fetched the world space position of the state machine's currently
    //			set waypoint with optional increment
    // -----------------------------------------------------------------------------
    public Vector3 GetWaypointPosition(bool increment)
    {
        if (_currentWaypoint == -1 || increment == false)
        {
            _currentWaypoint = Random.Range(0, WayPointNetwork.instance.waypoints.Count - 1);
        }
        else if (increment)
            NextWaypoint();

        // Fetch the new waypoint from the waypoint list
        if (WayPointNetwork.instance.waypoints[_currentWaypoint] != null)
        {
            Vector3 newWaypoint = WayPointNetwork.instance.waypoints[_currentWaypoint];

            // This is our new target position


            return newWaypoint;
        }

        return Vector3.zero;
    }

    // -------------------------------------------------------------------------
    // Name	:	NextWaypoint
    // Desc	:	Called to select a new waypoint. Either randomly selects a new
    //			waypoint from the waypoint network or increments the current
    //			waypoint index 
    // -------------------------------------------------------------------------
    private void NextWaypoint()
    {
        // Increase the current waypoint with wrap-around to zero (or choose a random waypoint)
        if (WayPointNetwork.instance.waypoints.Count > 1)
        {
            // Keep generating random waypoint until we find one that isn't the current one
            // NOTE: Very important that waypoint networks do not only have one waypoint :)
            int oldWaypoint = _currentWaypoint;
            while (_currentWaypoint == oldWaypoint)
            {
                _currentWaypoint = Random.Range(0, WayPointNetwork.instance.waypoints.Count);
            }
        }
        else
            _currentWaypoint = _currentWaypoint == WayPointNetwork.instance.waypoints.Count - 1 ? 0 : _currentWaypoint + 1;
    }

    public void SetShootSpawnersState(bool activated)
    {
        foreach (BaseShootSpawner shooter in shootersList)
            shooter.canShoot = activated;
    }

    public AIState GetStateByType(AIStateType incommingState)
    {
        AIState newState = null;
        if (incommingState != _currentStateType && _states.TryGetValue(incommingState, out newState))
            return newState;
        else
            return null;
    }

    public void SetNewState(AIStateType newStateType)
    {
        ChangeToNewState(newStateType);
    }


}
