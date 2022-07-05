using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform chkPos;
    public float distance;
    public float groundAngle;
    public Vector2 perp;
    public Vector2 groundVector;
    public LayerMask groundLayer;

    public bool onGround;
    public Vector2 bottomOffset;
    public Vector2 bottomSize;
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
        ChkGroundAngle(RayHit());
    }
    RaycastHit2D RayHit()
    {
        RaycastHit2D nowHit;
        RaycastHit2D hitDown = Physics2D.Raycast(chkPos.position,Vector2.down,distance,groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(chkPos.position,Vector2.right,distance,groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(chkPos.position,Vector2.left,distance,groundLayer);
        if(hitRight){
            nowHit = hitRight;
        }else if(hitLeft)
        {
            nowHit = hitLeft;
        }else
        {
            nowHit = hitDown;
        }
        return nowHit;
    }
    void ChkGroundAngle(RaycastHit2D hit)
    {
        groundAngle = Vector2.Angle(hit.normal,Vector2.up);
        groundVector = Vector2.Reflect(hit.normal,Vector2.up);
        perp = Vector2.Perpendicular(hit.normal).normalized;
        Debug.DrawLine(hit.point,hit.point+hit.normal,Color.green);
        Debug.DrawLine(hit.point,hit.point+perp,Color.red);
        Debug.DrawLine(chkPos.position,hit.point,Color.blue);
    }
    void contactChk()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset,bottomSize,0,groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position  + bottomOffset, bottomSize);
    }
}
