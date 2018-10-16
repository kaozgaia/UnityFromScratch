using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBShootInterfaces;
using PBUtils;

/*
|--------------------------------------------------------------------------
| Base Pool
| Author : Christian A. Fernández
|--------------------------------------------------------------------------

Base Class for have all properties of a proyectile in game
Expose the next properties 
- damage -> how much damage can do this proyectile
- shootedBy -> what kind of character shoot this proyectile
- speed -> movement speed of the proyectile
Make virtual following methods
OnCollisionEnter2D

 */
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ShootBaseEntity : MonoBehaviour, IPoolableObject  {

    public int instances { get { return baseInstances; } }
    public int baseInstances = 50;
    public int damage = 2;
    public CharacterType type = CharacterType.player;
	public float speed = 3.0f;
    public BasePool ownerPool;
	public string shootType{
		get{
			return _shootType;
		}
		set{
			_shootType = value;
		}
	}

    private Rigidbody2D _body;
	protected bool _inPool = true;
    protected int totalDamage = 0;
    private string _shootType = "SimpleShoot";
    protected BaseShootSpawner ownerSpawner = null;

	public virtual void Awake()
	{
		_body = GetComponent<Rigidbody2D>();
		_body.bodyType = RigidbodyType2D.Kinematic;
	}

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
        ShooterEntity shooter = other.gameObject.GetComponent<ShooterEntity>();
        if ((shooter != null && shooter.type != this.type && ownerPool) || other.gameObject.tag == "shootLimit")
        {
            if (shooter != null && shooter.type == CharacterType.npc && this.type == CharacterType.player) return;
            if (ownerSpawner)
            {
                ownerSpawner.DeleteCurrentShoot(this.gameObject);
                ownerSpawner = null;
            }
            ownerPool.RecycleObject(this.gameObject);
        }
		    
				
	}

	public virtual void Update()
	{
		if(_inPool) return;
		transform.position += -transform.up * Time.deltaTime * speed;
	}

    public virtual void SetOwnerSpawner(BaseShootSpawner spawner)
    {
        ownerSpawner = spawner;
    }

	public virtual void PoolAdquire(SpawnPosition spawnerTransform, string objType, BasePool pool)
    {
		_inPool = false;
        ownerPool = pool;
        shootType = objType;
        transform.position = spawnerTransform.spawnPoint;
		transform.rotation = spawnerTransform.rotation;
	}

	public virtual string PoolReset(){
		_inPool = true;
        if(ownerPool)
		    transform.position = ownerPool.transform.position;
        return shootType;	
	}

}
