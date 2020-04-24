using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D balancer;
    public GameObject target; 
    public GameObject character;
    public float speed;
    public float offset;

    private bool facingRight = true;
    public float rotationForceOffset;

    private Rigidbody2D pot; 
    // Start is called before the first frame update
    void Start()
    {
        pot = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            if(Input.mousePosition.x < playerScreenPoint.x - offset)
            {
                MoveLeft();
            }

            if (Input.mousePosition.x > playerScreenPoint.x + offset)
            {
                MoveRight();
            }
        }
    }

    public void SetSpeed(int level)
    {
        if (level == 0)
        {
            speed = 19;
        } else if (level == 1)
        {
            speed = 21;
        } else if (level == 2)
        {
            speed = 23;
        }
        else
        {
            speed = 25;
        }
    }

    private void MoveLeft()
    {
        if (facingRight)
        {
            Flip();
        }

        pot.velocity += Vector2.left * speed * Time.deltaTime;
    }

    private void MoveRight()
    {
        if (!facingRight)
        {
            Flip();
        }
        pot.velocity += Vector2.right * speed * Time.deltaTime;
    }


    private void Flip()
    {
        Vector3 scale = character.transform.localScale;
        scale.x *= -1;
        character.transform.localScale = scale;
        facingRight = !facingRight;
    }
}
