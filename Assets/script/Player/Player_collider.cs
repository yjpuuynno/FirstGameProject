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
    public LayerMask groundLayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        ChkGroundAngle(RayHit());
    }
    RaycastHit2D RayHit()
    {
        RaycastHit2D nowHit;
        RaycastHit2D hitDown = Physics2D.Raycast(chkPos.position,Vector2.down,distance,groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(chkPos.position,Vector2.right,distance,groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(chkPos.position,Vector2.left,distance,groundLayer);
        if(hitRight && groundAngle >= 90){
            nowHit = hitRight;
        }else if(hitLeft && groundAngle >= 90)
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
        perp = Vector2.Perpendicular(hit.normal).normalized;

        Debug.DrawLine(hit.point,hit.point+hit.normal,Color.red);
        Debug.DrawLine(hit.point,hit.point+perp,Color.red);
        Debug.DrawLine(chkPos.position,hit.point,Color.blue);
    }
}
