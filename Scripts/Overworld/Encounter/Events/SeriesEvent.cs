using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriesEvent : EncounterEvent
{
    public EncounterEvent[] events;
    int currEvent = 0;

    public override void preEvent()
    {

    }

    public override void runEvent()
    {

    }

    public override void postEvent()
    {

    }

    public override bool receiveInteract()
    {
        return false;
    }

    public override bool finishCondition()
    {
        throw new System.NotImplementedException();
    }
}
