using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossState_Idle01 : AIBossState
{

    // UNITY STUFF --------------------------------------

    [SerializeField]
    Vector2 _idleTimeRange = new Vector2(10.0f, 60.0f);

    // Private
    float _idleTime = 0.0f;
    float _timer = 0.0f;



    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Entering Idle State");
        if (_enemyStateMachine == null)
            return;

        if (imageToRotate != null && shouldRotateImage)
        {
            Quaternion newRotation = Quaternion.identity;
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            newRotation.z = 180f;
            imageToRotate.transform.rotation = newRotation;
        }

        _enemyStateMachine.SetShootSpawnersState(true);

        // Set Idle Time
        _idleTime = Random.Range(_idleTimeRange.x, _idleTimeRange.y);
        _timer = 0.0f;
    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Idle;
    }

    public override AIStateType OnUpdate()
    {
        if (_enemyStateMachine == null)
            return AIStateType.None;
        // Logic for stay or change the behaviour AIStateType 
        // Update the idle timer
        _timer += Time.deltaTime;

        // Patrol if idle time has been exceeded
        if (_timer > _idleTime)
        {
            return AIStateType.Moving;
        }

        // Return the current state
        return AIStateType.Idle;
    }
}
