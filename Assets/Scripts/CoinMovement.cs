using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTracking : MonoBehaviour
{

    public GameObject coin;
    public GameObject player;
    public float speed;
    public float destroyThreshold = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move coin towards player
        coin.transform.position = Vector3.MoveTowards(coin.transform.position, player.transform.position, speed);

        // Destroy coin once it reaches player
        if (Vector3.Distance(coin.transform.position, player.transform.position) <= destroyThreshold)
        {
            Destroy(coin);
        }
    }
}
