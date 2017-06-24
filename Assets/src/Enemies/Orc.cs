using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Orc : MonoBehaviour
{
    public float moveBy;
    protected Mode mode;
    protected Mode previousMode; // orc will return to its previous mode after attack mode

    protected Vector3 pointA;
    protected Vector3 pointB;

    protected SpriteRenderer sr;
    protected Animator animator;

    protected bool isAlive = true;

    public float speed = 1;

    public enum Mode
    {
        GoToA, GoToB, Attack
    }

    void Start()
    {
        this.pointA = this.transform.position;
        this.pointB = this.pointA + new Vector3(moveBy, 0);

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        mode = Mode.GoToB;
        previousMode = Mode.GoToB;
    }

    void Update()
    {
        animator.SetBool("walk", true);
    }

    void FixedUpdate()
    {

        if (isAlive) { 
            // rabbit's position
            Vector3 rabbitPos = HeroRabbit.lastRabbit.transform.position;
            RabbitCheck(rabbitPos);
            // === MOVEMENT ===
            OrcMovement(rabbitPos);
            // === ATTACK ===
            if(mode == Mode.Attack)
                AttackRabbit(rabbitPos);
        }
    }

    void OrcMovement(Vector3 rabbitPos)
    {
        animator.SetBool("walk", true);
        float value = this.GetDirection(rabbitPos);

        if (value > 0)
        {
            sr.flipX = true;
        }
        else if (value < 0)
        {
            sr.flipX = false;
        }
        else
        {
            animator.SetBool("walk", false);
        }

        transform.Translate(new Vector3(value, 0)*speed*Time.deltaTime);
    }

    float GetDirection(Vector3 rabbitPos)
    {
        Vector3 myPos = this.transform.position;

        if (mode == Mode.GoToA)
        {
            if (HasArrived(pointA))
            {
                mode = Mode.GoToB;
                previousMode = mode;
            }

            if (myPos.x < pointA.x)
            {
                return 1;
            }

            return -1;
        }

        if (mode == Mode.GoToB)
        {
            if (HasArrived(pointB))
            {
                mode = Mode.GoToA;
                previousMode = mode;
            }

            if (myPos.x < pointB.x)
            {
                return 1;
            }

            return -1;
        }

        if (mode == Mode.Attack)
        {
            return RabbitAttackMovement(rabbitPos);
        }

        return 0;
    }

    protected bool HasArrived(Vector3 target)
    {
        Vector3 pos = this.transform.position;
        pos.z = 0;
        target.z = 0;

        float distance = Vector3.Distance(pos, target);
        //Debug.Log(distance);

        return distance < 0.1f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabbit rabbit = collider.GetComponent<HeroRabbit>();

        if (rabbit != null)
        {

            // angle check
            Vector3 dir = collider.transform.position - transform.position;
            dir = collider.transform.InverseTransformDirection(dir);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (angle < 60 || angle > 120)
            {
                if (rabbit.invulnerableTime <= 0)
                {
                    if (rabbit.isBig)
                    {
                        animator.SetTrigger("attack");
                        rabbit.DecreaseRabbitSize();
                        rabbit.invulnerableTime = 4;
                    }
                    else
                    {
                        animator.SetTrigger("attack");
                        rabbit.isAlive = false;
                    }
                }
            }
            else
            {
                // orc dies
                animator.SetTrigger("die");
                isAlive = false;
                rabbit.TossRabbitUp();
                StartCoroutine(dieLater());
            }

        }
    }

    IEnumerator dieLater()
    {
        yield return new WaitForSeconds(GetDeathAnimationLength());
        Destroy(this.gameObject);
    }

    private float GetDeathAnimationLength()
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "DeathAnim")
            {
                return ac.animationClips[i].length;
            }
        }

        return 1;
        
    }

    protected virtual void RabbitCheck(Vector3 rabbitPos) { }
    protected virtual float RabbitAttackMovement(Vector3 rabbitPos)
    {
        return 0;
    }

    protected virtual void AttackRabbit(Vector3 rabbitPos) { }
}
