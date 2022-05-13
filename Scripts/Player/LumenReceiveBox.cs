using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenReceiveBox : MonoBehaviour
{
    public PlayerStats stats;
    public SpriteRenderer wireframe;

    // Stores the number of colliders of each type of color at that moment.
    int[] collisionCount = new int[11];

    public Color getColor()
    {
        return stats.getActiveColor();
    }

    public PlayerStats getStats()
    {
        return stats;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<IAttacker>() != null)
        {
            if(collider.GetComponent<IProjectile>() != null)
            {
                collisionCount[(int) collider.GetComponent<IProjectile>().getColor()]++;
            }
            else
            {
                collisionCount[(int)Color.WHITE]++;
            }
            syncLumenWireframe();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<IAttacker>() != null)
        {
            if (collider.GetComponent<IProjectile>() != null)
            {
                Color currColor = collider.GetComponent<IProjectile>().getColor();
                collisionCount[(int)currColor]--;
                if (totalCollisionCountExcluding(currColor) <= 0)
                {
                    wireframe.enabled = false;
                }
            }
            else
            {
                collisionCount[(int)Color.WHITE]--;
                if (totalCollisionCount() <= 0)
                {
                    wireframe.enabled = false;
                }
            }
            if (totalCollisionCount() < 0)
            {
                //Should never run, but is a failsafe.
                for (int i = 0; i < collisionCount.Length; i++)
                {
                    collisionCount[i] = 0;
                }
            }
        }
    }

    public void syncLumenWireframe()
    {
        if (!wireframe.enabled &&
            (
                totalCollisionCountExcluding(getColor()) > 0 ||
                (
                    getColor() == Color.WHITE &&
                    totalCollisionCount() > 0
                )
            )
        ) wireframe.enabled = true;
    }

    private int totalCollisionCount()
    {
        int count = 0;
        for (int i = 0; i < collisionCount.Length; i++)
        {
            count += collisionCount[i];
        }
        return count;
    }

    private int totalCollisionCountExcluding(Color c)
    {
        int count = 0;
        for(int i = 0; i < collisionCount.Length; i++)
        {
            if((int) c != i) count += collisionCount[i];
        }
        return count;
    }

    public bool removeFromCounter(Color c, int amount)
    {
        if(collisionCount[(int)c] >= amount)
        {
            collisionCount[(int)c] -= amount;
            return true;
        }
        return false;
    }
}
