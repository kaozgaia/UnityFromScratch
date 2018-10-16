using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBUtils;
using PBShootInterfaces;

public class BossEnemyShooterEntity : ShooterEntity, IEnemyHitableObject, IHitableObject
{
    [Range(0.001f, 0.05f)]
    public float decreaseVelocity = 0.025f;
    [Range(5, 25)]
    public int deathExplosionsNumber = 8;
    public float startTime = 0.5f;
    public AIBossStateMachine enemyStateMachine;
    public AudioClip[] middleExplosions = { };
    public AudioClip deathAudio;

    private GameObject _currentImage;
    private SpriteRenderer _imageRenderer;

    public override void Start()
    {
        base.Start();
        Transform imageTransform = transform.Find("Enemy");
        if (imageTransform)
        {
            _currentImage = imageTransform.gameObject;
            _imageRenderer = _currentImage.GetComponent<SpriteRenderer>();
        }

    }


    public void CharacterOnDestroy()
    {
        
        enemyStateMachine.SetShootSpawnersState(false);
        enemyStateMachine.SetNewState(AIStateType.None);
        StartCoroutine(StartDeath());
        
    }



    public void OnHit()
    {
        StartCoroutine(WhiteStroke());
    }

    // Cooroutines

    private IEnumerator WhiteStroke()
    {
        if (_currentImage)
            _imageRenderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.02f);
        if (_currentImage)
            _imageRenderer.material.SetColor("_Color", Color.white);
    }

    IEnumerator StartDeath()
    {
        yield return StartExplosions();
        yield return DestroyBoss();
    }

    IEnumerator StartExplosions()
    {
        for(int i = 0; i< deathExplosionsNumber; i++)
        {
            if (startTime < 0f) startTime = 0.1f;
            SpawnPosition spawnerPosition = new SpawnPosition();
            spawnerPosition.spawnPoint = transform.position + Random.insideUnitSphere;
            spawnerPosition.spawnPoint.z = transform.position.z - 0.2f;
            spawnerPosition.rotation = transform.rotation;
            
            yield return new WaitForSeconds(startTime);
        }
    }

    IEnumerator DestroyBoss()
    {
        yield return new WaitForSeconds(1f);
        
        SpawnPosition spawnerPosition = new SpawnPosition();
        spawnerPosition.spawnPoint = transform.position;
        spawnerPosition.rotation = transform.rotation;
        
        ReleaseCoins();
        ReleasePowerUp();
        
        Destroy(this.gameObject);
        
    }
}
