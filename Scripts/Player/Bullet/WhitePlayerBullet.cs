using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePlayerBullet : IPlayerBullet
{
    protected override void onImpact(GameObject hit)
    {
        CrystalMover cm = hit.GetComponent<CrystalMover>();
        if(cm != null)
        {
            CrystalAI ai = cm.getCrystalAI();
            PlayerStats ps = ai.tPlayer.GetComponent<PlayerStats>();
            if(ps != null)
            {
                ai.damage(ps.getDamage(Color.WHITE));
            }
            else
            {
                Debug.LogError("No player attached to the enemy AI. Is this an error.");
            }
        }
        Destroy(gameObject);
    }
}
