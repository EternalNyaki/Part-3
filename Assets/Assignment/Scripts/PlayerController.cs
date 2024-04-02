using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private float input;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
