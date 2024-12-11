using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTracking : MonoBehaviour
{

    // Coin
    public GameObject coin;

    // GameController
    public GameObject gameController;

    // Speed that the coin moves at
    private float speed = 1.5f;

    // Distance from player to destroy coin
    public float dstrThresh = 0.5f;
    
    // PlayerCoins script
    public PlayerCoins playerCoins;

    private Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        if (gameController == null) gameController = GameObject.FindWithTag("GameController");

        if (gameController != null)
        {
            // Debug.Log($"CM GameController EXISTS.");
            // Get PlayerCoins script
            playerCoins = gameController.GetComponent<PlayerCoins>();
        }
        else
        {
            // Debug.LogError("CM GameController NULL!");
            return;
        }

        if (playerCoins != null)
        {
            // Debug.Log($"CM playerCoins EXISTS.");
        }
        else
        {
            // Debug.LogError("CM playerCoins NULL!");
            return;
        }

        if (playerCam == null) playerCam = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        // Move coin towards player
        coin.transform.position = Vector3.MoveTowards(coin.transform.position, playerCam.position, speed * Time.fixedDeltaTime);

        // Debug.Log(Vector3.Distance(coin.transform.position, playerCam.position));

        // Destroy coin once it reaches player
        if (Vector3.Distance(coin.transform.position, playerCam.position) <= dstrThresh)
        {

            if (playerCoins != null)
            {
                playerCoins.EarnCoins(1);
                // Debug.Log("CM Coin collected! Earned 1 coin.");
            }
            else
            {
                // Debug.LogError("CM playerCoins NULL!");
            }

            Destroy(coin);

            return;
        }
    }
}
