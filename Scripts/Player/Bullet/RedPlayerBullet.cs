using System.Collections;
using UnityEngine;

public class RedPlayerBullet : IPlayerBullet
{
    protected override void onImpact(GameObject hit)
    {
        CrystalMover cm = hit.GetComponent<CrystalMover>();
        if (cm != null)
        {
            CrystalAI ai = cm.getCrystalAI();
            PlayerStats ps = ai.tPlayer.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ai.damage(ps.getDamage(Color.RED));
            }
            else
            {
                Debug.LogError("No player attached to the enemy AI. Is this an error.");
            }
        }
        Destroy(gameObject);
    }
}