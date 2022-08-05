using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //movement
    public float moveSpeed;

    public float boundsX;
    public float boundsY;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    //stats
    public float size;
    public float growSize = 0.001f;
    public float defaultSize = 0.3f;
    public float maxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        size = defaultSize;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.isGameRunning)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
       var joystickAxis = playerInput.actions["Move"].ReadValue<Vector2>();

        if (joystickAxis.x > 0 && transform.position.x < boundsX)
        {
            //only move if inside bounds
            //transform.Translate(Vector3.right * joystickAxis.x * Time.deltaTime * moveSpeed);
            rb.AddForce(Vector3.right * joystickAxis.x * moveSpeed);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            spriteRenderer.flipX = false;
        }
        if (joystickAxis.x < 0 && transform.position.x > -boundsX)
        {

            //transform.Translate(Vector3.right * joystickAxis.x * Time.deltaTime * moveSpeed);
            rb.AddForce(Vector3.right * joystickAxis.x * moveSpeed);
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        if (joystickAxis.y < 0 && transform.position.y > -boundsY)
        {
            //transform.Translate(Vector3.up * joystickAxis.y * Time.deltaTime * moveSpeed);
            rb.AddForce(Vector3.up * joystickAxis.y * moveSpeed);
        }
        if (joystickAxis.y > 0  && transform.position.y < boundsY)
        {
            //transform.Translate(Vector3.up * joystickAxis.y * Time.deltaTime * moveSpeed);
            rb.AddForce(Vector3.up * joystickAxis.y * moveSpeed);
        }

        //stop adding force when max speed is hit
        if (rb.velocity.x > maxVelocity || rb.velocity.x < -maxVelocity)
        {
            var vel = rb.velocity;
            vel.x = vel.x > 0 ? maxVelocity : -maxVelocity;
            rb.velocity = vel;
        }
        if (rb.velocity.y > maxVelocity || rb.velocity.y < -maxVelocity)
        {
            var vel = rb.velocity;
            vel.y = vel.y > 0 ? maxVelocity : -maxVelocity;
            rb.velocity = vel;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.GetComponent<Fish>() != null)
        {
            var fish = obj.GetComponent<Fish>();
            if (fish.size <= size)
            {
                Grow();
                Destroy(obj);
            }
            else
            {
                Die();
                size = defaultSize;
                transform.localScale = new Vector2(size, size);
                //Destroy(gameObject);
            }   
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
        SoundModule.Instance.PlayDead();
        GameManager.Instance.GameOver(false);
    }

    private void Grow()
    {
        SoundModule.Instance.PlayBite();

        size = transform.localScale.x + growSize;
        transform.localScale = new Vector2(size, size);

        GameManager.Instance.AddScore();

        if (size > 2.7)
        {
            GameManager.Instance.GameOver(true);
        }
        
    }
}
