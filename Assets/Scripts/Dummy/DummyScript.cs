using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private WorkerControl _interactableWorker = null;
    public float _activeTimer;
    private Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        //count down till defloat
        InvokeRepeating("DecreaseActiveTimer", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //if on dummy trigger zone then no slack
        if (_interactableWorker != null)
            if (_interactableWorker.IsSlacking())
                _interactableWorker.Deslack();


        //check the active timer
        if (_activeTimer <= 0) {
            _anim.SetTrigger("deactivate");
            //destroy object after done playing animation
            Destroy(gameObject, _anim.GetCurrentAnimatorStateInfo(0).length);
            
        }  
    }

    private void DecreaseActiveTimer() {
        _activeTimer--;
        Debug.Log(_activeTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Worker")
        {
            _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_interactableWorker)
            if (collision.gameObject == _interactableWorker.gameObject)
                _interactableWorker = null;
    }
}
