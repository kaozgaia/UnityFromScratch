using UnityEngine;
using System.Collections;
using PBUtils;

namespace PBShootInterfaces
{

	public interface IPoolableObject
	{
        int instances { get; }
        void PoolAdquire(SpawnPosition spawnerTransform, string objType, BasePool pool);
		string PoolReset();		
	}

    public interface IHitableObject
    {
        void CharacterOnDestroy();
    }

    public interface IEnemyHitableObject
    {
        void OnHit();
    }
}

