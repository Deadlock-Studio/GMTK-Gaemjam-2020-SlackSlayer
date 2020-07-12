using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    private Inventory _inventory;
    private Inventory.Item item;
    private void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        switch (gameObject.tag) {
            case "Throwable":
                item = Inventory.Item.THROWABLE;
                break;
            case "Dummy":
                item = Inventory.Item.DUMMY;
                break;
            case "Ghost":
                item = Inventory.Item.GHOST;
                break;
            case "Usb":
                item = Inventory.Item.USB;
                break;
        }
    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            _inventory.IncreaseItemInInventory(item);
            Destroy(gameObject);
        }
    }
}
