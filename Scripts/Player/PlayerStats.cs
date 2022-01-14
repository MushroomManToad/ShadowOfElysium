using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth = 50;
    private float currHealth = 50;

    private int iFrames = 0;
    private int iFrameMax = 75;

    public SpriteRenderer playerSprite;
    private int iFrameMaxTimer = 6;
    private int iFrameTimer = 0;
    private bool inIFrameAnim;
    public HPBar healthBar;

    private Color activeColor = Color.WHITE;

    private Color lastColorHit = Color.WHITE;

    private bool mortal;

    public void Start()
    {
        healthBar.syncHP(maxHealth, currHealth, false);
    }

    private void FixedUpdate()
    {
        handleIFrames();
    }

    private void handleIFrames()
    {
        if (iFrames > 0)
        {
            iFrames--;

            if (iFrameTimer <= 0)
            {
                iFrameTimer = iFrameMaxTimer;
                if (inIFrameAnim)
                {
                    inIFrameAnim = false;
                    playerSprite.color = new UnityEngine.Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1.0f);
                }
                else
                {
                    inIFrameAnim = true;
                    playerSprite.color = new UnityEngine.Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.5f);
                }
            }
            iFrameTimer--;
        }
        if (inIFrameAnim && iFrames <= 0)
        {
            inIFrameAnim = false;
            playerSprite.color = new UnityEngine.Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1.0f);
        }
    }

    private void updateHealth()
    {
        healthBar.syncHP(maxHealth, currHealth, true);
    }

    public void damage(float amount)
    {
        if(amount < 0)
        {
            Debug.Log("Don't heal with this method! Use PlayerStats.heal() instead!");
        }
        else if (iFrames <= 0 && (activeColor == Color.WHITE || lastColorHit != activeColor))
        {
            if (currHealth - amount < 0) currHealth = 0;
            else currHealth -= amount;
            iFrames = iFrameMax;
            updateHealth();
            if (currHealth <= 0)
            {
                if(mortal)
                {
                    die();
                }
                else
                {
                    mortal = true;
                    healthBar.setMortalRender();
                }
            }
        }
    }

    public void heal(int amount)
    {

    }

    private void die()
    {
        if(lastColorHit == Color.RED)
        {
            Debug.Log("Vak'maer respawn sequence");
        }
        else
        {
            Debug.Log("YOU DIED");
            iFrames = 9999999;
        }
    }

    public void setActiveColor(Color c)
    {
        activeColor = c;
    }
    public Color getActiveColor()
    {
        return activeColor;
    }

    public void setLastColorHit(Color c)
    {
        lastColorHit = c;
    }
}
