using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableScript : MonoBehaviour
{
    public float throwForce = 10.0f;

    [SerializeField]
    private WorkerControl _interactableWorker = null;
    private Rigidbody2D _rb = null;
    private int bounds;
    public GameObject throwablePickupPrefab;
    public int numberOfBound;



    private void Start()
    {
        bounds = 0;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (bounds >= numberOfBound)
        {
            Instantiate(throwablePickupPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Worker":
                _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();

                if (_interactableWorker != null)
                    if (_interactableWorker.IsSlacking())
                        _interactableWorker.Deslack();
                bounds++;
                break;

            case "Environment":
                bounds++;
                break;

            default:
                break;
        }
    }
}
