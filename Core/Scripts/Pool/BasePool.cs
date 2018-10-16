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
|
| Base class for pooling reusable game objects on the game, it uses a special kind of serializable dictiorary to be friendly in the setup
|
*/

public abstract class BasePool : MonoBehaviour {

    private int lessInstances = 4;

    // Public
    protected Dictionary<string, List<GameObject>> _objectPool;
    

    [SerializeField]
    private StringGameobjectDictionary m_stringGameobjectDictionary;
    public IDictionary<string, GameObject> StringGameObjectDictionary
    {
        get { return m_stringGameobjectDictionary; }
        set { m_stringGameobjectDictionary.CopyFrom(value); }
    }

    public int baseNumberOfItems = 10;


    public virtual void Awake()
    {
        _objectPool = new Dictionary<string, List<GameObject>>();
    }

    public virtual void Start()
    {
        StartPooling();
    }

    protected virtual void StartPooling()
    {
        foreach (var item in StringGameObjectDictionary)
        {
            if (!_objectPool.ContainsKey(item.Key))
            {
                List<GameObject> newList = new List<GameObject>();
                int objectsToCreate;
                GameObject testInstatiated = Instantiate(StringGameObjectDictionary[item.Key], transform.position, transform.rotation);
                IPoolableObject testPoolable = testInstatiated.GetComponent<IPoolableObject>();
                if (testPoolable != null) objectsToCreate = testPoolable.instances;
                else objectsToCreate = baseNumberOfItems;
                DestroyImmediate(testInstatiated.gameObject);
                for (int i = 0; i < objectsToCreate; i++)
                {
                    GameObject instatiated = Instantiate(StringGameObjectDictionary[item.Key], transform.position, transform.rotation);
                    instatiated.transform.SetParent(this.transform);
                    IPoolableObject poolable = instatiated.GetComponent<IPoolableObject>();
                    if (poolable != null)
                        poolable.PoolReset();
                    instatiated.SetActive(false);
                    newList.Add(instatiated);
                }
                _objectPool[item.Key] = newList;
            }
        }
    }

    public virtual GameObject getObjectOfType(string objectType, SpawnPosition spawnerTransform)
    {
        if (StringGameObjectDictionary.ContainsKey(objectType))
        {
            

            List<GameObject> objects = _objectPool[objectType];
            
            if (objects.Count > 0)
            {
                GameObject g = objects[0];
                g.SetActive(true);
                IPoolableObject baseShoot = g.GetComponent<IPoolableObject>();
                if (baseShoot != null) {
                    baseShoot.PoolAdquire(spawnerTransform, objectType, this);
                }
                if (objects.Contains(g)) {
                    objects.Remove(g);
                } 
                return g;
            }
            else {
                
                GameObject instatiated = Instantiate(StringGameObjectDictionary[objectType], transform.position, transform.rotation);
                Debug.Log(string.Format("--- New Instance Created of Type {0} consider to increase the number of base instances", instatiated.name));
                instatiated.transform.SetParent(this.transform);
                IPoolableObject baseShoot = instatiated.GetComponent<IPoolableObject>();
                if(gameObject.name== "EnemyPool")
                {
                    Debug.Log("EnemyPool");
                }
                if(baseShoot !=  null) 
                    baseShoot.PoolAdquire(spawnerTransform, objectType, this);
                
                return instatiated;
            }
        }
        else return null;
    }


    public virtual void RecycleObject(GameObject incommingGameObject){
        IPoolableObject baseShoot = incommingGameObject.GetComponent<IPoolableObject>();
        if(baseShoot !=  null){
            string objectType = baseShoot.PoolReset();
            List<GameObject> objects = _objectPool[objectType];
            
            if (!objects.Contains(incommingGameObject))
            {

                incommingGameObject.SetActive(false);
                objects.Add(incommingGameObject);
            }
                
        }
    }

}
