using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        if (collision.gameObject.tag == "Player")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnTargetHit();
            }
        
Debug.Log(collision.gameObject.name);
           
            Destroy(gameObject);
        }
    }
    
}
