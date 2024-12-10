using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTracking : MonoBehaviour
{

    // Coins
    public GameObject coin;

    // Player
    public GameObject player;

    // Speed that the coin moves at
    public float speed;

    // Distance from player to destroy coin
    public float dstrThresh = 0.5f;
    
    // PlayerCoins script
    private PlayerCoins playerCoins;


    // Start is called before the first frame update
    void Start()
    {
        // Get PlayerCoins script
        playerCoins = player.GetComponent<PlayerCoins>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move coin towards player
        coin.transform.position = Vector3.MoveTowards(coin.transform.position, player.transform.position, speed);

        // Destroy coin once it reaches player
        if (Vector3.Distance(coin.transform.position, player.transform.position) <= dstrThresh)
        {
            Destroy(coin);

            playerCoins.EarnCoins(1);
        }
    }
}
