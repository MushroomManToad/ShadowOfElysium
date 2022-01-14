using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Vector2 moveVector = new Vector2(0.0f, 0.0f);
    
    [SerializeField]
    private PlayerStats playerStats;

    public float pScaleX = 0.51f;
    public float pScaleY = 0.51f;
    private float pOffset = 0.1f;

    private float pCastOffset = 0.01f;

    public float scaleMoveDir;

    private bool isMovingRight, isMovingLeft, isMovingUp, isMovingDown;

    private Color activeColor = Color.WHITE;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        computeMove();
        makeMove();
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
        if (moveVector.y < 0.0f)
        {
            if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - pScaleY + pCastOffset), -Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.down))
                if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x + pScaleX - pOffset, transform.position.y - pScaleY + pCastOffset), -Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.down))
                    doShot(Physics2D.Raycast(new Vector2(transform.position.x - pScaleX + pOffset, transform.position.y - pScaleY + pCastOffset), -Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.down);
        }

        if (moveVector.y > 0.0f)
        {
            if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + pScaleY - pCastOffset), Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.up))
                if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x + pScaleX - pOffset, transform.position.y + pScaleY - pCastOffset), Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.up))
                    doShot(Physics2D.Raycast(new Vector2(transform.position.x - pScaleX + pOffset, transform.position.y + pScaleY - pCastOffset), Vector2.up, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.up);
        }

        if (moveVector.x > 0.0f)
        {
            if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y), Vector2.right, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.right))
                if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y + pScaleY - pOffset), Vector2.right, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.right))
                    doShot(Physics2D.Raycast(new Vector2(transform.position.x + pScaleX - pCastOffset, transform.position.y - pScaleY + pOffset), Vector2.right, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.right);
        }

        if (moveVector.x < 0.0f)
        {
            if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y), Vector2.left, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.left))
                if (!doShot(Physics2D.Raycast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y + pScaleY - pOffset), Vector2.left, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.left))
                    doShot(Physics2D.Raycast(new Vector2(transform.position.x - pScaleX + pCastOffset, transform.position.y - pScaleY + pOffset), Vector2.left, scaleMoveDir, LayerMask.GetMask("ICollidable")), Vector2.left);
        }
    }

    private void snapToCollider(RaycastHit2D hit, Vector2 dir)
    {
        transform.position = new Vector3(
            hit.point.x + (dir.x > 0.0f ? -pScaleX : (dir.x < 0.0f ? pScaleX : -hit.point.x + transform.position.x)),
            hit.point.y + (dir.y > 0.0f ? -pScaleY : (dir.y < 0.0f ? pScaleY : -hit.point.y + transform.position.y)),
            transform.position.z);
    }

    private bool doShot(RaycastHit2D down, Vector2 dir)
    {
        if (down.collider != null)
        {
            moveVector = new Vector2(dir.y == 0.0f ? 0.0f : moveVector.x, dir.x == 0.0f ? 0.0f : moveVector.y);
            snapToCollider(down, dir);
            return true;
        }
        return false;
    }

    private void makeMove()
    {
        transform.position = new Vector3(transform.position.x + moveVector.x,
                                 transform.position.y + moveVector.y,
                                 transform.position.z);
    }

    private void setColor(Color c)
    {
        // TODO: Logic to check for having enough Light to shift
        activeColor = c;
        updateRenderer();
    }

    private void removeColor(Color c)
    {
        if(activeColor == c)
        {
            activeColor = Color.WHITE;
            updateRenderer();
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
}
