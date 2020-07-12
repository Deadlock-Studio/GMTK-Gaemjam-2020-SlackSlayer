using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIElements : MonoBehaviour
{
    public const int MAX_ITEMTYPE = 3;
    private static Image[] icon = new Image[MAX_ITEMTYPE];
    private static Image[] iconA = new Image[MAX_ITEMTYPE];

    public Image Throwables;
    public Image ThrowablesA;
    public Image USB;
    public Image USBA;
    public Image Dummy;
    public Image DummyA;

    public Inventory inventory;

    private void Awake()
    {
        if (Throwables) icon[0] = Throwables;
        if (ThrowablesA) iconA[0] = ThrowablesA;
        if (USB) icon[1] = USB;
        if (USBA) iconA[1] = USBA;
        if (Dummy) icon[2] = Dummy;
        if (DummyA) iconA[2] = DummyA;
    }

    private void Update()
    {
        InventoryUpdate(); 
    }

    private void InventoryUpdate()
    {
        if (inventory.GetThrowableNumber() > 0) ToggleIcon(0, true);
        else ToggleIcon(0, false);

        if (inventory.GetUsbNumber() > 0) ToggleIcon(1, true);
        else ToggleIcon(1, false);

        if (inventory.GetDummyNumber() > 0) ToggleIcon(2, true);
        else ToggleIcon(2, false);
    }

    //0 throwables
    //1 usb
    //2 dummy
    public static void ToggleIcon(int i, bool active = true)
    {
        icon[i].gameObject.SetActive(active);
    }

    public static void ToggleActive(int i, bool active = true)
    {
        iconA[i].gameObject.SetActive(active);
        deactivateOther(i);
    }

    private static void deactivateOther(int ex)
    {
        for (int i = 0; i < iconA.Length; i++)
        {
            if (i != ex)
            {
                iconA[i].gameObject.SetActive(false);
            }
        }
    }
}
