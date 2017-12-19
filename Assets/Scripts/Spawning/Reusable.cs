using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reusable : MonoBehaviour
{

    //So with this way of doing it, the objects themselves take care of going back to pool
    //And pool's only job is to aquire and attach event handler that will send them back to pool
    //
    public event EditReusable backToPool;
    protected int poolID;
    
    public int ReuseID
    {
        get
        {
            return poolID;
        }
    }

}
public delegate void EditReusable();


