using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControl : MonoBehaviour
{
    List<WorkerControl> _hackList = new List<WorkerControl>();

    //Component
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void InitiateHacking()
    {
        foreach (WorkerControl worker in _hackList)
        {
            worker.Deslack();
        }
    }

    public void AddToHackList(WorkerControl worker)
    {
        _hackList.Add(worker);
        worker.Hack();
    }
}
