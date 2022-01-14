using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskMultiRingSpread : AITask
{
    float angleOfSeparation;
    float startAngle;
    Color[] colors, colors2;
    int currRing = 0;
    int currRing2 = 0;
    int numProj;
    int delay;

    int timer = 0;

    int atkTimer = 0;
    int atkTimer2 = 0;
    int atkTimerMax = 0;
    int atkTimerMax2 = 0;

    bool ring2Dir;

    public AITaskMultiRingSpread(AIDataSet data, float angleOfSeparation, int numProj, int delay, int gap, Color[] colors, Color[] colors2) : base(data)
    {
        this.angleOfSeparation = angleOfSeparation;
        this.colors = colors;
        this.colors2 = colors2;
        this.numProj = numProj;
        this.delay = delay;
        atkTimerMax = gap;
        atkTimerMax2 = gap * 3 / 2;

        timer = getDuration();
        ring2Dir = Random.Range(0.0f, 1.0f) < 0.5f;
    }

    public override void runAction()
    {
        if (timer == getDuration())
        {
            startAngle = (float)Random.Range(-angleOfSeparation, angleOfSeparation + 1);
        }
        timer--;
        if (timer <= getDuration() - delay)
        {
            if (atkTimer <= 0)
            {
                atkTimer = atkTimerMax;
                if (currRing < colors.Length)
                {
                    GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), new Vector3(0, 5, 0), Quaternion.identity);
                    if (nSplitter.GetComponent<SplitterShooter>() != null)
                    {
                        float offset = calcOffset();

                        setProjVals(nSplitter.GetComponent<SplitterShooter>(), angleOfSeparation, colors, startAngle + offset, currRing);
                    }
                }
                currRing++;
            }
            atkTimer--;
        }
        if(timer <= getDuration() - delay * 2)
        {
            if (atkTimer2 <= 0)
            {
                atkTimer2 = atkTimerMax2;
                if (currRing2 < colors2.Length)
                {
                    GameObject nSplitter = MonoBehaviour.Instantiate(getSplitter(), ring2Dir ? new Vector3(5, 0, 0) : new Vector3(-5, 0, 0), Quaternion.identity);
                    if (nSplitter.GetComponent<SplitterShooter>() != null)
                    {
                        float offset = calcOffset();

                        setProjVals(nSplitter.GetComponent<SplitterShooter>(), angleOfSeparation * 2, colors2, (ring2Dir ? -90.0f : 90.0f) + startAngle + offset, currRing2);
                    }
                }
                currRing2++;
            }
            atkTimer2--;
        }
    }

    public override void endAction()
    {
        timer = getDuration();
        atkTimer = 0;
        currRing = 0;
        atkTimer2 = 0;
        currRing2 = 0;
        ring2Dir = Random.Range(0.0f, 1.0f) < 0.5f;
    }

    private void setProjVals(SplitterShooter shooter, float aOS, Color[] c, float spawnAngle, int cR)
    {
        shooter.setSplitterAngle(aOS);
        shooter.setShootPrefab(getProj());
        shooter.setMaxProjLifeTime(175);
        shooter.setCurrCD(1);
        shooter.setShooterLifetime(3);
        shooter.setMaxCD(50);
        shooter.setProjZRotation(0.0f);
        shooter.setProjDamage(getDamage());
        shooter.setColor(c[cR]);
        shooter.setExtraProj(numProj);
        shooter.setSpawnVelocity(getSpawnVel() * Mathf.Sin(toRad(spawnAngle)), -getSpawnVel() * Mathf.Cos(toRad(spawnAngle)));
    }

    private float toRad(float angleInDegrees)
    {
        return angleInDegrees * Mathf.PI / 180.0f;
    }

    private float calcOffset()
    {
        float offset = 0;
        if (currRing > 0)
        {
            int i = Random.Range(0, 4);
            switch (i)
            { 
                case 0:
                    offset = angleOfSeparation * 0.25f;
                    break;
                case 1:
                    offset = angleOfSeparation * 0.5f;
                    break;
                case 2:
                    offset = angleOfSeparation * 0.75f;
                    break;
                default:
                    offset = 0;
                    break;
            }
        }
        return offset;
    }

    public override bool endCondition()
    {
        if (timer < 0) return true;
        else return false;
    }

    public override bool runCondition()
    {
        if (getAI().getCurrHealth() / getAI().getMaxHealth() > 0.33f)
        {
            return false;
        }
        else return true;
    }
}
