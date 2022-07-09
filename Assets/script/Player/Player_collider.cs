using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform groundChkPos;
    public float distance;
    public float groundAngle;
    public Vector2 perp;
    public Vector2 groundVector;
    public LayerMask groundLayer;

#region COLLIDER_BOOL
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
#endregion

#region OFFSET
    public Vector2 bottomOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
#endregion

    public Vector2 bottomSize;
    [Range(0, 0.5f)] public float collisionRadius;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        contactChk();
        ChkGroundAngle(GroundRayHit());
    }
    void LedgeRayHit()
    {
        
    }
    void ChkLedge()
    {

    }
    RaycastHit2D GroundRayHit()
    {
        RaycastHit2D hit;
        RaycastHit2D hitDown = Physics2D.Raycast(groundChkPos.position,Vector2.down,distance,groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(groundChkPos.position,Vector2.right,distance,groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(groundChkPos.position,Vector2.left,distance,groundLayer);
        if(hitRight){
            hit = hitRight;
        }else if(hitLeft)
        {
            hit = hitLeft;
        }else
        {
            hit = hitDown;
        }
        return hit;
    }   
    void ChkGroundAngle(RaycastHit2D hit)
    {
        groundAngle = Vector2.Angle(hit.normal,Vector2.up);
        groundVector = Vector2.Reflect(hit.normal,Vector2.up);
        perp = Vector2.Perpendicular(hit.normal).normalized;
        Debug.DrawLine(hit.point,hit.point+hit.normal,Color.green);
        Debug.DrawLine(hit.point,hit.point+perp,Color.red);
        Debug.DrawLine(groundChkPos.position,hit.point,Color.blue);
    }
    void contactChk()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset,bottomSize,0,groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position  + bottomOffset, bottomSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
