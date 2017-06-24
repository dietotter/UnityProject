using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {

    protected override void OnRabbitHit(HeroRabbit rabbit)
    {
        rabbit.IncreaseRabbitSize();
        this.CollectedHide();
    }
}
