using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float size;
    public int moveDirection;
    public float moveSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.isGameRunning)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            //Destroy Fish out of Bounds.
            if ((moveDirection < 0 && transform.position.x < -GameManager.Instance.boundsX) || (moveDirection > 0 && transform.position.x > GameManager.Instance.boundsX))
                Destroy(gameObject);
        }
    }
}
