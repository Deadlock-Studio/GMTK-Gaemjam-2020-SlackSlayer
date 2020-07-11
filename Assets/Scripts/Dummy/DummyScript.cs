using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private WorkerControl _interactableWorker = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if on dummy trigger zone then no slack
        if (_interactableWorker != null)
            if (_interactableWorker.IsSlacking())
                _interactableWorker.Deslack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Worker")
        {
            _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();
            Debug.Log("worker in trigger zone");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_interactableWorker)
            if (collision.gameObject == _interactableWorker.gameObject)
                _interactableWorker = null;
    }
}
