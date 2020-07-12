using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(Animator))]
public class WorkerControl : MonoBehaviour
{
    [SerializeField]
    private bool _isSlacking = false;
    private bool _isHacked = false;
    private bool _slackable = true;

    //GameObjects
    [SerializeField]
    private GameObject _border = null;
    [SerializeField]
    private GameObject _hacked = null;

    //Components
    private Animator _anim;

    public bool IsSlacking()
    {
        return _isSlacking;
    }

    public bool IsHacked()
    {
        return _isHacked;
    }

    public bool Slackable {
        get { return _slackable; }
        set { _slackable = value; }
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("isSlacking", _isSlacking);
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

    public void Highlight(Color color, bool active = true)
    {
        _border.SetActive(active);
        _border.GetComponent<SpriteRenderer>().color = color;
    }

    public void Highlight(bool active)
    {
        _border.SetActive(active);
    }

    public void Hack()
    {
        _isHacked = true;
        _hacked.SetActive(true);
    }

    public void Unhack()
    {
        _isHacked = false;
        _hacked.SetActive(false);
    }
}
