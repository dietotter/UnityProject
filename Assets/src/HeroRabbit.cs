using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class HeroRabbit : MonoBehaviour {

    public int speed = 1;

    bool isGrounded = true;

    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;

	// Use this for initialization
	void Start ()
	{
        // save initial rabbit position
	    LevelController.current.setStartPosition(transform.position);
	}
	
	// Update is called once per frame
	void Update ()
	{

	    Animator animator = GetComponent<Animator>();

	    // === RUN ANIMATION ===
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
	    {
	        animator.SetBool("run", true);
	    }
	    else
	    {
	        animator.SetBool("run", false);
	    }

        // === JUMP ANIMATION ===
	    if (this.isGrounded)
	    {
	        animator.SetBool("jump", false);
	    }
	    else
	    {
	        animator.SetBool("jump", true);
        }
	}

    void FixedUpdate()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Rigidbody2D myBody = GetComponent<Rigidbody2D>();

        // === MOVEMENT ===
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            sr.flipX = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            sr.flipX = false;
        }

        // === IS GROUNDED CHECK ===
        Vector3 from = transform.position + Vector3.up * 0.3f;
        Vector3 to = transform.position + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");

        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);

        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Debug.DrawLine(from, to, Color.red);

        // === JUMPING ===
        // jump button was just pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.JumpActive = true;
        }
        
        if (this.JumpActive)
        {
            // if jump button is still being held
            if (Input.GetButton("Jump"))
            {
                this.JumpTime += Time.deltaTime;
                if (this.JumpTime < this.MaxJumpTime)
                {
                    Vector2 vel = myBody.velocity;
                    vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                    myBody.velocity = vel;
                }
            }
            else
            {
                this.JumpActive = false;
                this.JumpTime = 0;
            }
        }
    }
}
