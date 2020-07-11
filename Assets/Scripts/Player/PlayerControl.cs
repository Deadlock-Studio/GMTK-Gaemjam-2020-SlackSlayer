using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UnitMovement))]
public class PlayerControl : MonoBehaviour
{
    private float _moveX;
    private float _moveY;

    [SerializeField]
    private DogControl _dog;
    [SerializeField]
    private WorkerControl _interactableWorker = null;

    //Components
    private UnitMovement _move;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _move = GetComponent<UnitMovement>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Direct meeting
        if (Input.GetKeyDown(KeyCode.E))
        {
            DirectMeeting();
        }

        //Right click to queue for dog to deslack
        if (Input.GetMouseButtonDown(1))
        {
            DogEnqueue();
        }

        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");

        if (!(_moveX == 0 && _moveY == 0))
        {
            _anim.SetFloat("FaceX", _moveX);
            _anim.SetFloat("FaceY", _moveY);
            _anim.SetBool("Walking", true);
        }
        else _anim.SetBool("Walking", false);
    }

    private void FixedUpdate()
    {
        _move.Walk(_moveX, _moveY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Worker"))
            _interactableWorker = collision.transform.GetComponent<WorkerControl>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_interactableWorker)
            if (collision.transform == _interactableWorker.transform)
                _interactableWorker = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Dog"))
        {
            _interactableWorker = collision.transform.parent.GetComponent<WorkerControl>();
            _interactableWorker.Highlight(Color.white, true);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_interactableWorker)
            if (collision.transform.parent == _interactableWorker.transform)
            {
                _interactableWorker.Highlight(false);
                _interactableWorker = null;
            }
    }

    private void DirectMeeting()
    {
        if (_interactableWorker != null)
            if (_interactableWorker.IsSlacking())
                _interactableWorker.Deslack();
    }

    private void DogEnqueue()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Worker"));

        if (hit.collider != null)
        {
            //If hit is a slacking worker
            if (hit.transform.CompareTag("Worker") && hit.transform.GetComponent<WorkerControl>().IsSlacking())
            {
                //Enqueue dog
                _dog.EnqueueDeslack(hit.transform);
            }
        }
    }
}
