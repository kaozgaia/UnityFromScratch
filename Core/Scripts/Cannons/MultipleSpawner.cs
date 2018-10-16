using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpawner : BaseShootSpawner {

    public float apertureAngle = 45f;
    [Range(1f, 50f)]public int numberOfIterations = 3;
    [Range(1f, 10f)]public int numberOfShoots = 3;
    [Range(0.1f, 1f)]public float waitBetweenShoots = 0.5f;



    public override void Update(){
        if (_bussy) return;

        base.Update();
        _timeToWait -= Time.deltaTime;
        if (_timeToWait <= -10f)
            _timeToWait = 0f;
        if (_timeToWait <= 0f && canShoot)
        {
            _timeToWait = shootTreshold;
            // Ask for a pool object of type _bullet
            StartCoroutine(MultipleShoot());
        }
    }

    IEnumerator MultipleShoot(){
        _bussy = true;
        if(numberOfShoots < 2){
            DesiredBehaviour();
            for (int j = 0; j < numberOfShoots; j++) Shoot();
            yield return new WaitForSeconds(waitBetweenShoots);
        } else{
            float angleStep = (apertureAngle / (numberOfShoots-1));
            float startAngle = apertureAngle / 2f;
            for (int i = 1; i <= numberOfIterations; i++)
            {
                // Move direction to initial angle
                DesiredBehaviour();
                RotateBy(-startAngle);
                for (int j = 1; j <= numberOfShoots; j++)
                {
                    Shoot();
                    RotateBy(angleStep);
                }
                yield return new WaitForSeconds(waitBetweenShoots);
            }    
        }

        _bussy = false;
    }
}
