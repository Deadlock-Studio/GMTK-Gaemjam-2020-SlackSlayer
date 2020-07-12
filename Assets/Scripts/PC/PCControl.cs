using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControl : MonoBehaviour
{
    List<WorkerControl> _hackList = new List<WorkerControl>();

    [SerializeField]
    private GameObject _border = null;

    //Component

    void Awake()
    {
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
