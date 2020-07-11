using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DogControl : MonoBehaviour
{
    private float _moveX;
    private float _moveY;
    private List<Transform> _deslackQueue = new List<Transform>();

    //Component
    private WorkerControl _slack;
    private UnitMovement _move;

    // Start is called before the first frame update
    void Start()
    {
        _move = GetComponent<UnitMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_slack.IsSlacking())
            enforce();
    }

    private void FixedUpdate()
    {
        _move.Walk(_moveX, _moveY);
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
            if (Vector3.Distance(transform.position, target.transform.position) <= 1.5)
            {
                target.GetComponent<WorkerControl>().Deslack();
                Dequeue();
            }
        }
        else
        {
            _moveX = 0;
            _moveY = 0;
        }
    }

    private void goToTarget(Transform _transform)
    {
        Vector3 movement = _transform.position - transform.position;
        movement = movement.normalized;
        _moveX = movement.x;
        _moveY = movement.y;
    }
}
