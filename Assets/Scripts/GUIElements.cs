using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIElements : MonoBehaviour
{
    private static Image throwables;
    private static Image throwablesA;

    public Image Throwables;
    public Image ThrowablesA;

    private void Awake()
    {
        if (Throwables) throwables = Throwables;
        if (ThrowablesA) throwablesA = ThrowablesA;
    }

    public static void ToggleIcon(int i, bool active = true)
    {
        switch (i)
        {
            case 0:
                throwables.gameObject.SetActive(active);
                throwablesA.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public static void ToggleActive(int i, bool active = true)
    {
        switch (i)
        {
            case 0:
                throwablesA.gameObject.SetActive(active);
                throwables.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
