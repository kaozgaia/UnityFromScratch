using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

// Public Enums of the AI System
public enum AIStateType 		{ None, Presenting, Idle, Fordward, Moving, TopDown, Following, PathFollower }
public enum AITriggerEventType	{ Enter, Stay, Exit }



// ----------------------------------------------------------------------
// Class	:	AIStateMachine
// Desc		:	Base class for all AI State Machines
// ----------------------------------------------------------------------
public abstract class AIStateMachine : MonoBehaviour 
{
    private bool stopAllStates = false;

	// Protected
	protected AIState			                _currentState		=	null;
	protected Dictionary< AIStateType, AIState> _states	            =	new Dictionary< AIStateType, AIState>();

	// Protected Inspector Assigned
	[SerializeField]	protected AIStateType	_currentStateType	=	AIStateType.Idle;
	[SerializeField]	[Range(0,15)]		    protected float			_stoppingDistance	=	1.0f;


	// Component Cache
	protected Animator		_animator		=	null;
	protected Transform		_transform		=	null;
    

	// Public Properties
	public Animator 		animator 		{ get{ return _animator; }}
    
	// -----------------------------------------------------------------
	// Name	:	Awake
	// Desc	:	Cache Components
	// -----------------------------------------------------------------
	protected virtual void Awake()
	{
		// Cache all frequently accessed components
		_transform	=	transform;
		_animator	=	GetComponent<Animator>();
	}

	// -----------------------------------------------------------------
	// Name	:	Start
	// Desc	:	Called by Unity prior to first update to setup the object
	// -----------------------------------------------------------------
	protected virtual void Start()
	{
		
		// Fetch all states on this game object
		AIState[] states = GetComponents<AIState>();

		// Loop through all states and add them to the state dictionary
		foreach( AIState state in states )
		{
			if (state!=null && !_states.ContainsKey(state.GetStateType()))
			{
				// Add this state to the state dictionary
				_states[state.GetStateType()] = state;

				// And set the parent state machine of this state
				state.SetStateMachine(this);
			}
		}

		// Set the current state
		if (_states.ContainsKey( _currentStateType ))
		{
			_currentState = _states[_currentStateType];
			_currentState.OnEnterState();
		}
		else
		{
			_currentState =	null;
		}

	}

	

	
	

	// -------------------------------------------------------------------
	// Name :	Update
	// Desc	:	Called by Unity each frame. Gives the current state a
	//			chance to update itself and perform transitions.
	// -------------------------------------------------------------------
	protected virtual void Update()
	{
        
		if (_currentState==null || stopAllStates) return;

		AIStateType newStateType = _currentState.OnUpdate();
        ChangeToNewState(newStateType);
	}

    protected void ChangeToNewState(AIStateType newStateType) {
        if (newStateType != _currentStateType)
        {
            AIState newState = null;
            if (_states.TryGetValue(newStateType, out newState))
            {
                stopAllStates = false;
                _currentState.OnExitState();
                newState.OnEnterState();
                _currentState = newState;
            }
            else if (newStateType == AIStateType.None)
            {
                stopAllStates = true;
            }
            else if (_states.TryGetValue(AIStateType.Idle, out newState))
            {
                _currentState.OnExitState();
                newState.OnEnterState();
                _currentState = newState;
            }

            _currentStateType = newStateType;
        }
    }
	
}
