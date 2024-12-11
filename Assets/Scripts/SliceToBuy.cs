using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliceToBuy : MonoBehaviour
{   
    public GameObject gameController;

    // PlayerCoins script
    public PlayerCoins playerCoins;

    // Reference to Right Hand Text UI
    public TextMeshProUGUI output;

    private int numCoins;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize numCoins
        numCoins = 0;

        if (gameController == null) gameController = GameObject.FindWithTag("GameController");

        if (gameController != null)
        {
            Debug.Log($"STB GameController EXISTS.");
            playerCoins = gameController.GetComponent<PlayerCoins>();
        }
        else
        {
            Debug.LogError("STB GameController NULL!");
            output.text = "STB playerCoins is NULL!";
            return;
        }

        if (playerCoins != null)
        {
            numCoins = playerCoins.GetCoinBalance();
            output.text = $"STB Player has {numCoins}.";
            Debug.Log($"STB playerCoins EXISTS.");
        }
        else
        {
            Debug.LogError("STB playerCoins NULL!");
            output.text = "STB playerCoins NULL!";
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        playerCoins.EarnCoins(5);
        numCoins = playerCoins.GetCoinBalance();
        output.text = $"Player has {numCoins}!";
    }
}
