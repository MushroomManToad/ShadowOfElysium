using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerSword : IProjectile
{
    private int trackingTime;
    private float speed;
    private GameObject targetPlayer;
    [SerializeField]
    private Rigidbody2D r2;

    private Vector2 trackingVector;

    void Start()
    {
        trackingVector = new Vector2(targetPlayer.transform.position.x - transform.position.x, targetPlayer.transform.position.y - transform.position.y).normalized;
        float angle = Mathf.Asin(trackingVector.y);
        if (trackingVector.x == 0)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                (trackingVector.y > 0 ? 90.0f : -90.0f));
        }
        else
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                Mathf.Atan2(trackingVector.y, trackingVector.x) * (180.0f / Mathf.PI));
        }
    }

    private void FixedUpdate()
    {
        trackingTime--;
        if(trackingTime >= 0)
        {
            trackingVector = new Vector2(targetPlayer.transform.position.x - transform.position.x, targetPlayer.transform.position.y - transform.position.y).normalized;
            float angle = Mathf.Asin(trackingVector.y);
            if (trackingVector.x == 0)
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    (trackingVector.y > 0 ? 90.0f : -90.0f));
            }
            else
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    Mathf.Atan2(trackingVector.y, trackingVector.x) * (180.0f / Mathf.PI));
            }
        }
        if (trackingTime == 0)
        {
            launch();
        }
        superFixedUpdate();
    }

    private void launch()
    {
        r2.velocity = trackingVector * speed;
    }

    public void setTargetPlayer(GameObject player)
    {
        targetPlayer = player;
    }

    public void setTrackingTimer(int val) { trackingTime = val; }
    public void setSpeed(float val) { speed = val; }
}
