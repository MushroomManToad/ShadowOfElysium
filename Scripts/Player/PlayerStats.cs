using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Serialized Fields (All handled through PlayerSaveData)
    private float maxHealth;
    private float currHealth;
    private int lumenPickups;

    // Internal (Non-Save-Serialized Fields)
    private int iFrames = 0;
    private int iFrameMax = 75;

    private float lumens = 0;

    public SpriteRenderer playerSprite;
    private int iFrameMaxTimer = 6;
    private int iFrameTimer = 0;
    private bool inIFrameAnim;
    public HPBar healthBar;
    public LumenBar lumenBar;
    public LumenReceiveBox lumenReceiveBox;

    public float whiteLumenCost, redLumenCost, orangeLumenCost, yellowLumenCost, greenLumenCost, blueLumenCost, purpleLumenCost, blackLumenCost;

    private Color activeColor = Color.WHITE;

    private Color lastColorHit = Color.WHITE;

    private bool mortal;

    // Contains the methods necessary to communicate with the file. Should only be read on Start, and written to onDestroy or at Battle Ends.
    private PlayerSaveData saveData;

    public void Start()
    {
        // Loads Save Data
        saveData = new PlayerSaveData(this);
        saveData.loadSaveData();

        // Renderer Updater
        updateHealth(false);
        updateLumenRenderer();
    }

    public void OnDestroy()
    {
        savePlayerData();
    }

    private void savePlayerData()
    {
        PlayerSaveData.PlayerDataBundle dataBundle = new PlayerSaveData.PlayerDataBundle();
        dataBundle.setLumenPickups(lumenPickups);
        dataBundle.setCurrHealth(currHealth);
        dataBundle.setMaxHealth(maxHealth);

        saveData.saveData(dataBundle);
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

    private void updateHealth(bool shake)
    {
        healthBar.syncHP(maxHealth, currHealth, shake);
    }

    public bool damage(float amount)
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
            updateHealth(true);
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
            return true;
        }
        return false;
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
        updateLumenRenderer();
        lumenReceiveBox.syncLumenWireframe();
    }
    public Color getActiveColor()
    {
        return activeColor;
    }

    public void setLastColorHit(Color c)
    {
        lastColorHit = c;
    }

    /*
     * Convenience methods for adding and removing Lumens as needed.
     */
    public float getLumens()
    {
        return lumens;
    }

    public void setLumens(float amount)
    {
        if (amount > getMaxLumens()) lumens = getMaxLumens();
        else if (amount < 0) lumens = 0;
        else lumens = amount;

        updateLumenRenderer();
    }

    public void addLumens(float amount)
    {
        if(amount >= 0)
        {
            setLumens(getLumens() + amount);
        }
        else
        {
            removeLumens(-amount);
        }
    }

    public void removeLumens(float amount)
    {
        if(amount >= 0)
        {
            setLumens(getLumens() - amount);
        }
        else
        {
            addLumens(-amount);
        }
    }

    public float getLumenCost(Color c)
    {
        switch (c)
        {
            case (Color.WHITE):
                return whiteLumenCost;
            case (Color.RED):
                return redLumenCost;
            case (Color.ORANGE):
                return orangeLumenCost;
            case (Color.YELLOW):
                return yellowLumenCost;
            case (Color.GREEN):
                return greenLumenCost;
            case (Color.BLUE):
                return blueLumenCost;
            case (Color.PURPLE):
                return purpleLumenCost;
            case (Color.BLACK):
                return blackLumenCost;
            default:
                return 0.0f;
        }
    }

    private void updateLumenRenderer()
    {
        lumenBar.syncHP(getMaxLumens(), getLumens(), false);
    }

    public float getMaxLumens()
    {
        return 50.0f + 10.0f * lumenPickups;
    }

    // Sets values from a PlayerSaveData.PlayerDataBundle on save loading
    public void loadSaveData(PlayerSaveData.PlayerDataBundle dataBundle)
    {
        this.maxHealth = dataBundle.getMaxHealth();
        this.currHealth = this.maxHealth;
        //this.currHealth = dataBundle.getCurrHealth();
        this.lumenPickups = dataBundle.getLumenPickups();
    }

    /*
     * Bullet Damage and such.
     */
    public float getDamage(Color c)
    {
        switch (c) {
            case (Color.WHITE):
                return 1.0f;
            case (Color.RED):
                return 7.0f;
            default:
                return 0.0f;
        }
    }
}
