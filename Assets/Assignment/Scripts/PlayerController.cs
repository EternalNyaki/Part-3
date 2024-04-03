using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Hitbox
{
    public Vector2 point;
    public Vector2 size;
    public float angle;
}

public class PlayerController : DestructibleObject
{
    public float speed = 5f;

    public float attackDuration = 1f;
    public float hitStun = 0.3f;
    public float deathDuration = 1f;

    public int attackDamage = 10;
    public LayerMask attackLayerMask;

    public Hitbox[] attackHitboxes;

    private float input;

    private bool disableMovement = false;
    private Coroutine interruptAction;

    private Animator animator;
    private Rigidbody2D rb;

#if UNITY_EDITOR
    public int debugAttackFrame;
#endif

    // Start is called before the first frame update
    protected override void Initialize()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (disableMovement) return;

        Vector2 inputVector = new Vector2(input, 0);
        rb.MovePosition(rb.position + inputVector * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableMovement)
        {
            input = Input.GetAxis("Horizontal");

            if(-input * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            if(Input.GetKeyDown(KeyCode.J))
            {
                interruptAction = StartCoroutine(Attack());
            }

            animator.SetFloat("movement", Mathf.Abs(input));
        }
        else
        {
            animator.SetFloat("movement", 0f);
        }
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("attack");
        disableMovement = true;
        List<GameObject> objectsHit = new List<GameObject>();
        float timer = 0;
        while(timer < attackDuration)
        {
            int index = (int)(timer / attackDuration * attackHitboxes.Length);
            Collider2D[] collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + attackHitboxes[index].point, attackHitboxes[index].size, attackHitboxes[index].angle, attackLayerMask);
            //Collder2D[] collisions = Physics2D.OverlapBoxAll((Vector2)(transform.position + transform.forward * 2), new Vector2(5, 5), 0f);
            foreach(Collider2D collision in collisions)
            {
                if(!objectsHit.Contains(collision.gameObject))
                {
                    collision.gameObject.SendMessage("TakeDamage", attackDamage);
                    objectsHit.Add(collision.gameObject);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
        disableMovement = false;
    }

    private IEnumerator Hurt()
    {
        animator.SetTrigger("hurt");
        disableMovement = true;
        float timer = 0;
        while(timer < hitStun)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        disableMovement = false;
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("die");
        disableMovement = true;
        float timer = 0;
        while(timer < deathDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(!dead)
        {
            if(interruptAction != null) StopCoroutine(interruptAction);
            interruptAction = StartCoroutine(Hurt());
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(Death());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Hitbox hitbox = attackHitboxes[debugAttackFrame];
        Gizmos.color = Color.red;
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, hitbox.angle));
        Gizmos.matrix = transform.localToWorldMatrix * rotation;

        Gizmos.DrawWireCube(hitbox.point, hitbox.size);
    }
#endif
}
