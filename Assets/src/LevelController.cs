using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public static LevelController current;
    Vector3 startingPosition;

    public void setStartPosition(Vector3 pos)
    {
        this.startingPosition = pos;
    }

    public void onRabitDeath(HeroRabbit rabbit)
    {
        // on death return rabbit to initial position
        rabbit.transform.position = this.startingPosition;
    }

    // Awake() invokes before Start()
    void Awake()
    {
        current = this;
    }
}
