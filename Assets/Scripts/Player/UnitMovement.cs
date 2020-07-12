using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private float _speed = 6;
    private float _diagSlow = 0.7f;

    //Component
    private Rigidbody2D _rb;

    public float Speed {
        get { return _speed; }
        set { _speed = value; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Walk(float moveX, float moveY)
    {
        Vector2 movement = new Vector2(moveX, moveY);
        _rb.MovePosition(_rb.position + movement * (moveX != 0 && moveY != 0 ? _diagSlow : 1) * _speed * Time.fixedDeltaTime);

        //Vector3 movement = new Vector3(moveX, moveY, 0);
        //transform.Translate(movement * _speed * (moveX != 0 && moveY != 0 ? _diagSlow : 1) * Time.deltaTime);
    }
}
