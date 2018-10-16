using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossState_Present01 : AIBossState {
    //PUBLIC 
    [Range(0.5f, 6f)]
    public float speed = 1.5f;
    public AudioClip presentAudioDialogue;

    //PRIVATE
    [SerializeField]
    [Range(0.2f, 1f)]
    private float _accuracy = 0.2f;
    private Vector3 _objective;
    private bool _finishPresenting = false;
    private bool _presenting = false;

    public override void OnEnterState()
    {
        
        Debug.Log("Entering Boss Presenting State State");
        if (_enemyStateMachine == null)
            return;
        if (WayPointNetwork.instance)
            _objective = WayPointNetwork.instance.GetRandomTopPoint();
        _enemyStateMachine.SetShootSpawnersState(false);
    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Presenting;
    }

    public override AIStateType OnUpdate()
    {
        if (_enemyStateMachine == null)
            return AIStateType.None;

        // Go to next objective
        Vector3 direction = _objective - this.transform.position;
        
        if (direction.magnitude > _accuracy)
        {
            
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
        else
        {
            if (!_presenting)
            {
                _presenting = true;
                StartCoroutine(MainPresentation());
            }
        }


        // Return the correct state based on sequence
        if (_finishPresenting) return AIStateType.Idle;
        else return AIStateType.Presenting;
    }

    // Coorutines

    IEnumerator MainPresentation()
    {
        yield return PlayAudioPresentation();
        yield return ReleasePlay();
    }

    IEnumerator PlayAudioPresentation()
    {
        yield return new WaitForSeconds(1.5f);
        // BOSS PRESENTATION

        yield return new WaitForSeconds(1f);
    }

    IEnumerator ReleasePlay()
    {
        yield return new WaitForSeconds(1f);
        _finishPresenting = true;
        
        
    }

}
