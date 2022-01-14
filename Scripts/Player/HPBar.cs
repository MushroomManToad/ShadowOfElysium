using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public Sprite HPBarBackground, HPBarBackgroundShattered;
    public Image HPBarBackgroundImage;
    public Image HPBarFill;

    private int damageAnimTimer = 0;
    private int damageAnimTimerMax = 30;
    private bool damageSign;
    private int flashTimer = 0;
    private int flashTimerMax = 3;

    private Vector2 rectStartPos = new Vector2();

    private void Start()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        if (rect != null)
        {
            rectStartPos = new Vector2(rect.position.x, rect.position.y);
        }
    }

    private void FixedUpdate()
    {
        if(damageAnimTimer > 0)
        {
            damageAnimTimer--;
            handleDamageAnim();
        }
    }

    public void syncHP(float maxHealth, float currHealth, bool flash)
    {
        float val = (float) currHealth / (float)maxHealth;
        slider.value = val < 0 ? 0 : val;
        if (flash)
        {
            damageAnimTimer = damageAnimTimerMax;
        }
    }

    public void setMortalRender()
    {
        HPBarBackgroundImage.sprite = HPBarBackgroundShattered;
    }

    public void setColor(Color c)
    {

    }


    private void handleDamageAnim()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        if (rect != null)
        {
            if (flashTimer <= 0)
            {
                flashTimer = flashTimerMax;

                if (damageSign)
                {
                    int i = Random.Range(0, 8);
                    switch (i)
                    {
                        case 0:
                            rect.position = new Vector3(rect.position.x, rect.position.y - 4, rect.position.z);
                            break;
                        case 1:
                            rect.position = new Vector3(rect.position.x, rect.position.y + 4, rect.position.z);
                            break;
                        case 2:
                            rect.position = new Vector3(rect.position.x - 4, rect.position.y, rect.position.z);
                            break;
                        case 3:
                            rect.position = new Vector3(rect.position.x + 4, rect.position.y, rect.position.z);
                            break;
                        case 4:
                            rect.position = new Vector3(rect.position.x - 4, rect.position.y - 4, rect.position.z);
                            break;
                        case 5:
                            rect.position = new Vector3(rect.position.x + 4, rect.position.y - 4, rect.position.z);
                            break;
                        case 6:
                            rect.position = new Vector3(rect.position.x - 4, rect.position.y + 4, rect.position.z);
                            break;
                        case 7:
                            rect.position = new Vector3(rect.position.x + 4, rect.position.y + 4, rect.position.z);
                            break;
                        default: break;
                    }
                }
                else
                {
                    rect.position = new Vector3(rectStartPos.x, rectStartPos.y, rect.position.z);
                }
                damageSign = !damageSign;
            }
            else
            {
                flashTimer--;
            }

            if (damageAnimTimer <= 0)
            {
                rect.position = new Vector3(rectStartPos.x, rectStartPos.y, rect.position.z);
                flashTimer = 0;
            }
        }
    }
}
