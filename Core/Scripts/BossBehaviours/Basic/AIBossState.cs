using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBossState : AIState {
    
    // Private

    public GameObject imageToRotate;
    public bool shouldRotateImage = true;

    protected AIBossStateMachine _enemyStateMachine = null;

    // -------------------------------------------------------------------------------------
    // Name	:	SetStateMachine
    // Desc	:	Check for type compliance and store reference as derived type
    // -------------------------------------------------------------------------------------
    public override void SetStateMachine(AIStateMachine stateMachine)
    {
        if (stateMachine.GetType() == typeof(AIBossStateMachine))
        {
            base.SetStateMachine(stateMachine);
            _enemyStateMachine = (AIBossStateMachine)stateMachine;

        }
    }
}
