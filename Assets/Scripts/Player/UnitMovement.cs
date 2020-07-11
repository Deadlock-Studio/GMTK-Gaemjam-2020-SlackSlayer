using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private float _speed = 5;
    private float _diagSlow = 0.7f;

    public float Speed {
        get { return _speed; }
        set { _speed = value; }
    }

    public void Walk(float moveX, float moveY)
    {
        Vector3 movement = new Vector3(moveX, moveY, 0);
        transform.Translate(movement * _speed * (moveX != 0 && moveY != 0 ? _diagSlow : 1) * Time.deltaTime);
    }
}
