using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTimer : GenericTimer {

    // Public Events
    public delegate void OnTimeCycleEnd(int countdown, bool finish);
    public event OnTimeCycleEnd onCycleFinish;
    
    //Public 
    

    //Inspector Assigned
    [SerializeField] private int _loopTimes = 5;
    [SerializeField] bool backwards = false;

    //Private
    [SerializeField]
    private int _currentCycles = 0;

    #region PRIVATE

    private void OnCycleHasEnded(bool hasEnded)
    {
        if(onCycleFinish != null)
        {
            onCycleFinish(_currentCycles, hasEnded);
        }
        if (hasEnded)
        {
            Invalidate();
        }
    }

    #endregion

    #region PUBLIC

    protected override void CycleEnds()
    {
        _currentCycles = backwards ? --_currentCycles : ++_currentCycles;
        if(backwards && _currentCycles == 0)
        {
            OnCycleHasEnded(true);
        } else if (!backwards && _currentCycles == _loopTimes)
        {
            OnCycleHasEnded(true);
        }
        else
        {
            OnCycleHasEnded(false);
        }
    }

    #endregion

    public override void RunTimer()
    {
        _currentCycles = backwards ? _loopTimes : 0;
        base.RunTimer();
    }

}
