using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractible : MonoBehaviour
{
    public EncounterEvent e;

    public virtual void onInteract()
    {
        if (e != null)
        {
            e.run();
        }
    }
}
