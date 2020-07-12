using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableScript : MonoBehaviour
{
    public float throwForce = 10.0f;

    [SerializeField]
    private WorkerControl _interactableWorker = null;

    public GameObject throwablePickupPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Worker":
                _interactableWorker = collision.gameObject.GetComponent<WorkerControl>();

                if (_interactableWorker != null)
                    if (_interactableWorker.IsSlacking())
                        _interactableWorker.Deslack();
                Instantiate(throwablePickupPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;

            case "Environment":
                Instantiate(throwablePickupPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }
}
