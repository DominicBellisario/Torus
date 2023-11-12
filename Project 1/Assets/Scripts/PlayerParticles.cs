using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField]
    MovementController movementController;

    //engine particles
    [SerializeField] 
    ParticleSystem leftP;
    [SerializeField]
    ParticleSystem rightP;
    [SerializeField]
    ParticleSystem upP;
    [SerializeField]
    ParticleSystem downP;

    //boost particles
    [SerializeField]
    ParticleSystem leftB;
    [SerializeField]
    ParticleSystem rightB;
    [SerializeField]
    ParticleSystem upB;
    [SerializeField]
    ParticleSystem downB;

    //zero particles
    [SerializeField]
    ParticleSystem leftZ;
    [SerializeField]
    ParticleSystem rightZ;
    [SerializeField]
    ParticleSystem upZ;
    [SerializeField]
    ParticleSystem downZ;

    private int offset = 6;

    private Vector3 playerDirection = Vector3.zero;

    private List<ParticleSystem> engineParticles = new List<ParticleSystem>();

    private List<ParticleSystem> boostParticles = new List<ParticleSystem>();

    private List<ParticleSystem> zeroParticles = new List<ParticleSystem>();

    private List<ParticleSystem> enemyDamagedParticles = new List<ParticleSystem>();


    // Start is called before the first frame update
    void Start()
    {
        //adds particles to their lists
        engineParticles.Add(leftP);
        engineParticles.Add(rightP);
        engineParticles.Add(upP);
        engineParticles.Add(downP);
        boostParticles.Add(leftB);
        boostParticles.Add(rightB);
        boostParticles.Add(upB);
        boostParticles.Add(downB);
        zeroParticles.Add(leftZ);
        zeroParticles.Add(rightZ);
        zeroParticles.Add(upZ);
        zeroParticles.Add(downZ);

        //particles are not active at first
        foreach (ParticleSystem p in engineParticles)
        {
            p.Stop();
        }
        foreach (ParticleSystem b in boostParticles)
        {
            b.Stop();
        }
        foreach (ParticleSystem z in zeroParticles)
        {
            z.Stop();
        }
        foreach (ParticleSystem d in enemyDamagedParticles)
        {
            d.Stop();
        }
    }

    private void Update()
    {
        //lock particle x/y pos to player
        SetHorizontalPartcile(leftP, -1);
        SetHorizontalPartcile(rightP, 1);
        SetVerticalPartcile(upP, 1);
        SetVerticalPartcile(downP, -1);

        SetHorizontalPartcile(leftB, -1);
        SetHorizontalPartcile(rightB, 1);
        SetVerticalPartcile(upB, 1);
        SetVerticalPartcile(downB, -1);

        SetHorizontalPartcile(leftZ, -1);
        SetHorizontalPartcile(rightZ, 1);
        SetVerticalPartcile(upZ, 1);
        SetVerticalPartcile(downZ, -1);

        //stop all zero particles if player is not moving
        if (movementController.Velocity.x < .1 && movementController.Velocity.x > -.1 && movementController.Velocity.y < .1 && movementController.Velocity.y > -.1)
        {
            foreach (ParticleSystem z in zeroParticles)
            {
                z.Stop();
            }
        }

        //when an enemy damaged particle finishes its cycle, remove it
        foreach (ParticleSystem d in enemyDamagedParticles)
        {
            if (!d.isPlaying)
            {
                enemyDamagedParticles.Remove(d);
                Destroy(d);
            }
        }
    }

    //activate particles when the player moves
    public void OnMove(Vector2 direction)
    {
        playerDirection = direction;
        //left thruster
        if (playerDirection.x > 0)
        {
            leftP.Play();
        }
        else
        {
            leftP.Stop();
        }

        //right thruster
        if (playerDirection.x < 0)
        {
            rightP.Play();
        }
        else
        {
            rightP.Stop();
        }

        //up thruster
        if (playerDirection.y < 0)
        {
            upP.Play();
        }
        else
        {
            upP.Stop();
        }

        //down thruster
        if (playerDirection.y > 0)
        {
            downP.Play();
        }
        else
        {
            downP.Stop();
        }
    }

    public void OnZero(Vector3 velocity)
    {
        //left thruster
        if (velocity.x < -.1)
        {
            leftZ.Play();
        }
        else
        {
            leftZ.Stop();
        }

        //right thruster
        if (velocity.x > .1)
        {
            rightZ.Play();
        }
        else
        {
            rightZ.Stop();
        }

        //up thruster
        if (velocity.y > .1)
        {
            upZ.Play();
        }
        else
        {
            upZ.Stop();
        }

        //down thruster
        if (velocity.y < -.1)
        {
            downZ.Play();
        }
        else
        {
            downZ.Stop();
        }
    }

    //activates boost particles when player boosts.
    public void BoostEffect()
    {
        //left thruster
        if (playerDirection.x > 0)
        {
            leftB.Play();
        }

        //right thruster
        if (playerDirection.x < 0)
        {
            rightB.Play();
        }

        //up thruster
        if (playerDirection.y < 0)
        {
            upB.Play();
        }

        //down thruster
        if (playerDirection.y > 0)
        {
            downB.Play();
        }
    }

    //set a left or right particle to the player
    private void SetHorizontalPartcile(ParticleSystem particle, int direction)
    {
        particle.transform.position = new Vector3(transform.position.x + (offset * direction),
            transform.position.y,
            transform.position.z);
    }

    //set an up or down particle to the player
    private void SetVerticalPartcile(ParticleSystem particle, int direction)
    {
        particle.transform.position = new Vector3(transform.position.x,
            transform.position.y + (offset * direction),
            transform.position.z);
    }
}
