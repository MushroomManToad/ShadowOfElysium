using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPlayerController : MonoBehaviour
{
    private PlayerControls playerControls;

    private Vector2 moveVector = new Vector2(0.0f, 0.0f);

    public float pScaleX = 0.51f;
    public float pScaleY = 0.51f;
    private float pOffset = 0.01f;
    private float pCastOffset = 0.01f;
    public float scaleMoveDir;

    private Facing facing = Facing.NORTH;
    private float viewRange = 0.95f;

    private bool isMovingRight, isMovingLeft, isMovingUp, isMovingDown;

    private bool inputFrozen = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Movement
        playerControls.Player.Up.started += _ => { setMoveData(Facing.NORTH); };
        playerControls.Player.Down.started += _ => { setMoveData(Facing.SOUTH); };
        playerControls.Player.Right.started += _ => { setMoveData(Facing.EAST); };
        playerControls.Player.Left.started += _ => { setMoveData(Facing.WEST); };

        playerControls.Player.Up.canceled += _ => { isMovingUp = false; };
        playerControls.Player.Down.canceled += _ => { isMovingDown = false; };
        playerControls.Player.Right.canceled += _ => { isMovingRight = false; };
        playerControls.Player.Left.canceled += _ => { isMovingLeft = false; };

        // Interaction
        playerControls.Player.Confirm.started += _ => { sendInteract(); };
    }

    private void setMoveData(Facing dir)
    {
        switch (dir)
        {
            case (Facing.NORTH):
                isMovingUp = true;
                if (!inputFrozen && !isMovingLeft && !isMovingRight && !isMovingDown) setFacing(Facing.NORTH);
                break;
            case (Facing.SOUTH):
                isMovingDown = true;
                if (!inputFrozen && !isMovingLeft && !isMovingRight && !isMovingUp) setFacing(Facing.SOUTH);
                break;
            case (Facing.EAST):
                isMovingRight = true;
                if (!inputFrozen && !isMovingLeft && !isMovingUp && !isMovingDown) setFacing(Facing.EAST);
                break;
            case (Facing.WEST):
                isMovingLeft = true;
                if (!inputFrozen && !isMovingUp && !isMovingRight && !isMovingDown) setFacing(Facing.WEST);
                break;
            default:
                Debug.LogError("Attempted to move in a nonexistent direction.");
                break;
        }
    }

    void FixedUpdate()
    {
        if (!inputFrozen)
        {
            computeMove();
            makeMove();
        }
    }

    private void computeMove()
    {
        Vector2 tryMoveVector = new Vector2(0.0f, 0.0f);
        if (isMovingUp) tryMoveVector = tryMoveVector + new Vector2(0.0f, scaleMoveDir);
        if (isMovingDown) tryMoveVector = tryMoveVector + new Vector2(0.0f, -scaleMoveDir);
        if (isMovingRight) tryMoveVector = tryMoveVector + new Vector2(scaleMoveDir, 0.0f);
        if (isMovingLeft) tryMoveVector = tryMoveVector + new Vector2(-scaleMoveDir, 0.0f);
        moveVector = tryMoveVector;
        applyForces();
        detectICollidable();
    }

    private void applyForces()
    {

    }

    private void detectICollidable()
    {
        float x = moveVector.x;
        float y = moveVector.y;
        if (x != 0 || y != 0)
        {
            float m = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
            RayCastData r = new RayCastData(x, y, m);
            if (x > 0 && y > 0)
            {
                bool xb = xRightCheck(x, y);
                bool yb = yUpCheck(x, y);
                if (!xb && !yb)
                {
                    RaycastHit2D ray = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y + pScaleY - pCastOffset), r);
                    if (ray.collider != null)
                    {
                        diagonalSnap(x, y);
                    }
                }
            }
            else if (x > 0 && y < 0)
            {
                bool xb = xRightCheck(x, y);
                bool yb = yDownCheck(x, y);
                if (!xb && !yb)
                {
                    RaycastHit2D ray = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y - pScaleY + pCastOffset), r);
                    if (ray.collider != null)
                    {
                        diagonalSnap(x, y);
                    }
                }
            }
            else if (x < 0 && y > 0)
            {
                bool xb = xLeftCheck(x, y);
                bool yb = yUpCheck(x, y);
                if (!xb && !yb)
                {
                    RaycastHit2D ray = cast(new Vector2(transform.position.x + -pScaleX + pCastOffset, transform.position.y + pScaleY - pCastOffset), r);
                    if (ray.collider != null)
                    {
                        diagonalSnap(x, y);
                    }
                }
            }
            else if (x < 0 && y < 0)
            {
                bool xb = xLeftCheck(x, y);
                bool yb = yDownCheck(x, y);
                if (!xb && !yb)
                {
                    RaycastHit2D ray = cast(new Vector2(transform.position.x + -pScaleX + pCastOffset, transform.position.y - pScaleY + pCastOffset), r);
                    if (ray.collider != null)
                    {
                        diagonalSnap(x, y);
                    }
                }
            }
            else if (x > 0)
            {
                xRightCheck(x, y);
            }
            else if (x < 0)
            {
                xLeftCheck(x, y);
            }
            else if (y > 0)
            {
                yUpCheck(x, y);
            }
            else if (y < 0)
            {
                yDownCheck(x, y);
            }
        }
    }

    private void diagonalSnap(float x, float y)
    {
        if (x > y)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
            moveVector = new Vector2(0.0f, y);
            if(y < 0)
            {
                yDownCheck(0, y);
            }
            else
            {
                yUpCheck(0, y);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
            moveVector = new Vector2(x, 0.0f);
            if (x < 0)
            {
                xLeftCheck(x, 0);
            }
            else
            {
                xRightCheck(x, 0);
            }
        }
    }

    private bool setMoveWithSnap(RaycastHit2D ray, Vector2 m)
    {
        if (ray.collider != null)
        {
            if(!(m.x == 0.0f || m.y == 0.0f))
            {
                return false;
            }
            // Up / Down Cases
            if(m.x == 0.0f)
            {
                moveVector = new Vector2(moveVector.x, 0.0f);

                transform.position = new Vector3(
                transform.position.x,
                ray.point.y + (m.y > 0.0f ? -pScaleY : pScaleY),
                transform.position.z);
            }
            // Left / Right Cases
            else
            {
                moveVector = new Vector2(0.0f, moveVector.y);

                transform.position = new Vector3(
                ray.point.x + (m.x > 0.0f ? -pScaleX : pScaleX),
                transform.position.y,
                transform.position.z);
            }
            return true;
        }
        return false;
    }

    private bool xRightCheck(float x, float y)
    {
        RaycastHit2D ray0 = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y), x, 0, x);
        RaycastHit2D ray1 = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y + pScaleY - pOffset), x, 0, x);
        RaycastHit2D ray2 = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y - pScaleY + pOffset), x, 0, x);

        if (ray0.collider != null)
        {
            setMoveWithSnap(ray0, Vector2.right);
            return true;
        }
        else if (ray1.collider != null)
        {
            setMoveWithSnap(ray1, Vector2.right);
            return true;
        }
        else if (ray2.collider != null)
        {
            setMoveWithSnap(ray2, Vector2.right);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool xLeftCheck(float x, float y)
    {
        RaycastHit2D ray0 = cast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y), x, 0, -x);
        RaycastHit2D ray1 = cast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y + pScaleY - pOffset), x, 0, -x);
        RaycastHit2D ray2 = cast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y - pScaleY + pOffset), x, 0, -x);

        if (ray0.collider != null)
        {
            setMoveWithSnap(ray0, Vector2.left);
            return true;
        }
        else if (ray1.collider != null)
        {
            setMoveWithSnap(ray1, Vector2.left);
            return true;
        }
        else if (ray2.collider != null)
        {
            setMoveWithSnap(ray2, Vector2.left);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool yUpCheck(float x, float y)
    {
        RaycastHit2D ray0 = cast(new Vector2(transform.position.x, transform.position.y + pScaleY - pCastOffset), 0, y, y);
        RaycastHit2D ray1 = cast(new Vector2(transform.position.x + pScaleX - pOffset, transform.position.y + pScaleY - pCastOffset), 0, y, y);
        RaycastHit2D ray2 = cast(new Vector2(transform.position.x - pScaleX + pOffset, transform.position.y + pScaleY - pCastOffset), 0, y, y);

        if (ray0.collider != null)
        {
            setMoveWithSnap(ray0, Vector2.up);
            return true;
        }
        else if (ray1.collider != null)
        {
            setMoveWithSnap(ray1, Vector2.up);
            return true;
        }
        else if (ray2.collider != null)
        {
            setMoveWithSnap(ray2, Vector2.up);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool yDownCheck(float x, float y)
    {
        RaycastHit2D ray0 = cast(new Vector2(transform.position.x, transform.position.y - pScaleY + pCastOffset), 0, y, -y);
        RaycastHit2D ray1 = cast(new Vector2(transform.position.x + pScaleX - pOffset, transform.position.y - pScaleY + pCastOffset), 0, y, -y);
        RaycastHit2D ray2 = cast(new Vector2(transform.position.x - pScaleX + pOffset, transform.position.y - pScaleY + pCastOffset), 0, y, -y);

        if (ray0.collider != null)
        {
            
            setMoveWithSnap(ray0, Vector2.down);
            return true;
        }
        else if (ray1.collider != null)
        {
            setMoveWithSnap(ray1, Vector2.down);
            return true;
        }
        else if (ray2.collider != null)
        {
            setMoveWithSnap(ray2, Vector2.down);
            return true;
        }
        else
        {
            return false;
        }
    }

    private RaycastHit2D cast(Vector2 spawnLoc, RayCastData r)
    {
        return Physics2D.Raycast(spawnLoc, new Vector2(r.getX(), r.getY()).normalized, r.getM(), LayerMask.GetMask("ICollidable"));
    }

    private RaycastHit2D cast(Vector2 spawnLoc, float x, float y, float m)
    {
        return Physics2D.Raycast(spawnLoc, new Vector2(x, y), m, LayerMask.GetMask("ICollidable"));
    }

    private void makeMove()
    {
        transform.position = new Vector3(transform.position.x + moveVector.x,
                                 transform.position.y + moveVector.y,
                                 transform.position.z);
    }

    private void sendInteract()
    {
        IInteractible i = getFacingInteractible();
        if (i != null)
        {
            if(!i.onInteract())
            {
                EEventManager.sendInteract();
            }
        }
    }

    private IInteractible getFacingInteractible()
    {
        RaycastHit2D hit = new RaycastHit2D();
        switch(facing)
        {
            case (Facing.NORTH):
                hit = cast(new Vector2(transform.position.x, transform.position.y), 0, 1, viewRange);
                break;
            case (Facing.EAST):
                hit = cast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y), 1, 0, viewRange);
                break;
            case (Facing.SOUTH):
                hit = cast(new Vector2(transform.position.x, transform.position.y - pScaleY + pCastOffset), 0, -1, viewRange);
                break;
            case (Facing.WEST):
                hit = cast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y), -1, 0, viewRange);
                break;
            default:
                break;
        }
        if (hit.collider != null && hit.collider.GetComponent<IInteractible>() != null)
        {
            return hit.collider.GetComponent<IInteractible>();
        }
        return null;
    }

    public void setFreezeInputs(bool val)
    {
        inputFrozen = val;
    }

    public bool getFreezeInputs()
    {
        return inputFrozen;
    }

    /**
     * This should *ALWAYS* be used, rather than facing = dir;
     */
    public void setFacing(Facing dir)
    {
        facing = dir;
        // Code that actually reorients the player here.
    }

    protected class RayCastData
    {
        float x, y, m;
        public RayCastData(float x, float y, float m)
        {
            this.x = x;
            this.y = y;
            this.m = m;
        }
        public float getX() { return x; }
        public float getY() { return y; }
        public float getM() { return m; }
    }
}
