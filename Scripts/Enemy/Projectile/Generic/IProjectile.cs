using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IProjectile : IAttacker
{
    [SerializeField]
    private SpriteRenderer[] sprites;

    private Color color = Color.WHITE;

    public void setColor(Color c, bool recolor = true)
    {
        color = c;
        if(recolor) Colors.updateColors(color, sprites);
    }

    public Color getColor()
    {
        return color;
    }

    public override float getDamage(PlayerStats stats)
    {
        stats.setLastColorHit(getColor());
        switch (getColor())
        {
            case Color.WHITE:
                return getBaseDamage();
            case Color.RED:
                return getBaseDamage() * 2;
            default:
                return getBaseDamage();
        }
    }
}
