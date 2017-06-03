using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {

    protected override void OnRabbitHit(HeroRabbit rabbit)
    {
        rabbit.isBig = true;
        rabbit.transform.localScale += new Vector3(0.5f, 0.5f);
        this.CollectedHide();
    }
}
