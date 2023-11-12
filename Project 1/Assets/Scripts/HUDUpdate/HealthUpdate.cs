using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthUpdate : MonoBehaviour
{
    //the player's health
    [SerializeField]
    PlayerInfo playerInfo;

    //the panel that will be edited
    [SerializeField]
    UnityEngine.UI.Image panel;

    //the healthbar
    [SerializeField]
    SpriteRenderer healthBar;

    private Vector3 maxScale;
    private Vector3 basePosition;

    //strength the screen pulses when at one life
    [SerializeField]
    float pulseStrength;

    //how fast the screen pulses when at one life
    [SerializeField]
    float pulseSpeed;

    //player health
    private int health;

    //the x of the sin curve for low health effect
    private double timer;

    //the restart button, invisible until player dies
    [SerializeField]
    GameObject restartButton;

    private void Start()
    {
        //set initial values
        maxScale = healthBar.transform.localScale;
        basePosition = healthBar.transform.position;
    }

    void Update()
    {
        //~~~~delete when player does not dissapear upon death
        if (playerInfo != null)
        {
            health = playerInfo.Health;
        }
        else
        {
            health = 0;
        }

        //attempts to decrease the opacity of the panel if not clear
        if (panel.color.a != 0 && health > 20)
        {
            Color prevColor = panel.color;
            panel.color = new Color(prevColor.r, prevColor.g, prevColor.b, 
                prevColor.a - 1 * Time.deltaTime);
        }
        

        //make the screen slightly red when at 20% life and begin to pulse
        if (health <= 20 && health > 0)
        {
            //begin to increment timer
            timer += Math.PI * Time.deltaTime * pulseSpeed;

            //uses a sin curve to make a pulse effect with the low health effect
            float alpha = (float)(Math.Abs(Math.Sin(timer)) * pulseStrength * 0.1); 

            //creates a new color with this alpha
            panel.color = new Color(1, 0, 0, alpha);
        }
        //increase red and make restart button visible when dead
        else if (health <= 0)
        {
            health = 0;
            panel.color = new Color(1, 0, 0, 0.5f);

            restartButton.SetActive(true);
        }

        
    }


    /// <summary>
    /// updates health by the determined amount
    /// </summary>
    /// <param name="score"></param>
    public void UpdateHealth(int healthUpdate)
    {
        //health cannot be less than 0
        if (health + healthUpdate < 0)
        {
            playerInfo.Health = 0;
            health = 0;
        }
        //health cannot be greater than 100 (max)
        else if (health + healthUpdate > 100)
        {
            playerInfo.Health = 100;
            health = playerInfo.Health;
        }
        //otherwise, update normaly
        else
        {
            playerInfo.Health += healthUpdate;
            health = playerInfo.Health;
        }

        //calculates healthbar length
        double xScale = maxScale.x / (100 / (health + 0.1));

        //change the length of the healthbar
        healthBar.transform.localScale = new Vector3((float)xScale, maxScale.y, maxScale.z);

        //change the position of the healthbar so the green is on the left
        healthBar.transform.position = new Vector3
            ((float)(basePosition.x - (maxScale.x - xScale) / 2), basePosition.y, basePosition.z);

        //the screen flashes red if the player loses health
        if (healthUpdate < 0)
        {
            panel.color = new Color(1, 0, 0, 0.3f);
        }
        //the screen flashes green if the player gains health
        else if (healthUpdate > 0)
        {
            panel.color = new Color(0, 1, 0, 0.2f);
        }
    }
}
