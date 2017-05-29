using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    // [0-1], 0 - background is still as platforms; 1 - background moves as rabbit
    public float slowdown = 0.5f;

    Vector3 lastPosition;

    void Awake()
    {
        lastPosition = Camera.main.transform.position;
    }
	
	void LateUpdate ()
	{
	    Vector3 new_position = Camera.main.transform.position;
	    Vector3 diff = new_position - lastPosition;
	    lastPosition = new_position;

	    Vector3 my_pos = this.transform.position;
        // moving background in camera direction but with different speed
	    my_pos += slowdown * diff;
	    this.transform.position = my_pos;
	}
}
