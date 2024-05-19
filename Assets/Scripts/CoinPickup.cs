using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int scoreAmount = 100;

    [SerializeField] AudioClip coinPickupSFX;

    bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") && !pickedUp)          
        {           
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameManager>().ChangeScore(scoreAmount);
            Debug.Log("Test");
            pickedUp = true;
        }

    }


}
