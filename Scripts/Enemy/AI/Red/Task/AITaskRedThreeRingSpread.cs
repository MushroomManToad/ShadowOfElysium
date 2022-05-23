using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedThreeRingSpread : AITask
{
    float angleOfSeparation;
    float startAngle;
    Color[] colors;
    int currRing = 0;
    int numProj;
    int delay;

    int atkTimer = 0;
    int atkTimerMax = 0;

    public AITaskRedThreeRingSpread(AIDataSet data, float angleOfSeparation, int numProj, int delay, int gap, Color[] colors) : base(data)
    {
        this.angleOfSeparation = angleOfSeparation;
        this.colors = colors;
        this.numProj = numProj;
        this.delay = delay;
        atkTimerMax = gap;
    }

    public override void firstFrameUpdate()
    {
        atkTimer = 0;
        currRing = 0;
        startAngle = (float)Random.Range(-angleOfSeparation, angleOfSeparation + 1);
        base.firstFrameUpdate();
    }

    public override void setFirstFrameRender()
    {
        if (getCAI() != null) getCAI().sendMoveCode(MoveCode.CENTER_SLAM, false);
    }

    public override void runAction()
    {
        if(getTimer() <= getDuration() - delay)
        {
            if(atkTimer <= 0)
            {
                atkTimer = atkTimerMax;
                if(currRing < colors.Length)
                {
                    GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), new Vector3(0, 5, 0), Quaternion.identity);
                    if(nSplitter.GetComponent<SplitterShooter>() != null)
                    {
                        float offset = 0;
                        if(currRing > 0)
                        {
                            int i = Random.Range(0, 4);
                            if (i == 0) offset = angleOfSeparation * 0.25f;
                            else if (i == 1) offset = angleOfSeparation * 0.5f;
                            else if (i == 2) offset = angleOfSeparation * 0.75f;
                        }
                        setProjVals(nSplitter.GetComponent<SplitterShooter>(), startAngle + offset);
                    }
                }
                currRing++;
            }
            atkTimer--;
        }
    }

    private void setProjVals(SplitterShooter shooter, float spawnAngle)
    {
        shooter.setSplitterAngle(angleOfSeparation);
        shooter.setShootPrefab(getProj());
        shooter.setMaxProjLifeTime(175);
        shooter.setCurrCD(1);
        shooter.setShooterLifetime(3);
        shooter.setMaxCD(50);
        shooter.setProjZRotation(0.0f);
        shooter.setProjDamage(getDamage());
        shooter.setColor(colors[currRing]);
        shooter.setExtraProj(numProj);
        shooter.setSpawnVelocity(getSpawnVel() * Mathf.Sin(toRad(spawnAngle)), -getSpawnVel() * Mathf.Cos(toRad(spawnAngle)));
    }

    private float toRad(float angleInDegrees)
    {
        return angleInDegrees * Mathf.PI / 180.0f;
    }

    public override void endAction()
    {
    }

    public override bool endCondition()
    {
        return false;
    }

    public override bool runCondition()
    {
        if (getAI().getCurrHealth() / getAI().getMaxHealth() > 0.33f)
        {
            return true;
        }
        else return false;
    }
}
