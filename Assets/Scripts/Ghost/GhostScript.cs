using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    private WorkerControl _interactableWorker = null;
    private Animator _anim;
    private Rigidbody2D _rigidBody2D = null;
    private bool _isMovingLeft = false;
    private int _movingDirection = -1;

    public float _activeTimer;
    public float _speed;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        //count down till defloat
        InvokeRepeating("DecreaseActiveTimer", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //change orientation of ghost sprite
        _anim.SetBool("isMovingLeft", _isMovingLeft);


        //check the active timer, if < 0 then despawn
        if (_activeTimer <= 0)
        {
            _anim.SetTrigger("deactivate");
            //destroy object after done playing animation
            Destroy(gameObject, _anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void FixedUpdate()
    {
        //move
        Vector2 position = _rigidBody2D.position;
        position.x += Time.fixedDeltaTime * _speed * _movingDirection;
        _rigidBody2D.MovePosition(position);
    }

    private void DecreaseActiveTimer()
    {
        _activeTimer--;
        //Debug.Log(_activeTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Worker"))
        {
            _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();
            //if on dummy trigger zone then no slack
            if (_interactableWorker != null)
                if (_interactableWorker.IsSlacking())
                    _interactableWorker.Deslack();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Worker"))
        {
            _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();
            //if on dummy trigger zone then no slack
            if (_interactableWorker != null)
                if (_interactableWorker.IsSlacking())
                    _interactableWorker.Deslack();

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //change the moving direction of ghost if collide with something
        _movingDirection = - _movingDirection;
        _isMovingLeft = !_isMovingLeft;

    }

    public bool IsMovingLeft()
    {
        return _isMovingLeft;
    }
}
