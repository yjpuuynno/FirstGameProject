using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform groundChkPos;
    public Transform LedgeChkPos;
    public float distance;
    public float groundAngle;
    public Vector2 perp;
    public Vector2 groundVector;
    public LayerMask groundLayer;
    public float wallSide;
#region COLLIDER_BOOL
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onLedge;
#endregion

#region OFFSET
    public Vector2 bottomOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
#endregion

    public Vector2 bottomSize;
    [Range(0, 1)] public float wallDistance;
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
        //ChkGroundAngle(GroundRayHit());
        ChkLedge(LedgeRayHit());
        onLedge = onWall && !LedgeRayHit();
    }
    RaycastHit2D LedgeRayHit()
    {
        RaycastHit2D hit;
        RaycastHit2D hitRight = Physics2D.Raycast(LedgeChkPos.position,Vector2.right,distance+0.5f,groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(LedgeChkPos.position,Vector2.left,distance+0.5f,groundLayer);
        if(hitRight){
            hit = hitRight;
        }else 
        {
            hit = hitLeft;
        }
        return hit;
    }
    void ChkLedge(RaycastHit2D hit)
    {
        if(onWall && !onGround && hit)
        {
            Debug.DrawLine(LedgeChkPos.position,hit.point,Color.red);
        } 
    }
    /*
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
        if(onGround)
        {
        Debug.DrawLine(hit.point,hit.point+hit.normal,Color.green);
        Debug.DrawLine(hit.point,hit.point+perp,Color.green);
        Debug.DrawLine(groundChkPos.position,hit.point,Color.blue); 
        }
        
    }
    */
    void contactChk()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset,bottomSize,0,groundLayer);
        RaycastHit2D RightWall = Physics2D.Raycast((Vector2)transform.position + rightOffset,Vector2.right,wallDistance,groundLayer);
        onRightWall = RightWall;
        RaycastHit2D LeftWall = Physics2D.Raycast((Vector2)transform.position + leftOffset,Vector2.left,wallDistance,groundLayer);
        onLeftWall = LeftWall;
        Debug.DrawLine((Vector2)transform.position + rightOffset, (Vector2)transform.position + new Vector2(wallDistance,rightOffset.y),Color.green);
        Debug.DrawLine((Vector2)transform.position + leftOffset, (Vector2)transform.position + new Vector2(wallDistance*-1,leftOffset.y),Color.green);
        wallSide = onRightWall ? 1 : -1; 
        onWall = onRightWall || onLeftWall;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position  + bottomOffset, bottomSize);
    }
}
