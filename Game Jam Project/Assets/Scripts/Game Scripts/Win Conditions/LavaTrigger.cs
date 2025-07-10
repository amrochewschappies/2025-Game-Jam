using System;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public GameManager _gameManger;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject hitPlayer = other.gameObject;
            
            if (_gameManger != null)
            {
                _gameManger.CheckDeath(hitPlayer);
                Debug.Log("Loading Back to start Scene");
            }
            else
            {
                Debug.LogError("GameManager is not assigned properly.");
            }
        }
    }
    
    
    
}
