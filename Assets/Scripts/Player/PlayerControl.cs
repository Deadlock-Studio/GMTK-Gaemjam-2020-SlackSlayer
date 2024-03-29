﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using TMPro;
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
    private bool _control = true;

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
    [SerializeField]
    private bool _nearPC = false;

    //Components
    private UnitMovement _move;
    private Animator _anim;
    private Inventory _inventory;
    private Rigidbody2D _rb;
    
    public GameObject crosshair;
    public Camera cam;
    public GameObject throwablePrefab;
    public GameObject DummyPrefab;
    public GameObject GhostPrefab;


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
        if (Input.GetKeyDown(KeyCode.E) && _control)
        {
	        DirectMeeting();

            if (_nearPC) InitiateHacking();
        }

        //select throwable by pressing 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && _control)
        {
            if (_inventory.GetThrowableNumber() > 0)
            {
                if (_selectedItem == Inventory.Item.THROWABLE)
                {
                    DeselectConsumable(0);
                }
                else
                {
                    GUIElements.ToggleActive(0, true);
                    _selectedItem = Inventory.Item.THROWABLE;
                }
            }   
        }

        //select usb by pressing 2
        if (Input.GetKeyDown(KeyCode.Alpha2) && _control)
        {
            if (_inventory.GetUsbNumber() > 0)
            {
                if (_selectedItem == Inventory.Item.USB)
                {
                    DeselectConsumable(1);
                }
                else
                {
                    GUIElements.ToggleActive(1, true);
                    _selectedItem = Inventory.Item.USB;
                }
            }
        }

        //select dummy by pressing 3
        if (Input.GetKeyDown(KeyCode.Alpha3) && _control)
        {
            if (_inventory.GetDummyNumber() > 0)
            {
                if (_selectedItem == Inventory.Item.DUMMY)
                {
                    DeselectConsumable(2);
                }
                else
                {
                    GUIElements.ToggleActive(2, true);
                    _selectedItem = Inventory.Item.DUMMY;
                }
            }
        }

        //select ghost by pressing 3
        if (Input.GetKeyDown(KeyCode.Alpha4) && _control)
        {
            if (_inventory.GetGhostNumber() > 0)
            {
                if (_selectedItem == Inventory.Item.GHOST)
                {
                    DeselectConsumable(3);
                }
                else
                {
                    GUIElements.ToggleActive(3, true);
                    _selectedItem = Inventory.Item.GHOST;
                }
            }
        }

        //press mouse to use item after selecting an item
        if (Input.GetMouseButtonDown(0) && _control)
        {
            if(_selectedItem != Inventory.Item.NOTHING)
                UseItem(_selectedItem);
        }

	    //Right click to queue for dog to deslack
        if (Input.GetMouseButtonDown(1) && _control)
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
        if (_control)
        {
            _move.Walk(_moveX, _moveY);
        }
        
        _look = _mousePos - _rb.position;
    }

    private void DeselectConsumable(int i)
    {
        GUIElements.ToggleActive(i, false);
        _selectedItem = Inventory.Item.NOTHING;
    }

    private void MoveCrosshair() {
        Vector3 aim = new Vector3(_look.x, _look.y, 0.0f);
        crosshair.transform.localPosition = aim;
    }
    private void UseItem(Inventory.Item item) {
        RaycastHit2D insideLevel = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("LevelArea"));
        switch (item) {
            case Inventory.Item.GHOST:
                if (insideLevel.collider != null)
                {
                    _inventory.DecreaseItemInInventory(Inventory.Item.GHOST);
                    GameObject ghost = Instantiate(GhostPrefab, crosshair.transform.position, Quaternion.identity);
                    _selectedItem = Inventory.Item.NOTHING;
                    GUIElements.ToggleActive(3, false);
                }
                break;
            case Inventory.Item.DUMMY:
                if (insideLevel.collider != null) {
                    _inventory.DecreaseItemInInventory(Inventory.Item.DUMMY);
                    GameObject dummy = Instantiate(DummyPrefab, crosshair.transform.position, Quaternion.identity);
                    _selectedItem = Inventory.Item.NOTHING;
                    GUIElements.ToggleActive(2, false);
                }
                break;
            case Inventory.Item.THROWABLE:
                if (_inventory.GetThrowableNumber() > 0)
                {
                    _inventory.DecreaseItemInInventory(Inventory.Item.THROWABLE);
                    GameObject throwable = Instantiate(throwablePrefab, transform.position, Quaternion.identity);
                    //calculate shoot direction from crosshair
                    Vector3 shootDirection = crosshair.transform.localPosition.normalized;
                    throwable.GetComponent<Rigidbody2D>().AddForce(shootDirection * throwable.GetComponent<ThrowableScript>().throwForce, ForceMode2D.Impulse);
                    _selectedItem = Inventory.Item.NOTHING;
                    GUIElements.ToggleActive(0, false);
                }
                break;
            case Inventory.Item.USB:
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Worker"));
                RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Dog"));

                if (hit.collider != null)
                {
                    //If hit is a worker
                    if (hit.transform.CompareTag("Worker"))
                    {
                        if (!hit.transform.GetComponent<WorkerControl>().IsHacked())
                        {
                            //Create backdoor
                            _interactablePC.AddToHackList(hit.transform.GetComponent<WorkerControl>());
                            _inventory.DecreaseItemInInventory(Inventory.Item.USB);
                        }
                        else
                        {
                            //TODO "This target has already been hacked"
                        }
                        
                        _selectedItem = Inventory.Item.NOTHING;
                        GUIElements.ToggleActive(1, false);
                    }
                }

                if (hit2.collider != null)
                {
                    //If hit is a worker
                    if (hit2.transform.CompareTag("Dog"))
                    {
                        if (!hit2.transform.GetComponent<WorkerControl>().IsHacked())
                        {
                            //Create backdoor
                            _interactablePC.AddToHackList(hit2.transform.GetComponent<WorkerControl>());
                            _inventory.DecreaseItemInInventory(Inventory.Item.USB);
                        }
                        else
                        {
                            //TODO "This target has already been hacked"
                        }

                        _selectedItem = Inventory.Item.NOTHING;
                        GUIElements.ToggleActive(1, false);
                    }
                }
                break;
            default:
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Worker":
            case "Dog":
                if (_interactableWorker)
                {
                    _interactableWorker.Highlight(false);
                }
                _interactableWorker = collision.transform.GetComponent<WorkerControl>();
                _interactableWorker.Highlight(true);
                break;

            case "PC":
                _nearPC = true;
                _interactablePC = collision.transform.GetComponent<PCControl>();
                _interactablePC.Highlight(true);
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
            case "Dog":
                if (_interactableWorker)
                    if (collision.transform == _interactableWorker.transform)
                    {
                        _interactableWorker.Highlight(false);
                        _interactableWorker = null;
                    }
                break;

            case "PC":
                _nearPC = false;
                if (_interactablePC) {
                    if (collision.transform == _interactablePC.transform) {
                        _interactablePC.Highlight(false);
                        _interactablePC = null;
                    }
                }

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

    private void InitiateHacking()
    {
        if (_interactablePC != null)
        {
            _interactablePC.InitiateHacking();
        }
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

    public void SetControl(bool active)
    {
        _control = active;
    }
}
