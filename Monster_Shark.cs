using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Shark : MonoBehaviour
{
    public float speed = 2f;
    private bool movingLefh = true;// 이동 방향 
    private SpriteRenderer sr;
 
    void Start()
    {
       sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (movingLefh)
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }
        else
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        movingLefh = !movingLefh;
        sr.flipX = !sr.flipX;
    }
}
