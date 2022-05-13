using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    // This vector is updated each frame by most of the move code methods. Used to store the in-calculation movement vector. 
    // Used directly by makeMove() to update the transform.
    private Vector2 moveVector = new Vector2(0.0f, 0.0f);
    
    // Contains all stats, inventory, health, etc.
    [SerializeField]
    private PlayerStats playerStats;

    public float pScaleX = 0.51f;
    public float pScaleY = 0.51f;
    private float pOffset = 0.1f;

    private float pCastOffset = 0.01f;

    public float scaleMoveDir;

    /* Boolean variables tracking which keys are input. Are set to true on key down, and are set to false on key released.
     * Do not care about pause state. If the game is running, these variables are updating. Used ONLY in computeMove()
     */
    private bool isMovingRight, isMovingLeft, isMovingUp, isMovingDown;
    // Speed is halved using the left shift (or rebound, eventually) key. Calculated in computeMove().
    private bool isSneaking = false;

    private Color activeColor = Color.WHITE;
    private List<Color> activeColors = new List<Color>();

    private bool isShooting;
    private int shootingCD;
    private int maxShootingCD = 25;

    public GameObject whiteBullet, redBullet, orangeBullet, yellowBullet, greenBullet;

    public GameObject[] spriteColors;

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
        playerControls.Player.Up.started += _ => { isMovingUp = true; };
        playerControls.Player.Down.started += _ => { isMovingDown = true; };
        playerControls.Player.Right.started += _ => { isMovingRight = true; };
        playerControls.Player.Left.started += _ => { isMovingLeft = true; };
        
        playerControls.Player.Up.canceled += _ => { isMovingUp = false; };
        playerControls.Player.Down.canceled += _ => { isMovingDown = false; };
        playerControls.Player.Right.canceled += _ => { isMovingRight = false; };
        playerControls.Player.Left.canceled += _ => { isMovingLeft = false; };

        playerControls.Player.Red.started += _ => { setColor(Color.RED); };
        playerControls.Player.Orange.started += _ => { setColor(Color.ORANGE); };
        playerControls.Player.Yellow.started += _ => { setColor(Color.YELLOW); };
        playerControls.Player.Green.started += _ => { setColor(Color.GREEN); };
        playerControls.Player.Blue.started += _ => { setColor(Color.BLUE); };
        playerControls.Player.Purple.started += _ => { setColor(Color.PURPLE); };

        playerControls.Player.Red.canceled += _ => { removeColor(Color.RED); };
        playerControls.Player.Orange.canceled += _ => { removeColor(Color.ORANGE); };
        playerControls.Player.Yellow.canceled += _ => { removeColor(Color.YELLOW); };
        playerControls.Player.Green.canceled += _ => { removeColor(Color.GREEN); };
        playerControls.Player.Blue.canceled += _ => { removeColor(Color.BLUE); };
        playerControls.Player.Purple.canceled += _ => { removeColor(Color.PURPLE); };

        playerControls.Player.Shoot.started += _ => { startShooting(); };
        playerControls.Player.Shoot.canceled += _ => { stopShooting(); };

        playerControls.Player.Slow.started += _ => { isSneaking = true; };
        playerControls.Player.Slow.canceled += _ => { isSneaking = false; };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Current protocol is remove Lumens (and fix color accordingly), shoot, then move.
        updateLumens();
        shootLogic();
        computeMove();
        makeMove();
    }

    private void computeMove()
    {
        Vector2 tryMoveVector = new Vector2(0.0f, 0.0f);
        // Create a new vector that is the base movement speed adjusted for sneaking
        float moveAmount = scaleMoveDir * (isSneaking ? 0.5f : 1.0f);
        if (isMovingUp) tryMoveVector = tryMoveVector + new Vector2(0.0f, moveAmount);
        if (isMovingDown) tryMoveVector = tryMoveVector + new Vector2(0.0f, -moveAmount);
        if (isMovingRight) tryMoveVector = tryMoveVector + new Vector2(moveAmount, 0.0f);
        if (isMovingLeft) tryMoveVector = tryMoveVector + new Vector2(-moveAmount, 0.0f);
        moveVector = tryMoveVector;
        applyForces();
        detectICollidable();
    }

    private void applyForces()
    {

    }

    /* 
     * BEGIN MOVEMENT + COLLISION SECTION
     */

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
            if (y < 0)
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
            if (!(m.x == 0.0f || m.y == 0.0f))
            {
                return false;
            }
            // Up / Down Cases
            if (m.x == 0.0f)
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

    /* 
     * END MOVEMENT + COLLISION SECTION
     */

    private void setColor(Color c)
    {
        if(playerStats.getLumens() > 0)
        {
            activeColors.Insert(0, c);
            activeColor = c;
            updateRenderer();
        }
    }

    private void removeColor(Color c)
    {
        // Create pointer to current active color.
        Color oldActive = activeColor;

        // Check to make sure nothing breaks (It never should fail, though, but just to be safe)
        if(activeColors.Contains(c))
        {
            // Remove the color
            activeColors.Remove(c);
            // If more colors exist in the array, switch to those in order of intial button press.
            if(activeColors.Count > 0)
            {
                activeColor = activeColors[0];
            }
            // Otherwise, go back to default white.
            else
            {
                activeColor = Color.WHITE;
            }
        }
        else
        {
            // Failsafe case
            activeColor = Color.WHITE;
            updateRenderer();
        }
        if (oldActive == c)
        {
            updateRenderer();
            stopShooting();
        }
    }

    private void updateRenderer()
    {
        playerStats.setActiveColor(activeColor);
        // Sprite Renderer Update
        if((int)activeColor < spriteColors.Length)
        {
            for(int i = 0; i < spriteColors.Length; i++)
            {
                if(i == (int)activeColor)
                {
                    spriteColors[i].SetActive(true);
                }    
                else
                {
                    spriteColors[i].SetActive(false);
                }
            }
        }
    }

    public Color getColor()
    {
        return activeColor;
    }

    /*
     * Shooting logic
     */

    /*
     * Called on shoot input pressed. 
     */
    private void startShooting()
    {
        isShooting = true;
    }

    /*
     * Called either on shooting input release OR when color changes.
     * Shooting protocol continues when Lumens are exhausted. The shots just won't be fired until enough Lumens are regenerated.
     */
    private void stopShooting()
    {
        isShooting = false;
    }

    /*
     * Called each FixedUpdate to handle shooting logic.
     */
    private void shootLogic()
    {
        if (shootingCD > 0) shootingCD--;
        if (isShooting)
        {
            if (shootingCD <= 0)
            {
                switch (activeColor)
                {
                    case (Color.WHITE):
                        if(playerStats.getLumens() >= playerStats.getLumenCost(activeColor))
                        {
                            playerStats.removeLumens(playerStats.getLumenCost(activeColor));
                            // Set Actives called here to allow setting data before Start() is called.
                            whiteBullet.SetActive(false);
                            GameObject bullet = Instantiate(whiteBullet, new Vector3(), Quaternion.identity);
                            if(bullet.GetComponent<IPlayerBullet>() != null)
                            {
                                bullet.GetComponent<IPlayerBullet>().setPlayerController(this);
                            }
                            bullet.SetActive(true);
                            whiteBullet.SetActive(true);
                            // TODO: Shake lumen meter
                            shootingCD = maxShootingCD;
                        }
                        break;
                    case (Color.RED):
                        if (playerStats.getLumens() >= playerStats.getLumenCost(activeColor))
                        {
                            playerStats.removeLumens(playerStats.getLumenCost(activeColor));
                            // Set Actives called here to allow setting data before Start() is called.
                            redBullet.SetActive(false);
                            GameObject bullet = Instantiate(redBullet, new Vector3(), Quaternion.identity);
                            if (bullet.GetComponent<IPlayerBullet>() != null)
                            {
                                bullet.GetComponent<IPlayerBullet>().setPlayerController(this);
                            }
                            bullet.SetActive(true);
                            redBullet.SetActive(true);
                            // TODO: Shake lumen meter
                            shootingCD = maxShootingCD;
                        }
                        break;
                    case (Color.ORANGE):
                        break;
                    case (Color.YELLOW):
                        break;
                    case (Color.GREEN):
                        break;
                    case (Color.BLUE):
                        break;
                    case (Color.PURPLE):
                        break;
                    case (Color.BLACK):
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /*
     * END SHOOTING LOGIC
     */

    /*
     * Called each frame to handle all Lumen-related events that must be updated each frame.
     */
    private void updateLumens()
    {
        if(activeColor != Color.WHITE)
        {
            playerStats.removeLumens(0.2f);
            if(playerStats.getLumens() <= 0)
            {
                activeColors.Clear();
                activeColor = Color.WHITE;
                updateRenderer();
                // TODO: Shake Lumen Meter.
            }
        }
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
