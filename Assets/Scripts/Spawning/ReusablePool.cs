using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pool doesn't need to be a monobehaviour, spawners use pools and they will
//be monos
public class ReusablePool<T> where T : Reusable, new() {

    private Stack<T> objectPool;
    private static readonly int maxBuffer = 100;
    int total;
    //Won't be singleton, there will be different instances per type
    public ReusablePool(int size)
    {
        for (int i = 0; i < size; ++i)
        {
            objectPool.Push(new T());
        }

        //Even if aquire this still total size of pool
        total = size;

    }

    public T Acquire()
    {
        if (objectPool.Count > 0)
        {
            T pooledObj = objectPool.Peek();
            objectPool.Pop();
            pooledObj.backToPool += () => {Release(pooledObj); };
            return pooledObj;
        }
        else
        {
            if (total < maxBuffer)
            {
                total += 1;
                T poolAddition = new T();
                poolAddition.backToPool += () => { Release(poolAddition); };
                return poolAddition;
            }
            else
            {
                //If greater than buffer then don't back to pool jst add new instance or can return null to say that need to wait
                //return new T();
                return null;
            }
            
        }

    }
    
    //Private as only this class calls it with added event handler to backToPool Event
    private void Release(T obj)
    {

        objectPool.Push(obj);

    }
}
