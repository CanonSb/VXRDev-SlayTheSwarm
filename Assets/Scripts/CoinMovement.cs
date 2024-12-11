using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTracking : MonoBehaviour
{

    // Coin
    public GameObject coin;

    // XR Rig
    public GameObject xrRig;

    // GameController
    public GameObject player;

    // Speed that the coin moves at
    public float speed;

    // Distance from player to destroy coin
    public float dstrThresh = 0.5f;
    
    // PlayerCoins script
    public PlayerCoins playerCoins;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.FindWithTag("GameController");

        if (player != null)
        {
            Debug.Log($"CM GameController EXISTS.");
            // Get PlayerCoins script
            playerCoins = player.GetComponent<PlayerCoins>();
        }
        else
        {
            Debug.LogError("CM GameController NULL!");
            return;
        }

        if (playerCoins != null)
        {
            Debug.Log($"CM playerCoins EXISTS.");
        }
        else
        {
            Debug.LogError("CM playerCoins NULL!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move coin towards player
        coin.transform.position = Vector3.MoveTowards(coin.transform.position, xrRig.transform.position, speed * Time.fixedDeltaTime);

        Debug.Log(Vector3.Distance(coin.transform.position, xrRig.transform.position));

        // Destroy coin once it reaches player
        if (Vector3.Distance(coin.transform.position, xrRig.transform.position) <= dstrThresh)
        {

            if (playerCoins != null)
            {
                playerCoins.EarnCoins(1);
                Debug.Log("CM Coin collected! Earned 1 coin.");
            }
            else
            {
                Debug.LogError("CM playerCoins NULL!");
            }

            Destroy(coin);

            return;
        }
    }
}
