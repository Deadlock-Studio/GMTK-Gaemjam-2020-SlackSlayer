using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControl : MonoBehaviour
{
    private Queue<GameObject> _deslackQueue = new Queue<GameObject>();

    //Component


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnqueueDeslack(GameObject obj)
    {
        if (obj.CompareTag("Worker"))
            if (obj.GetComponent<WorkerControl>().IsSlacking())
            {
                _deslackQueue.Enqueue(obj);
                Debug.Log(obj.name + " enqueued! Commencing deslacking!");
                //TODO Highlight enqueued workers
            }
    }

    public void Deslack()
    {
        //Is near target
        _deslackQueue.Dequeue();
    }
}
