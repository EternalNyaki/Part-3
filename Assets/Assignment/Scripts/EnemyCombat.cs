using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : DestructibleObject
{
    public float speed = 3f;

    public float attackDuration = 0.8f;
    public float attack2Duration = 0.8f;
    public float hitStun = 1.5f;
    public float deathDuration = 0.6f;

    private Transform target;

    public Hitbox detectionBox;

    public int attackDamage = 10;
    public LayerMask attackLayerMask;

    public Hitbox[] attack1Hitboxes;
    public Hitbox[] attack2Hitboxes;

    private float movement = 0f;

    private bool disableMovement = false;
    private Coroutine interruptAction;

    private Animator animator;
    private Rigidbody2D rb;

#if UNITY_EDITOR
    public int debugAttack;
    public int debugAttackFrame;
#endif

    // Start is called before the first frame update
    protected override void Initialize()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(movement, 0);
        rb.MovePosition(rb.position + movementVector * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) disableMovement = true;

        if (!disableMovement)
        {
            if((target.position.x - transform.position.x) * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            if (Physics2D.OverlapBox((Vector2)transform.position + detectionBox.point * transform.localScale.x, detectionBox.size, 0f, attackLayerMask) != null)
            {
                interruptAction = StartCoroutine(Attack());
            }
        }

        movement = disableMovement ? 0f : transform.localScale.x / Mathf.Abs(transform.localScale.x);
        animator.SetBool("movement", !disableMovement);
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("attack");
        disableMovement = true;
        List<GameObject> objectsHit = new List<GameObject>();
        float timer = 0;
        bool targetHit = false;
        while (timer < attackDuration)
        {
            int index = (int)(timer / attackDuration * attack1Hitboxes.Length);
            Collider2D[] collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + attack1Hitboxes[index].point * transform.localScale.x, attack1Hitboxes[index].size, attack1Hitboxes[index].angle, attackLayerMask);
            foreach (Collider2D collision in collisions)
            {
                if (!objectsHit.Contains(collision.gameObject))
                {
                    collision.gameObject.SendMessage("TakeDamage", attackDamage);
                    objectsHit.Add(collision.gameObject);
                    if(!targetHit)
                    {
                        targetHit = true;
                        animator.SetTrigger("attack");
                    }
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
        disableMovement = false;
        if(targetHit)
        {
            interruptAction = StartCoroutine(Attack2());
        }
    }

    private IEnumerator Attack2()
    {
        animator.SetTrigger("attack");
        disableMovement = true;
        List<GameObject> objectsHit = new List<GameObject>();
        float timer = 0;
        while (timer < attack2Duration)
        {
            int index = (int)(timer / attack2Duration * attack2Hitboxes.Length);
            Collider2D[] collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + attack2Hitboxes[index].point * transform.localScale.x, attack2Hitboxes[index].size, attack2Hitboxes[index].angle, attackLayerMask);
            foreach (Collider2D collision in collisions)
            {
                if (!objectsHit.Contains(collision.gameObject))
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
        animator.SetBool("stun", true);
        disableMovement = true;
        float timer = 0;
        while (timer < hitStun)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("stun", false);
        disableMovement = false;
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("die");
        disableMovement = true;
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(!dead)
        {
            if (interruptAction != null) StopCoroutine(interruptAction);
            interruptAction = StartCoroutine(Hurt());
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(Death());
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(detectionBox.point, detectionBox.size);

        Hitbox hitbox;
        switch (debugAttack)
        {
            case 0:
                hitbox = attack1Hitboxes[debugAttackFrame];
                break;

            case 1:
                hitbox = attack2Hitboxes[debugAttackFrame];
                break;

            default:
                hitbox = attack1Hitboxes[debugAttackFrame];
                break;
        }

        Gizmos.color = Color.red;
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, hitbox.angle));
        Gizmos.matrix = transform.localToWorldMatrix * rotation;

        Gizmos.DrawWireCube(hitbox.point, hitbox.size);
    }
#endif
}
