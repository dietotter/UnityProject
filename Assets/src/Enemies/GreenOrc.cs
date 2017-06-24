using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOrc : Orc {

    protected override void RabbitCheck(Vector3 rabbitPos)
    {
        // if rabbit enters overwatched zone, change mode to Attack
        if (rabbitPos.x > Mathf.Min(pointA.x, pointB.x) - 1f
            && rabbitPos.x < Mathf.Max(pointA.x, pointB.x) + 1f)
        {

            if(mode != Mode.Attack) {
                previousMode = mode; // set previous mode to be which direction does rabbit go now (GoToA or GoToB)
                mode = Mode.Attack;
            }
        }
        // when rabbit is out of zone, return to previous mode
        else
        {
            mode = previousMode;
        }
    }

    protected override float RabbitAttackMovement(Vector3 rabbitPos)
    {
        float x = transform.position.x;
        float rabbitX = rabbitPos.x;

        //Move towards rabit
        if (x < rabbitX)
        {
            return 1;
        }

        return -1;
    }

}
