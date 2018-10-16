using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBUtils;

/*
|--------------------------------------------------------------------------
| Base Pool
| Author : Christian A. Fernández
|--------------------------------------------------------------------------

Base Class that will have all the properties and methods for a ShootSpawnPoint
Expose the next properties 
- type -> kind of behaviour of the shoot
- shootTreshold -> time that spawns have to wait to shoot
- _bullet -> shoot prefab
- _canShoot ->  flag that determines if the spawner can instantiate a shoot

Make virtual following methods
OnCollisionEnter2D
OnCollisionExit2D

 */

public enum ShootSpawnerType {forward, follow, rotation, linealTopDown, free };

public abstract class BaseShootSpawner : MonoBehaviour {

    public AudioClip shootClip;
    public ShootSpawnerType type = ShootSpawnerType.forward;
	[Range(0.05f, 6.0f)] public float shootTreshold = 3.0f; 
	[Range(10f, 280f)]public float rotationSpeed = 20f;
	public bool rotateLeft = false;
	public string bulletName = "SimpleShoot";
    public bool rotationAuthority = false;
    public List<GameObject> currentShoots = new List<GameObject>();
    public bool canShoot{
        get
        {
            return _canShoot;
        }
        set
        {
            _timeToWait = 0f;
            _canShoot = value;
        }
    }


    private bool _canShoot = false;
   
    protected float _timeToWait = 3.0f;
    protected bool _bussy = false;

	public virtual void Start()
	{
        _timeToWait = shootTreshold;
	}

	public virtual void Update()
	{
        if (_canShoot)
        {
            DesiredBehaviour();
        }
	}

    protected void DesiredBehaviour(){
        switch (type)
        {
            case ShootSpawnerType.linealTopDown:
                transform.rotation = Quaternion.identity;
                break;
            case ShootSpawnerType.follow:
                break;
            case ShootSpawnerType.forward:
                AimForward();
                break;
            case ShootSpawnerType.rotation:
                int direction = rotateLeft ? -1 : 1;
                transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * direction);
                break;
            case ShootSpawnerType.free:
            default:
                break;
        }
    }

    // Shoot management
    protected virtual void Shoot(){
        GameObject shoot = ShootPool.instance.getObjectOfType(bulletName, Utils.CreateStructFromTransform(transform));
        ShootBaseEntity shootEntity = shoot.GetComponent<ShootBaseEntity>();
        if (shootEntity && !currentShoots.Contains(shoot)) {
            currentShoots.Add(shoot);
            shootEntity.SetOwnerSpawner(this);
        } 
	}

    public void DeleteCurrentShoot(GameObject shoot)
    {
        if (currentShoots.Contains(shoot)) currentShoots.Remove(shoot);
    }

    public void ReleaseAllShoots()
    {
        StartCoroutine(DisableCurrentEntityShoots());
    }

    // Aim Behaviour
    public virtual void AimForward(){
        Quaternion newRotation = transform.parent.rotation;
        if (rotationAuthority)
            newRotation.z = 180f;
        transform.localRotation = newRotation;
    }

    public void RotateBy(float difference){
        transform.Rotate(new Vector3(0, 0, difference));
        
    }

	protected void FollowTarget(Transform target){
		Quaternion newRotation = Quaternion.LookRotation(target.position - transform.position , Vector3.forward);
		newRotation.x = 0.0f;
		newRotation.y = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime*rotationSpeed);
	}


    IEnumerator DisableCurrentEntityShoots()
    {
        if (currentShoots.Count > 0 && ShootPool.instance)
        {
            for (int i = 0; i < currentShoots.Count; i++)
            {
                ShootPool.instance.RecycleObject(currentShoots[i]);
                yield return new WaitForSeconds(0.08f);
            }
            currentShoots = new List<GameObject>();
        }
    }

}
