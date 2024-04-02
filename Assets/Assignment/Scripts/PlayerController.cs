using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private float input;
    
    private Animator animator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = new Vector2(input, 0);
        rb.MovePosition(rb.position + inputVector * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxis("Horizontal");

        if(-input * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        animator.SetFloat("movement", Mathf.Abs(input));
    }
}
