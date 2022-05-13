using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    WHITE,
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    PURPLE,
    BROWN,
    LIME,
    PINK,
    BLACK
}

public static class Colors
{
    public static void updateColors(Color color, SpriteRenderer[] sprites)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            switch (color)
            {
                case (Color.WHITE):
                    sprite.color = UnityEngine.Color.white;
                    break;
                case (Color.RED):
                    sprite.color = new UnityEngine.Color(0.9176f, 0.1960f, 0.2353f, 1);
                    break;
                case (Color.ORANGE):
                    sprite.color = new UnityEngine.Color(1, 0.5f, 0, 1);
                    break;
                case (Color.YELLOW):
                    sprite.color = new UnityEngine.Color(1, 1, 0, 1);
                    break;
                case (Color.GREEN):
                    sprite.color = new UnityEngine.Color(0, 0.62f, 0, 1);
                    break;
                case (Color.BLUE):
                    sprite.color = new UnityEngine.Color(0, 0.56f, 1, 1);
                    break;
                case (Color.PURPLE):
                    sprite.color = new UnityEngine.Color(1, 0, 0.5f, 1);
                    break;
                case (Color.BLACK):
                    sprite.color = new UnityEngine.Color(0, 0, 0, 1);
                    break;
                default:
                    break;
            }
        }
    }

    public static void updateIndicator(Color color, SpriteRenderer sprite)
    {
        switch (color)
        {
            case (Color.WHITE):
                sprite.color = new UnityEngine.Color(0.8f, 0.8f, 0.8f, 1);
                break;
            case (Color.RED):
                sprite.color = new UnityEngine.Color(0.75f, 0f, 0f, 1);
                break;
        }
    }

    public static bool sync(GameObject proj, Color shooterColor)
    {
        if (proj.GetComponent<IProjectile>() != null && proj.GetComponent<IProjectile>().getColor() != shooterColor)
        {
            proj.GetComponent<IProjectile>().setColor(shooterColor);
        }
        return false;
    }
}