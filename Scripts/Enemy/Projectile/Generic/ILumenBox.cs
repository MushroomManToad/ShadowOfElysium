using System.Collections;
using UnityEngine;

public class ILumenBox : MonoBehaviour
{
    public IAttacker attacker;
    public IProjectile projectile;

    private Color color = Color.WHITE;

    private void Start()
    {
        if(projectile != null)
        {
            color = projectile.getColor();
        }
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        // LumenReceiveBox is a special collider attached to the player that's larger than the player's normal hitbox to let Lumen gain be more player-favored.
        LumenReceiveBox lrb = collider.GetComponent<LumenReceiveBox>();
        // Cannot gain lumens if not a player, if the projectile has already hit the player, or if the player is the same color as the projectile (Except white).
        if (lrb != null && 
            !attacker.getHasCollided() && 
            (lrb.getColor() != color || lrb.getColor() == Color.WHITE)
        )
        {
            lrb.getStats().addLumens(0.1f);
        }
    }
}