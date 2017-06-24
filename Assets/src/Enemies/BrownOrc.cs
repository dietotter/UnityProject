using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownOrc : Orc
{

    // here orc needs to stop, change mode to attack and start throwing carrots at rabbit. Also, it should flip to the direction of rabbit
    // in AttackMovements method, return 0 if orc already faces rabbit's direction, and return -1 or 1 according to where should orc turn around (or just change sr.flipX)
    public float rabbitRadius = 4;

    public float throwInterval = 3;
    private float carrotTime = 0;

    private GameObject carrotPrefab;

    void Awake()
    {
        carrotPrefab = (GameObject)Resources.Load("Prefabs/Enemies/Carrot", typeof(GameObject));
        speed = 1.5f;
    }

    void Update()
    {
        // carrot throw cooldown
        if (carrotTime > 0)
            carrotTime -= Time.deltaTime;
    }

    protected override void RabbitCheck(Vector3 rabbitPos)
    {
        float x = transform.position.x;
        if (rabbitPos.x > (x - rabbitRadius)
            && rabbitPos.x < (x + rabbitRadius))
        {

            if (mode != Mode.Attack)
            {
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

        if (rabbitPos.x > x)
            sr.flipX = true;
        else if (rabbitPos.x < x)
            sr.flipX = false;

        return 0;
    }

    protected override void AttackRabbit(Vector3 rabbitPos)
    {
        if (carrotTime <= 0.02f)
        {
            float direction;

            if (sr.flipX)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            LaunchCarrot(direction);
        }
    }

    private void LaunchCarrot(float direction)
    {
        GameObject carrotObj = Instantiate(carrotPrefab);
        carrotObj.transform.position = transform.position + new Vector3(0, 0.5f);

        Carrot carrot = carrotObj.GetComponent<Carrot>();

        carrot.Launch(direction);

        // reset carrot throw cooldown to maximum
        carrotTime = throwInterval;
    }
}
