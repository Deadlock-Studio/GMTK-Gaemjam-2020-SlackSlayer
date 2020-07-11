using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControl : MonoBehaviour
{
    private bool _isHacked = false;
    private Animator _anim;

    public bool IsHacked()
    {
        return _isHacked;
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("isHacked", _isHacked);
    }

    public void Hacked() {
        _isHacked = true;
        _anim.SetBool("isHacked", true);
    }
}
