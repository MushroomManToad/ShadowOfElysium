using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriesEvent : EncounterEvent
{
    public EncounterEvent[] events;
    int currEvent = 0;

    public override void preEvent()
    {
        if(events.Length > 0)
        {
            startEvent(events[0]);
        }
    }

    public override void runEvent()
    {
        if(currEvent + 1 < events.Length)
        {
            if(events[currEvent].getIsFinished())
            {
                currEvent++;
                startEvent(events[currEvent]);
            }
        }
        else
        {
            // The last event ends without starting the next event.
            if(events[currEvent].getIsFinished())
            {
                currEvent++;
            }
        }
    }

    public override void postEvent()
    {
        currEvent = 0;
    }

    private void startEvent(EncounterEvent e)
    {
        e.run();
    }

    public override bool receiveInteract()
    {
        return false;
    }

    public override bool finishCondition()
    {
        return currEvent >= events.Length;
    }
}
