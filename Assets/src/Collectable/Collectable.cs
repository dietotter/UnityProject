using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected virtual void OnRabbitHit(HeroRabbit rabbit)
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabbit rabbit = collider.GetComponent<HeroRabbit>();
        if (rabbit != null)
        {
            this.OnRabbitHit(rabbit);
        }
    }

    public void CollectedHide()
    {
        Destroy(this.gameObject);
    }
}
