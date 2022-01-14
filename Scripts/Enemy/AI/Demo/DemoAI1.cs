using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAI1 : EnemyAI
{
    public GameObject usingSplitter;
    public GameObject splitterProjectile;

    protected override void AITaskSequence()
    {
        
    }

    protected override void AITaskRandom()
    {
        aiTaskF.Add(new AITaskDemoSpawnSplitter(new AIDataSet(this, tPlayer, 35, 5, 7.0f, usingSplitter, splitterProjectile)));
    }

    protected override void AITaskForce()
    {
        
    }
}
