using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bomb : Collectable {

    protected override void OnRabbitHit(HeroRabbit rabbit)
    {

        if (rabbit.invulnerableTime <= 0)
        {
            if (rabbit.isBig)
            {
                rabbit.DecreaseRabbitSize();
                rabbit.invulnerableTime = 4;
            }
            else
            {
                rabbit.isAlive = false;
            }

            this.CollectedHide();
        }
    }
}
