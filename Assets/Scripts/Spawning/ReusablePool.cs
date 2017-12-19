using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pool doesn't need to be a monobehaviour, spawners use pools and they will
//be monos
//I could just pool the model, but that would mean have to make model for everything and just is bad.
//also would mean sitll need to instantite game Objects which defeats purpose
public class PoolManager : MonoBehaviour {

    //Read dictionary idea where just have pools for different types organized by id
    //Better than template idea had, since this is UNity not normal c# and can't instantiate MonoBehaviours
    private Dictionary<int, Queue<Reusable>> objectPools;

    //To make it a singleton
    private static PoolManager instance;


    public static PoolManager Instance
    {
        get{
        
            if (instance == null)
            {
                instance = new PoolManager();
            }
            return instance;
        }

    }

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

    }
   

    public void AddPool(int id, Reusable prefab, int buffer)
    {

        objectPools[id] = new Queue<Reusable>();

        for (int i = 0; i < buffer; ++i)
        {
            objectPools[id].Enqueue(Instantiate(prefab));
        }

    }

    public Reusable Acquire(int poolID)
    {
        if (objectPool[poolID].Count > 0)
        {
            T pooledObj = objectPool[poolID].Peek();
            objectPool[poolID].Dequeue();
            pooledObj.backToPool += () => {Release(pooledObj); };
            return pooledObj;
        }

        return null;
       
    }
    
    //Private as only this class calls it with added event handler to backToPool Event
    private void Release(int id,T obj)
    {

        objectPools[id].Enqueue(obj);

    }
}
