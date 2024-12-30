using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPlayerControl : MonoBehaviour
{
    public Rigidbody2D rb; 
    public Transform pivotPoint; 
    public LineRenderer lineRenderer;
    public GameObject pointPrefab; 
    public float maxStretch = 3f; 
    public float maxPower = 20f;
    public Slider powerSlider; 
    public int pointCount = 10; 
    public float simulationStep = 0.1f; 

    private List<GameObject> points = new List<GameObject>();
    private bool isDragging = false; 
    private Vector2 dragDirection;
    private float currentPower = 0f; 
    private bool isLaunched = false; 

    public GameManager gameManager;

    public AudioClip jumpSound;
    private AudioSource audioSource;
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
      
        for (int i = 0; i < pointCount; i++)
        {
            GameObject point = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
            point.SetActive(false); 
            points.Add(point);
        }

        ResetPlayer();
    }

    void Update()
    {
        if (!isLaunched)
        {
            HandleMouseInput();
            UpdateLineRenderer();
            UpdatePoints();
            UpdatePowerUI();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging) 
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

           
            Vector2 pullDirection = mousePosition - (Vector2)pivotPoint.position;
            float pullDistance = Mathf.Clamp(pullDirection.magnitude, 0, maxStretch);
            dragDirection = pullDirection.normalized;
            currentPower = Mathf.Clamp(pullDistance / maxStretch * maxPower, 0, maxPower);
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if (isDragging)
            {
                isDragging = false;
                LaunchPlayer();
            }
        }
    }

    private void LaunchPlayer()
    {
        if (rb != null)
        {
            Vector2 launchForce = dragDirection * currentPower;
            rb.isKinematic = false; 
            rb.AddForce(launchForce, ForceMode2D.Impulse);
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
            isLaunched = true;
            
            if (gameManager != null)
            {
                gameManager.OnPlayerLaunched();
            }
            
            foreach (GameObject point in points)
            {
                point.SetActive(false);
            }
        }
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer != null && isDragging)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, pivotPoint.position); 
            Vector2 stretchedPosition = (Vector2)pivotPoint.position + (dragDirection * Mathf.Clamp(currentPower / maxPower * maxStretch, 0, maxStretch));
            lineRenderer.SetPosition(1, stretchedPosition); 
        }
        else if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; 
        }
    }

    private void UpdatePoints()
    {
        if (isDragging)
        {
            Vector2 startPosition = (Vector2)pivotPoint.position;
            Vector2 initialVelocity = dragDirection * currentPower;

            for (int i = 0; i < points.Count; i++)
            {
                float t = i * simulationStep; 
                Vector2 position = startPosition + initialVelocity * t + Physics2D.gravity * (0.5f * (t * t)); 
                points[i].transform.position = position;
                points[i].SetActive(true); 
            }
        }
        else
        {
            foreach (GameObject point in points)
            {
                point.SetActive(false);
            }
        }
    }

    private void UpdatePowerUI()
    {
        if (powerSlider != null)
        {
            powerSlider.value = currentPower / maxPower;
        }
    }

    public void ResetPlayer()
    {
       
        isLaunched = false;
        currentPower = 0f;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true; 
        }

        transform.position = pivotPoint.position; 
        transform.rotation = Quaternion.identity;
    }
}
