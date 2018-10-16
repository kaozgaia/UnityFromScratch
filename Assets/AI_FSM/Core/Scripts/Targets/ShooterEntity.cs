using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBShootInterfaces;
using PBUtils;

/*
|--------------------------------------------------------------------------
| Shooter Entity
| Author : Christian A. Fernández
|--------------------------------------------------------------------------

Base Class for have all properties and behaviours of a target in game that can be hittable


 */

public enum CharacterType {player, npc, enemy};
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ShooterEntity : MonoBehaviour {
    // public
    public int totalHealth;
	public CharacterType type;
    public AudioClip[] HitAudios;
    public AudioClip destroyAudio;
    public float hitVolume = 0.6f;
    public int health { get { return currentHealth; } }
    public int coinsDropMaxNumber = 5;
    public int scorePerEnemy = 50;
    public int damageByCollision = 4;
    public string rewardPowerupName = "";
    // Private
    private Rigidbody2D _body;
    private int currentHealth;
    [SerializeField]private HPBar _bar;

    protected IHitableObject hitableInterface;
    protected IEnemyHitableObject enemyInterface;
    protected IPoolableObject enemyPoolable;

    public virtual void Awake()
	{
		_body = GetComponent<Rigidbody2D>();
		_body.bodyType = RigidbodyType2D.Kinematic;
        
	}

    public virtual void Start() {
        Prepare();
        hitableInterface = GetComponent<IHitableObject>();
        enemyInterface = GetComponent<IEnemyHitableObject>();
    }

    public virtual void Prepare()
    {
        currentHealth = totalHealth;
        if(_bar) _bar.ResetBar();
    }

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
        if (currentHealth <= 0) return;
		ShootBaseEntity shootEntity = other.gameObject.GetComponent<ShootBaseEntity>();
        ShooterEntity otherShooter = other.gameObject.GetComponent<ShooterEntity>();
        if (other.gameObject.tag == "shoot" && shootEntity != null)
        {
            switch (this.type)
            {
                case CharacterType.npc:
                case CharacterType.player:
                    if (shootEntity.type == CharacterType.enemy)
                    {
                        // NPC BEHAVIOUR
                    }
                               
                    break;
                case CharacterType.enemy:
                    if (shootEntity.type == CharacterType.npc || shootEntity.type == CharacterType.player)
                    {
                        // Critical
                        float baseDamage = shootEntity.damage;
                        bool isCritical = false;
                        if (Utils.GetRand01() < 0.09f)
                        {
                            baseDamage *= 1.8f;
                            isCritical = true;
                        }
                        // Health reduction
                        currentHealth -= (int)Mathf.Ceil(baseDamage);
                        PlayDamageHit();
                        
                        
                        if (enemyInterface != null)
                            enemyInterface.OnHit();
                        if (_bar)
                            _bar.ReduceHealthBar(currentHealth, totalHealth);
                    }
                        
                    break;
                default:
                    break;
            }
            
                
        } else if (otherShooter != null && otherShooter.type == CharacterType.enemy && (this.type == CharacterType.player || this.type == CharacterType.npc))
        {
            if(otherShooter.damageByCollision > 0)
            {
                currentHealth -= otherShooter.damageByCollision;
                PlayDamageHit();
                if (enemyInterface != null)
                    enemyInterface.OnHit();
                if (_bar)
                    _bar.ReduceHealthBar(currentHealth, totalHealth);
            }
            
        }
        if (currentHealth <= 0 && hitableInterface != null)
            EntityDestroy();

    }

    protected virtual void EntityDestroy()
    {
        // AUDIO HIT

        // ON DESTROY
        if(hitableInterface != null) hitableInterface.CharacterOnDestroy();
    }

    protected virtual void ReleasePowerUp()
    {
        // REWARDS
    }

    protected virtual void ReleaseCoins()
    {
        // REWARD USER WITH COINS
    }

    protected virtual void PlayDamageHit()
    {
        // VISUAL DAMAGE
    }

    protected virtual void TryShowDamage(GameObject damageText, int damage, bool isCritial)
    {
       // VISUAL CALCULATED DAMAGE
    }


    // CURRENT SHOOTED OBJ MANAGEMENT WHEN DESTROY
    public virtual void DisableEntityShoots() { }
	
}
