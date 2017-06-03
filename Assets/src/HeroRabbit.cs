using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HeroRabbit : MonoBehaviour {

    public int speed = 1;

    bool isGrounded = true;

    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;

    // this object's parent's GameObject
    private Transform heroParent = null;

    // rabbit becomes big after eating mushroom
    public bool isBig = false;

    public bool isAlive = true;

    // time when rabbit can't die from bombs
    public float invulnerableTime = 0;

    // Use this for initialization
    void Start ()
	{
        // save initial rabbit position
	    LevelController.current.setStartPosition(transform.position);
        // save standard parent's GameObject
	    this.heroParent = this.transform.parent;
	}
	
	// Update is called once per frame
	void Update ()
	{

	    Animator animator = GetComponent<Animator>();

	    if (isAlive)
	    {
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
        // === DEATH ANIMATION ===
	    else
	    {
	        animator.SetBool("die", true);

            // kostyli probably
	        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DeathAnim"))
	        {
	            animator.SetBool("die", false);
	        }

            // check if death animation has finished
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("IdleAnim"))
	        {
                LevelController.current.onRabitDeath(this);
	            animator.SetBool("die", false);
            }
	    }
	}

    void FixedUpdate()
    {
        // rabbit timers
        rabbitTimers();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Rigidbody2D myBody = GetComponent<Rigidbody2D>();

        // === MOVEMENT ===
        if (Input.GetKey(KeyCode.LeftArrow) && isAlive)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            sr.flipX = true;
        }
        if (Input.GetKey(KeyCode.RightArrow) && isAlive)
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

            // check whether we are on a moving platform
            if (hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null)
            {
                // stick to moving platform
                SetNewParent(this.transform, hit.transform);
            }
        }
        else
        {
            isGrounded = false;

            // unstick from moving platform
            SetNewParent(this.transform, this.heroParent);
        }

        Debug.DrawLine(from, to, Color.red);

        // === JUMPING ===
        // jump button was just pressed
        if (Input.GetButtonDown("Jump") && isGrounded && isAlive)
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

    static void SetNewParent(Transform obj, Transform newParent)
    {
        if (obj.transform.parent != newParent)
        {
            // save object position in global coordinates
            Vector3 pos = obj.transform.position;

            // set new parent (after this, coordinates of object will change as they are now relative to parent object)
            obj.transform.parent = newParent;

            // return obj to saved global coordinates
            obj.transform.position = pos;
        }
    }

    private void rabbitTimers()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (invulnerableTime > 0)
        {

            sr.color = Color.red;
            invulnerableTime -= Time.deltaTime;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
