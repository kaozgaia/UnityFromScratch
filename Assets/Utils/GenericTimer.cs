using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTimer : MonoBehaviour {

    public delegate void OnTimeCycleEnds();

    public event OnTimeCycleEnds OnCycleEnds;

    public delegate void OnTimeWillUpdate(float time);

    public event OnTimeWillUpdate OnTimeUpdate;

    private bool _isActive = false;

    [SerializeField] private float _waitToEvent = 2f;


    [SerializeField] private float _timer = 0f;

    public float WaitToEvent { set { _waitToEvent = value; } }

    #region UNITY_CALLBACKS


    protected virtual void Start()
    {
        _timer = _waitToEvent;
    }

    protected virtual void LateUpdate()
    {
        if (!_isActive)
        {
            return;
        }
        if (_timer <= 0f)
        {
            CycleEnds();
            _timer = _waitToEvent;
        }
        else
        {
            TimeElapsed();
        }
    }

    #endregion

    #region PROTECTED

    protected virtual void CycleEnds()
    {
        if (OnCycleEnds != null)
        {
            OnCycleEnds();
        }
    }

    protected virtual void TimeElapsed()
    {
        _timer -= Time.deltaTime;
        if (_timer >= 0f && OnTimeUpdate != null)
        {
            float minus = _waitToEvent - _timer;
            OnTimeUpdate(Mathf.Clamp(minus / _waitToEvent, 0, _waitToEvent));
        }
    }

    #endregion

    #region PUBLIC METHODS

    public virtual void Invalidate()
    {
        _isActive = false;
    }

    public virtual void RunTimer()
    {
        _timer = _waitToEvent;
        _isActive = true;
    }

    #endregion

}
