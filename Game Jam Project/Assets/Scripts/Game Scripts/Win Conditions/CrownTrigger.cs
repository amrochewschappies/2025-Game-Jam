using System;
using System.Collections;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private GameManager _gameManger;
    public Transform[] hexPoints; 
    public float moveDuration = 1f;
    public float moveDelay = 1f; 

    private int currentIndex = 0; 
    private void Start()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(MoveBetweenPoints());
    }

    

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            GameObject hitInfo = other.gameObject;
            if (_gameManger!= null)
            {
                _gameManger.CheckWinner(hitInfo);    
            }
            StartCoroutine(deleteCrown());
        }
    }

    IEnumerator deleteCrown()
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = hexPoints[currentIndex].position;
            
            yield return StartCoroutine(SmoothMove(startPosition, targetPosition, moveDuration));
            
            currentIndex = (currentIndex + 1) % hexPoints.Length;
            yield return new WaitForSeconds(moveDelay);
        }
    }
    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

  
        transform.position = end;
    }

}
