using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(Animator))]
public class WorkerControl : MonoBehaviour
{
    private bool _isSlacking = true;
    private bool _slackable = true;

    //GameObjects
    [SerializeField]
    private GameObject _border = null;

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
}
