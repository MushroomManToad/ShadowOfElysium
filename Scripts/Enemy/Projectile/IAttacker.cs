using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAttacker : MonoBehaviour
{
    [SerializeField]
    private int lifeTime;
    private float damage;

    private bool hasCollided;

    protected void superFixedUpdate()
    {
        if (lifeTime > 0) lifeTime--;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerStats stats = collider.GetComponent<PlayerStats>();
        // Code inside this block will only be called once: When the projectile actually hits the player.
        if (stats != null && !hasCollided)
        {
            // Deal damage to the player
            stats.damage(getDamage(stats));
            // Udpate hasCollided
            hasCollided = true;

            // Remove 1 from the appropriate counter of the Lumen Receieve Box and redraw wireframe if necessary.
            LumenReceiveBox lrb = stats.lumenReceiveBox;
            Color c = Color.WHITE;
            if(this is IProjectile)
            {
                IProjectile proj = (IProjectile)this;
                c = proj.getColor();
            }
            if (lrb.removeFromCounter(c, 1))
            {
                lrb.syncLumenWireframe();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // LumenReceiveBox is a special collider attached to the player that's larger than the player's normal hitbox to let Lumen gain be more player-favored.
        LumenReceiveBox lrb = collider.GetComponent<LumenReceiveBox>();

        // Get the appropriate color
        Color color = Color.WHITE;
        if(this is IProjectile)
        {
            IProjectile proj = (IProjectile) this;
            color = proj.getColor();
        }

        // Cannot gain lumens if not a player, if the projectile has already hit the player, or if the player is the same color as the projectile (Except white).
        if (lrb != null &&
            !getHasCollided() &&
            (lrb.getColor() != color || lrb.getColor() == Color.WHITE)
        )
        {
            lrb.getStats().addLumens(0.1f);
        }
    }

    public void setLifeTime(int amount)
    {
        lifeTime = amount;
    }

    public void setDamage(float val) { damage = val;}
    public virtual float getDamage(PlayerStats stats) { stats.setLastColorHit(Color.WHITE); return damage; }

    protected float getBaseDamage() { return damage; }

    public bool getHasCollided() { return hasCollided; }
}
