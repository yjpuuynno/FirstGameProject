using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform LedgeChkPos;

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
    public Vector2 LedgeRayBottomOffset;
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
        onLedge = LedgeRayHit();
        LedgeRayHit();
    }
    bool LedgeRayHit()
    {
        RaycastHit2D onLedgeChkMid = Physics2D.Raycast((Vector2)LedgeChkPos.position,Vector2.right * wallSide, wallDistance, groundLayer);
        RaycastHit2D onLedgeChkBottom = Physics2D.Raycast((Vector2)LedgeChkPos.position + LedgeRayBottomOffset, Vector2.right * wallSide, wallDistance, groundLayer);

        Debug.DrawLine((Vector2)LedgeChkPos.position, (Vector2)LedgeChkPos.position + new Vector2(wallDistance * wallSide,0), Color.red);
        Debug.DrawLine((Vector2)LedgeChkPos.position + LedgeRayBottomOffset, (Vector2)LedgeChkPos.position + new Vector2(wallDistance * wallSide,LedgeRayBottomOffset.y),Color.green);
        return !onLedgeChkMid && onLedgeChkBottom;
    }
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
