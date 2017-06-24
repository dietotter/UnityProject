using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHere : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {

        HeroRabbit rabbit = collider.GetComponent<HeroRabbit>();

        if (rabbit != null)
        {
            if(rabbit.isBig)
                rabbit.DecreaseRabbitSize();
            LevelController.current.onRabitDeath(rabbit);
        }
    }
}
