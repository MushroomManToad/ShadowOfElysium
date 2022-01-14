using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IShooter : MonoBehaviour
{
    protected GameObject shootPrefab;
    public SpriteRenderer indicator;
    protected Vector2 spawnVel;
    protected int maxProjLifeTime;
    protected int maxCD = 50;
    protected int currCD = 0;
    protected int shooterLifeTime = -1;
    protected int startIndicator = 20;
    protected int indicatorFlashTime = 4;
    protected float projDamage = 0;
    private int currIndicatorFlashTime = 0;
    protected float projZRotation = 0;
    private Color color = Color.WHITE;

    private void Start()
    {
        if(indicator != null) Colors.updateIndicator(color, indicator);
    }

    private void FixedUpdate()
    {
        if (currCD > 0)
        {
            currCD--;
            if(indicator != null)
            {
                if(currCD > startIndicator || currCD <= 0)
                {
                    indicator.enabled = false;
                }
                else if(currCD > 0)
                {
                    if (currIndicatorFlashTime <= 0)
                    {
                        currIndicatorFlashTime = indicatorFlashTime;
                        indicator.enabled = !indicator.enabled;
                    }
                    else
                    {
                        currIndicatorFlashTime--;
                    }
                }
            }
        }
        else
        {
            currCD = maxCD;
            onShoot();
        }
        if (shooterLifeTime > 0) shooterLifeTime--;
        else if (shooterLifeTime == 0) Destroy(gameObject);
    }

    protected void setProjDefaults(GameObject proj)
    {
        if (proj.GetComponent<IAttacker>() != null)
        {
            IAttacker proja = proj.GetComponent<IAttacker>();
            proja.setLifeTime(getMaxProjLifeTime());
            proja.setDamage(getProjDamage());
            if (proja is IProjectile)
            {
                Colors.sync(proj, getColor());
            }
        }
    }

    public abstract void onShoot();

    public void setMaxCD(int val) { maxCD = val; }
    public int getMaxCD() { return maxCD;}
    public void setCurrCD(int val) { currCD = val; }
    public int getCurrCD() { return currCD;}
    public void setMaxProjLifeTime(int val) { maxProjLifeTime = val; }
    public int getMaxProjLifeTime() { return maxProjLifeTime;}
    public void setShootPrefab(GameObject prefab) { shootPrefab = prefab; }
    public GameObject getShootPrefab() { return shootPrefab; }
    public void setSpawnVelocity(float x, float y) { spawnVel = new Vector2(x, y);}
    public void setSpawnVelocity(Vector2 val) { spawnVel = val; }
    public Vector2 getSpawnVelocity() { return spawnVel; }
    public void setShooterLifetime(int val) { shooterLifeTime = val; }
    public int getShooterLifetime() { return shooterLifeTime; }
    public void setStartIndicator(int val) { startIndicator = val; }
    public int getStartIndicator() { return startIndicator; }
    public void setIndicatorFlashTime(int val) { indicatorFlashTime = val;}
    public int getIndicatorFlashTime() { return indicatorFlashTime; }
    public void setProjZRotation(float val) { projZRotation = val; }
    public float getProjZRotation() { return projZRotation; }
    public void setColor(Color c) 
    {
        color = c;
        if(indicator != null) Colors.updateIndicator(color, indicator);
    }
    public Color getColor() { return color; }
    public void setProjDamage(float val) { projDamage = val; }
    public float getProjDamage() { return projDamage; }
}
