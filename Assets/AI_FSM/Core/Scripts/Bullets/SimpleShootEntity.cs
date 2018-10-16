using System.Collections;
using System.Collections.Generic;
using PBUtils;
using UnityEngine;

public class SimpleShootEntity : ShootBaseEntity {

    private TrailRenderer[] trails;

    public override void Awake()
    {
        base.Awake();
        trails = GetComponentsInChildren<TrailRenderer>();
    }

    public override void PoolAdquire(SpawnPosition spawnerTransform, string objType, BasePool pool)
    {
        base.PoolAdquire(spawnerTransform, objType, pool);
        if(trails.Length > 0)
        {
            foreach (TrailRenderer t in trails) t.enabled = true;
        }
    }

    public override string PoolReset()
    {
        if (trails.Length > 0)
        {
            foreach (TrailRenderer t in trails) t.enabled = false;
        }
        return base.PoolReset();
    }

}
