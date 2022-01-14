using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBossAI : EnemyAI
{
    public GameObject redDiamondSplitter, redDiamondProjectile;

    public GameObject redRingSplitter, redRingProjectile;

    public GameObject shortSword, longSword;

    static Color[] ringColors = { Color.WHITE, Color.RED, Color.WHITE, Color.RED, Color.WHITE };
    static Color[] ringColors2 = { Color.WHITE, Color.RED, Color.WHITE};


    protected override void AITaskSequence()
    {

    }

    protected override void AITaskRandom()
    {
        AIDataSet redDiaData = new AIDataSet(this, tPlayer, 150, 5.0f, 7.0f, redDiamondSplitter, redDiamondProjectile);
        AIDataSet redRinData = new AIDataSet(this, tPlayer, 250, 5.0f, 4.5f, redRingSplitter, redRingProjectile);
        AIDataSet redRi2Data = new AIDataSet(this, tPlayer, 275, 5.0f, 4.5f, redRingSplitter, redRingProjectile);
        AIDataSet redSwoData = new AIDataSet(this, tPlayer, 50, 5.0f, 9.0f, null, null);
        AIDataSet redSw2Data = new AIDataSet(this, tPlayer, 200, 5.0f, 12.0f, null, null);

        aiTaskR.Add(new AITaskRedDiamond(redDiaData));
        aiTaskR.Add(new AITaskRedThreeRingSpread(redRinData, 10, 10, 75, 25, ringColors));
        aiTaskR.Add(new AITaskRedSwordTrackTop(redSwoData, shortSword, longSword));
        aiTaskR.Add(new AITaskRedSwordOmnihit(redSw2Data, longSword, 50));
        aiTaskR.Add(new AITaskMultiRingSpread(redRi2Data, 10, 10, 75, 25, ringColors, ringColors2));
    }

    protected override void AITaskForce()
    {

    }
}
