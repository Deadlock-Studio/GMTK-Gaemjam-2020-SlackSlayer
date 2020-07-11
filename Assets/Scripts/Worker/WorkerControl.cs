using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(Animator))]
public class WorkerControl : MonoBehaviour
{
    private bool _isSlacking = true;
    private bool _slackable = true;

    //Components
    private Animator _anim;

    public bool IsSlacking()
    {
        return _isSlacking;
    }

    public bool Slackable {
        get { return _slackable; }
        set { _slackable = value; }
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("isSlacking", true);
    }

    public void Slack()
    {
        _isSlacking = true;
        _anim.SetBool("isSlacking", true);
    }

    public void Deslack()
    {
        _isSlacking = false;
        _anim.SetBool("isSlacking", false);
    }
}
