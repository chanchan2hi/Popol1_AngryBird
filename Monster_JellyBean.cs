using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_JellyBean : MonoBehaviour
{
    private float moveSpeed = 0.5f;
    private bool isDown = true;
    void Update()
    {
        if (isDown)
        {
            transform.Translate(Vector3.down * (Time.deltaTime * moveSpeed));
        }
        else
        {
            transform.Translate(Vector3.up * (Time.deltaTime * moveSpeed));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isDown = !isDown;
        Debug.Log("Trigger");
    }
}
