using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedSmite: AITask
{
    bool hasRun = false;

    public AITaskRedSmite(AIDataSet data) : base(data)
    {

    }

    public override void firstFrameUpdate()
    {
        // LOGIC HERE

        base.firstFrameUpdate();
    }

    public override void setFirstFrameRender()
    {
        throw new System.NotImplementedException();
    }

    public override void runAction()
    {
        throw new System.NotImplementedException();
    }

    public override void endAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool endCondition()
    {
        return false;
    }

    public override bool runCondition()
    {
        return !hasRun && getAI().getHealthPercent() < 0.33f;
    }
}
