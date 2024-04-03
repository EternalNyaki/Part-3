using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : DestructibleObject
{
    public float speed = 5f;

    public float hitStun = 0.3f;
    public float deathDuration = 1f;

    private float input;

    private bool disableMovement = false;
    private Coroutine interruptAction;

    private Animator animator;
    private Rigidbody2D rb;

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

        if(Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10);
        }

        if (!disableMovement)
        {
            input = Input.GetAxis("Horizontal");

            if(-input * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            animator.SetFloat("movement", Mathf.Abs(input));
        }
        else
        {
            animator.SetFloat("movement", 0f);
        }
    }

    private IEnumerator Hurt()
    {
        disableMovement = true;
        float timer = hitStun;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        disableMovement = false;
    }

    private IEnumerator Death()
    {
        disableMovement = true;
        float timer = deathDuration;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(!dead)
        {
            animator.SetTrigger("hurt");
            if(interruptAction != null) StopCoroutine(interruptAction);
            interruptAction = StartCoroutine(Hurt());
        }
    }

    protected override void Die()
    {
        animator.SetTrigger("die");
        StopAllCoroutines();
        StartCoroutine(Death());
    }
}
