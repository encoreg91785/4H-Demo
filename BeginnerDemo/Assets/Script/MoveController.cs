using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {
    Rigidbody2D rg;
    Collider2D cl;
    Animator ani;
    bool onGround = true;
    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && onGround)
        {
            rg.AddForce(Vector2.up * 150);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * Time.deltaTime * 2;
            ani.SetBool("Run", true);
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * 2;
            ani.SetBool("Run", true);
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) ani.SetBool("Run", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onGround = true;
        ani.SetBool("Jump", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ani.SetBool("Jump", true);
        onGround = false;
    }
}
