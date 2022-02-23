using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractible : IInteractible
{
    public EncounterEvent e;

    public override bool onInteract()
    {
        if (e != null && !EEventManager.contains(e))
        {
            e.run();
            return true;
        }
        return false;
    }
}
