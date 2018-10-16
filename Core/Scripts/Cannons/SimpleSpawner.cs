using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : BaseShootSpawner {

    public override void Update(){
        base.Update();
        _timeToWait -= Time.deltaTime;
        if (_timeToWait <= -10f)
            _timeToWait = 0f;
        if (_timeToWait <= 0f && canShoot)
        {

            _timeToWait = shootTreshold;
            // Ask for a pool object of type _bullet
            Shoot();
        }
    }

}
