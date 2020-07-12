using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    public PauseMenu pause;

    // Start is called before the first frame update
    void Start()
    {
        pause.Freeze();
    }

    public void CloseTutorial()
    {
        pause.Unfreeze();
        gameObject.SetActive(false);
    }
}
