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
        objectPools = new Dictionary<int, Queue<Reusable>>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

    }
   

    //Same here
    public void AddPool(Reusable prefab, int buffer)
    {
       

       
        if (objectPools.ContainsKey(prefab.ReuseID))
        {
            Debug.Log("Already there");
            return;
        }

        objectPools[prefab.ReuseID] = new Queue<Reusable>();

        for (int i = 0; i < buffer; ++i)
        {
            Reusable created = Instantiate(prefab);
            created.gameObject.SetActive(false);
            created.transform.parent = this.transform;
            objectPools[prefab.ReuseID].Enqueue(created);
        }
     
    }

    //This is fine since only passind id
    public Reusable Acquire(int poolID)
    {



        if (objectPools.ContainsKey(poolID))
        {
            if (objectPools[poolID].Count > 0)
            {
                Reusable pooledObj = objectPools[poolID].Peek();
                objectPools[poolID].Dequeue();
                pooledObj.ReuseID = poolID;
                pooledObj.backToPool += () => { Release(pooledObj); };
                return pooledObj;
            }
        }


        return null;
    }   
    
    
  

    //Private as only this class calls it with added event handler to backToPool Event
    //Can derive id from obj, so wtf was i doin here.
    private void Release(Reusable obj)
    {
        obj.gameObject.SetActive(false);
        objectPools[obj.ReuseID].Enqueue(obj);


    }
}
