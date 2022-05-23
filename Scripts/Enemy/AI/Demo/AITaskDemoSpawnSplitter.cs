using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskDemoSpawnSplitter : AITask
{
    bool hasFired = false;

    public AITaskDemoSpawnSplitter(AIDataSet data) : base(data)
    {

    }

    public override bool endCondition()
    {
        return false;
    }

    public override void setFirstFrameRender()
    {
        //throw new System.NotImplementedException();
    }

    public override void runAction()
    {
        if (!hasFired)
        {
            GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), getAI().genPosOnEdge(), Quaternion.identity);
            IShooter shooter = nSplitter.GetComponent<IShooter>();
            TrackerShooter ts = nSplitter.GetComponent<TrackerShooter>();
            if (shooter != null && ts != null)
            {
                shooter.setShootPrefab(getProj());
                shooter.setMaxProjLifeTime(100);
                shooter.setSpawnVelocity(0, 7);
                shooter.setCurrCD(20);
                shooter.setShooterLifetime(31);
                shooter.setMaxCD(50);
                ts.setTarget(getPlayer());
                hasFired = true;
            }
        }
    }

    public override void endAction()
    {
        hasFired = false;
    }

    public override bool runCondition()
    {
        return true;
    }
}
