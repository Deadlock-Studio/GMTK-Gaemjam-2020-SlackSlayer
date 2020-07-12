using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UnitMovement))]

public class PlayerControl : MonoBehaviour
{
    private float _moveX;
    private float _moveY;
    private Vector2 _mousePos;
    private Vector2 _look;
    private Inventory.Item _selectedItem;




    [SerializeField]
    private PCControl _interactablePC = null;

    [SerializeField]
    private WorkerControl _interactableWorker = null;

    //Components
    private UnitMovement _move;
    private Animator _anim;
    private Inventory _inventory;
    private Rigidbody2D _rb;
    
    public GameObject crosshair;
    public Camera cam;
    public GameObject throwablePrefab;


    // Start is called before the first frame update
    void Start()
    {
        _move = GetComponent<UnitMovement>();
        _anim = GetComponent<Animator>();
        _inventory = GetComponent<Inventory>();
        _rb = GetComponent<Rigidbody2D>();
        _selectedItem = Inventory.Item.NOTHING;
        //hide mouse cursor
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

        //mouse coordinate 
        _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

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

        //select throwable by pressing 1
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            _selectedItem = Inventory.Item.THROWABLE;
        }

        //press mouse to use item after selecting an item
        if (Input.GetMouseButtonDown(0))
        {
            if(_selectedItem != Inventory.Item.NOTHING)
                UseItem(_selectedItem);
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

        MoveCrosshair();
    }

    private void FixedUpdate()
    {
        _move.Walk(_moveX, _moveY);
        _look = _mousePos - _rb.position;
    }

    private void MoveCrosshair() {
        Vector3 aim = new Vector3(_look.x, _look.y, 0.0f);
        crosshair.transform.localPosition = aim;
    }
    private void UseItem(Inventory.Item item) {
        switch (item) {
            case Inventory.Item.GHOST:
                break;
            case Inventory.Item.DUMMY:
                break;
            case Inventory.Item.THROWABLE:
                if (_inventory.GetThrowablesNumber() > 0)
                {
                    GameObject throwable = Instantiate(throwablePrefab, transform.position, Quaternion.identity);
                    //calculate shoot direction from crosshair
                    Vector3 shootDirection = crosshair.transform.localPosition.normalized;
                    throwable.GetComponent<Rigidbody2D>().AddForce(shootDirection * throwable.GetComponent<ThrowableScript>().throwForce, ForceMode2D.Impulse);
                    _inventory.DecreaseItemInInventory(Inventory.Item.THROWABLE);
                }
                break;
            case Inventory.Item.USB:
                break;
            default:
                break;
        }
        
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
