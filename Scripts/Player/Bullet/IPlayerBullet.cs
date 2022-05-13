using System.Collections;
using UnityEngine;

public abstract class IPlayerBullet : MonoBehaviour
{
    public float yVel;
    private PlayerController controller;
    // The total height of the bullet / 2.0f, and the total width of the bullet / 2.0f
    public float bulletHeight, bulletWidth;

    private void Start()
    {
        GameObject player = controller.gameObject;
        float pHeight = controller.pScaleY / 2.0f;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + pHeight + bulletHeight);
    }

    private void FixedUpdate()
    {
        attemptCollideAndMove();
    }

    private void attemptCollideAndMove()
    {
        // Three rays. One at each end of the bullet, and one in the center. If any make contact, destroy the bullet, do the visual effect, and damage the enemy crystal.
        RaycastHit2D ray0 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + bulletHeight), Vector2.up, yVel, LayerMask.GetMask("Crystal"));
        RaycastHit2D ray1 = Physics2D.Raycast(new Vector2(transform.position.x + bulletWidth, transform.position.y + bulletHeight), Vector2.up, yVel, LayerMask.GetMask("Crystal"));
        RaycastHit2D ray2 = Physics2D.Raycast(new Vector2(transform.position.x - bulletWidth, transform.position.y + bulletHeight), Vector2.up, yVel, LayerMask.GetMask("Crystal"));
        if (ray0.collider != null)
        {
            moveWithYVel();
            onImpact(ray0.collider.gameObject);
        }
        else if (ray1.collider != null)
        {
            moveWithYVel();
            onImpact(ray1.collider.gameObject);
        }
        else if (ray2.collider != null)
        {
            moveWithYVel();
            onImpact(ray2.collider.gameObject);
        }
        else
        {
            moveWithYVel();
        }
    }

    private void moveWithYVel()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + yVel, transform.position.z);
    }

    public void setPlayerController(PlayerController p)
    {
        controller = p;
    }

    protected abstract void onImpact(GameObject hit);
}