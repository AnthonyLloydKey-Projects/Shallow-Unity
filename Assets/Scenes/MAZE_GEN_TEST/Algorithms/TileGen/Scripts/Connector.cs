using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [Range(1, 10)]
    public  float doorScale = 4f;
    Vector2 doorSize = Vector2.one;
    
   
    public bool isConnected = false;

    void OnDrawGizmos()
    {
        //size *= sizeS;
        Vector2 size = doorSize * doorScale;

        Gizmos.color = Color.green;

        Vector3 halfSize = size * 0.5f;
        Vector3 offset = transform.position + transform.up * halfSize.x;

        Gizmos.DrawLine(offset, offset + transform.forward);

        Vector3 top = transform.up * size.y;
        Vector3 side = transform.right * halfSize.y;

        Vector3 tR = transform.position + top + side;
        Vector3 tL = transform.position + top - side;
        Vector3 bR = transform.position + side;
        Vector3 bL = transform.position - side;

        
        Gizmos.DrawLine(tR, tL);
        Gizmos.DrawLine(tL, bL);
        Gizmos.DrawLine(bL, bR);
        Gizmos.DrawLine(bR, tR);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(tL, 0.1f);
        Gizmos.DrawSphere(tR, 0.1f);
        Gizmos.DrawSphere(bL, 0.1f);
        Gizmos.DrawSphere(bR, 0.1f);

    }

}
