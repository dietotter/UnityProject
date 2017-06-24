using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour {

    public float carrotLife = 3;
    public float carrotSpeed = 2;

    private float direction;

    public bool isLaunched = false;

	void Update () {
	    if (isLaunched)
	    {
	        transform.Translate(new Vector3(direction, 0) * carrotSpeed * Time.deltaTime);
	    }
	}

    public void Launch(float direction)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        this.direction = direction;
        if (direction == -1)
        {
            sr.flipX = true;
            bc.offset = new Vector2(bc.offset.x * -1, bc.offset.y);
        }
        else
        {
            sr.flipX = false;
        }

        isLaunched = true;

        StartCoroutine(dieLater());
    }

    IEnumerator dieLater()
    {
        yield return new WaitForSeconds(carrotLife);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabbit rabbit = collider.GetComponent<HeroRabbit>();

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
        }
    }
}
