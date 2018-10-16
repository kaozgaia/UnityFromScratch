using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossState_Moving01 : AIBossState
{
    //PUBLIC
    [Range(0f, 1.0f)]
    [Tooltip("Closing to 0, the shoot is garanted")]
    public float movingShootProbability = 0.2f;
    [Range(0.5f, 6f)]
    public float speed = 1.5f;
    [Range(10f, 120f)]
    public float rotationSpeed = 60f;
    public bool canShootOnMove = false;

    //PRIVATE
    [SerializeField]
    [Range(0.2f, 1f)]
    private float _accuracy = 0.5f;
    private Vector3 _objective;

    public override void OnEnterState()
    {
        
        _enemyStateMachine.SetShootSpawnersState(false);
         
        
        base.OnEnterState();
        Debug.Log("Entering Moving State");
        if (_enemyStateMachine == null)
            return;

        _enemyStateMachine.SetShootSpawnersState(canShootOnMove);

        // Set Objective Position
        _objective = _enemyStateMachine.GetWaypointPosition(false); //Random position

    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Moving;
    }

    public override AIStateType OnUpdate()
    {
        if (_enemyStateMachine == null)
            return AIStateType.None;
        // Logic for stay or change the behaviour AIStateType 



        // Rotate to next point
        if (imageToRotate != null && shouldRotateImage)
        {
            Vector3 lookDirection = _objective - imageToRotate.transform.position;
            Quaternion newRotation = Quaternion.LookRotation(lookDirection, -Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            imageToRotate.transform.rotation = Quaternion.Slerp(imageToRotate.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }

        // Go to next objective
        Vector3 direction = _objective - this.transform.position;

        if (direction.magnitude > _accuracy)
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        else
            return AIStateType.Idle;
        // Return the current state
        return AIStateType.Moving;
    }
}
