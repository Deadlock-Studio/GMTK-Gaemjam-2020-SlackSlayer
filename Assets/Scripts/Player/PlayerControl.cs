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
    private DogControl _dog = null;
	
    [SerializeField]
    private WorkerControl _interactableWorker = null;
    [SerializeField]
    private PCControl _interactablePC = null;

    //Components
    private UnitMovement _move;
    private Animator _anim;
    private Inventory _inventory;
    private Rigidbody2D _rb;
    
    public GameObject crosshair;
    public Camera cam;
    public GameObject throwablePrefab;


    void Awake()
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
	        DirectMeeting();

            if (_interactablePC != null)
            {
                if (!_interactablePC.IsHacked())
                    _interactablePC.Hacked();
            }
        }

        //select throwable by pressing 1
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            if (_inventory.GetThrowablesNumber() > 0)
            {
                if (_selectedItem == Inventory.Item.THROWABLE)
                {
                    GUIElements.ToggleActive(0, false);
                    _selectedItem = Inventory.Item.NOTHING;
                }
                else
                {
                    GUIElements.ToggleActive(0, true);
                    _selectedItem = Inventory.Item.THROWABLE;
                }
            }   
        }

        //select usb by pressing 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_selectedItem == Inventory.Item.USB)
            {
                GUIElements.ToggleActive(1, false);
                _selectedItem = Inventory.Item.NOTHING;
            }
            else
            {
                GUIElements.ToggleActive(1, true);
                _selectedItem = Inventory.Item.USB;
            }
        }

        //press mouse to use item after selecting an item
        if (Input.GetMouseButtonDown(0))
        {
            if(_selectedItem != Inventory.Item.NOTHING)
                UseItem(_selectedItem);
        }

	//Right click to queue for dog to deslack
        if (Input.GetMouseButtonDown(1))
        {
            if (_dog) DogEnqueue();
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

                    _selectedItem = Inventory.Item.NOTHING;
                    GUIElements.ToggleActive(0, false);
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

            case "Dog":
                _interactableWorker = collision.transform.GetComponent<WorkerControl>();
                _interactableWorker.Highlight(Color.white, true);
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

            case "Dog":
                if (_interactableWorker)
                    if (collision.transform == _interactableWorker.transform)
                    {
                        _interactableWorker.Highlight(false);
                        _interactableWorker = null;
                    }
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
