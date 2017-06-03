using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Vector3 MoveBy; // by how much will the platform move from its initial position
    public float timeToWait = 1; // for how much time the platform stops when reaching the target
    public float speed = 1;

    private Vector3 pointA;
    private Vector3 pointB;

    private bool goingToA; // checks whether platforms now moves to point A or point B
    private Vector3 destination; // used for moving the platform to target direction

    void Start()
    {
        this.pointA = this.transform.position;
        this.pointB = this.pointA + MoveBy;

        goingToA = false;
        destination = pointB - pointA;
    }

    void FixedUpdate()  
    {
        
        if (timeToWait <= 0)
        {
            Vector3 myPos = this.transform.position;
            Vector3 target;
                    
            if (goingToA)
            {
                target = this.pointA;
            }
            else
            {
                target = this.pointB;
            }

            // check if platform has arrived to the current target. If yes, then set the wait time and change the current target
            if (isArrived(myPos, target))
            {
                timeToWait = 2;
                goingToA = goingToA ? false : true;

                if (goingToA)
                {
                    destination = this.pointA - this.pointB;
                    destination.z = 0;
                }
                else
                {
                    destination = this.pointB - this.pointA;
                    destination.z = 0;
                }
            }

            // move the platforms
            transform.Translate(destination * speed * Time.deltaTime);
        }
        else
        {
            // decrease wait time while platform stands in one place
            timeToWait -= Time.deltaTime;
        }
    }

    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) < 0.02f;
    }
}
