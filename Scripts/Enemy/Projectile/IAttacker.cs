using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAttacker : MonoBehaviour
{
    [SerializeField]
    private int lifeTime;
    private float damage;

    protected void superFixedUpdate()
    {
        if (lifeTime > 0) lifeTime--;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            collider.GetComponent<PlayerStats>().damage(getDamage(collider.GetComponent<PlayerStats>()));
        }
    }

    public void setLifeTime(int amount)
    {
        lifeTime = amount;
    }

    public void setDamage(float val) { damage = val;}
    public virtual float getDamage(PlayerStats stats) { stats.setLastColorHit(Color.WHITE); return damage; }

    protected float getBaseDamage() { return damage; }
}
