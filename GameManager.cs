using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform startPoint;
    public Text scoreText;
    public Text clearText;

   
    private int score = 5;
    private int  remainingScore ;
    
    private bool isPlayerReady = true;
    void Start()
    {
        remainingScore = score;
        ResetPlayer(); 
        UpdateScoreUI();
        clearText.gameObject.SetActive(false);
    }

    public void OnPlayerLaunched()
    {
        isPlayerReady = false; // 플레이어 발사
        Invoke(nameof(ResetPlayer), 1.5f); // 초기화 
    }
    private void ResetPlayer()
    {
        player.transform.position = startPoint.position;
        player.transform.rotation = Quaternion.identity;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; 
            rb.angularVelocity = 0f; 
            rb.isKinematic = true; 
        }

        MPlayerControl playerControl = player.GetComponent<MPlayerControl>();
        if (playerControl != null)
        {
            playerControl.ResetPlayer(); 
        }

        isPlayerReady = true; // 플레이어가 준비됨
    }
    public void OnTargetHit()
    {
        remainingScore--;
        UpdateScoreUI();
        if (remainingScore <= 0)
        {
            ClearGame();
        }
      
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{remainingScore}/{score}";
        }
    }

    private void ClearGame()
    {
        clearText.gameObject.SetActive(true);
        clearText.text = "게임 종료!";
        Invoke(nameof(QuitGame), 2.0f); 
        
    }
    private void QuitGame()
    {
        Application.Quit(); 
    }
    
}
