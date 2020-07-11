using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UnitMovement))]

public class PlayerControl : MonoBehaviour
{
    private float _moveX;
    private float _moveY;

    [SerializeField]
    private PCControl _interactablePC = null;

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
        //hack pc
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_interactableWorker != null)
                if (_interactableWorker.IsSlacking())
                    _interactableWorker.Deslack();

            if (_interactablePC != null)
            {
                if (!_interactablePC.IsHacked())
                    _interactablePC.Hacked();
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Worker":
                _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();
                break;

            case "PC":
                _interactablePC = collision.gameObject.GetComponent<PCControl>();
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Worker":
                if (_interactableWorker)
                    if (collision.gameObject == _interactableWorker.gameObject)
                        _interactableWorker = null;
                break;

            case "PC":
                if (_interactablePC)
                    if (collision.gameObject == _interactablePC.gameObject)
                        _interactablePC = null;
                break;

            default:
                break;
        }
    }
}
