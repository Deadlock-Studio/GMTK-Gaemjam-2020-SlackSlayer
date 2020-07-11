using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DogControl : MonoBehaviour
{
    private List<Transform> _deslackQueue = new List<Transform>();

    //Component
    [SerializeField]
    private AIDestinationSetter _destination;
    private WorkerControl _slack = null;

    // Start is called before the first frame update
    void Start()
    {
        _slack = GetComponent<WorkerControl>();
        _slack.Deslack();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_slack.IsSlacking())
            enforce();
    }

    public void EnqueueDeslack(Transform transform)
    {
        if (transform.CompareTag("Worker"))
            if (transform.GetComponent<WorkerControl>().IsSlacking())
            {
                _deslackQueue.Add(transform);
                transform.GetComponent<WorkerControl>().Highlight(Color.magenta, true);
            }
    }

    public void Dequeue(int i)
    {
        _deslackQueue[i].GetComponent<WorkerControl>().Highlight(false);
        _deslackQueue.RemoveAt(i);
    }

    public void Dequeue()
    {
        _deslackQueue[0].GetComponent<WorkerControl>().Highlight(false);
        _deslackQueue.RemoveAt(0);
    }

    private void enforce()
    {
        //Dequeue people who has been deslacked since
        for (int i = _deslackQueue.Count - 1; i >= 0; i--)
        {
            if (!_deslackQueue[i].GetComponent<WorkerControl>().IsSlacking())
            {
                Dequeue(i);
            }
        }

        //Go deslack remaining slackers
        if (_deslackQueue.Count > 0)
        {
            Transform target = _deslackQueue.First();
            goToTarget(target);
            //TODO Check for contact before deslack
            if (Vector3.Distance(transform.position, target.transform.position) <= 3)
            {
                target.GetComponent<WorkerControl>().Deslack();
                Dequeue();
            }
        }
    }

    private void goToTarget(Transform _transform)
    {
        _destination.target = _transform;
    }
}
