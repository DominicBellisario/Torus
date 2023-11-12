using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class BoostUpdate : MonoBehaviour
{
    //used to get boost timer value
    [SerializeField]
    MovementController movementController;
    
    //the boostbar
    [SerializeField]
    SpriteRenderer boostBar;

    //color the bar will show when at full charge
    [SerializeField]
    Color chargedColor;

    //color the bar will show when charging
    [SerializeField]
    Color spentColor;

    private float prevTimer;
   
    private Vector3 maxScale;
    private Vector3 basePosition;

    // Start is called before the first frame update
    private void Start()
    {
        //gets initial values of bar
        prevTimer = movementController.Timer;
        maxScale = boostBar.transform.localScale;
        basePosition = boostBar.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //only updates length when it changes (to save on cpu)
        if (prevTimer != movementController.Timer)
        {
            //calculates boostBar length
            double xScale = maxScale.x * movementController.Timer;

            //change the length of the boostBar
            boostBar.transform.localScale = new Vector3((float)xScale, maxScale.y, maxScale.z);

            //change the position of the boostBar so it is on the left
            boostBar.transform.position = new Vector3
                ((float)(basePosition.x - (maxScale.x - xScale) / 2), basePosition.y, basePosition.z);
        }

        //bar is blue when fully charged, white when not
        if (boostBar.transform.localScale.x >= maxScale.x)
        {
            boostBar.color = chargedColor;
        }
        else
        {
            boostBar.color = spentColor;
        }
    }
}
