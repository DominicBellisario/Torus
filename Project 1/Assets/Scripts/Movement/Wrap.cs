using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Wrap : MonoBehaviour
{
    private int leftWrap = -160;
    private int rightWrap = 505;
    private int topWrap = 310;
    private int bottomWrap= -110;

    private Vector3 objectPos = Vector3.zero;

    void Update()
    {
        objectPos = transform.position;

        //left side
        if (objectPos.x < leftWrap)
        {
            objectPos = new Vector3(rightWrap - 1, objectPos.y, objectPos.z);
        }
        //right side
        else if (objectPos.x > rightWrap)
        {
            objectPos = new Vector3(leftWrap + 1, objectPos.y, objectPos.z);
        }
        //bottom
        else if (objectPos.y > topWrap)
        {
            objectPos = new Vector3(objectPos.x, bottomWrap + 1, objectPos.z);
        }
        //top
        else if (objectPos.y < bottomWrap)
        {
            objectPos = new Vector3(objectPos.x, topWrap - 1, objectPos.z);
        }
        
        transform.position = objectPos;
    }
}
