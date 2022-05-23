using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedDiamond : AITask
{
    bool hasFired = false;
    int currCase = 0;

    Vector3[] firePos1 = { new Vector3(0.0f, -5.0f, 0.0f), new Vector3(0.0f, 5.0f, 0.0f), new Vector3(-5.0f, 0.0f, 0.0f), new Vector3(5.0f, 0.0f, 0.0f) };
    Vector3[] firePos2 = { new Vector3(-5.0f, -5.0f, 0.0f), new Vector3(5.0f, -5.0f, 0.0f), new Vector3(5.0f, 5.0f, 0.0f), new Vector3(-5.0f, 5.0f, 0.0f) };

    public AITaskRedDiamond(AIDataSet data) : base(data)
    {
    }

    public override void firstFrameUpdate()
    {
        hasFired = false;
        currCase = 0;
        base.firstFrameUpdate();
    }

    public override void setFirstFrameRender()
    {
        if (getCAI() != null) getCAI().sendMoveCode(MoveCode.SWIRL, false, 65, 0.95f, 0.95f, 2.0f * Mathf.PI, (3.0f / 2.0f) * Mathf.PI);
    }

    public override bool endCondition()
    {
        return false;
    }

    public override void runAction()
    {
        if (getTimer() == getDuration() - 10 || getTimer() == getDuration() - 60 || getTimer() == getDuration() - 110)
        {
            if (!hasFired)
            {
                if (currCase == 0 || currCase == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), firePos1[i], Quaternion.identity);
                        SplitterShooter shooter = nSplitter.GetComponent<SplitterShooter>();
                        if (shooter != null)
                        {
                            setProjVals(shooter);
                            if (i == 0) shooter.setSpawnVelocity(0, getSpawnVel());
                            else if (i == 1) shooter.setSpawnVelocity(0, -getSpawnVel());
                            else if (i == 2) shooter.setSpawnVelocity(getSpawnVel(), 0);
                            else if (i == 3) shooter.setSpawnVelocity(-getSpawnVel(), 0);
                        }
                    }
                    if (currCase == 2) hasFired = true;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), firePos2[i], Quaternion.identity);
                        SplitterShooter shooter = nSplitter.GetComponent<SplitterShooter>();
                        if (shooter != null)
                        {
                            setProjVals(shooter);
                            shooter.setColor(Color.RED);
                            float c = (getSpawnVel() * Mathf.Sqrt(2)) / 2;
                            if (i == 0) shooter.setSpawnVelocity(c, c);
                            else if (i == 1) shooter.setSpawnVelocity(-c, c);
                            else if (i == 2) shooter.setSpawnVelocity(-c, -c);
                            else if (i == 3) shooter.setSpawnVelocity(c, -c);
                        }
                    }
                }
                currCase++;
            }
        }
    }

    private void setProjVals(SplitterShooter shooter)
    {
        shooter.setSplitterAngle(35.0f);
        shooter.setShootPrefab(getProj());
        shooter.setMaxProjLifeTime(100);
        shooter.setCurrCD(20);
        shooter.setShooterLifetime(48);
        shooter.setMaxCD(50);
        shooter.setProjZRotation(90.0f);
        shooter.setProjDamage(getDamage());
    }

    public override void endAction()
    {
    }

    public override bool runCondition()
    {
        return true;
    }

    public override float getSpawnVel()
    {
        // Wait holy f*zz, it gets faster in phase 3??
        return getAI().getCurrHealth() / getAI().getMaxHealth() > 0.33f ? base.getSpawnVel() : base.getSpawnVel() * 1.2f;
    }
}
