using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliceToBuy : MonoBehaviour
{   
    // Start is called before the first frame update

    public GameObject gameController;

    // PlayerCoins script
    public PlayerCoins playerCoins;

    // Reference to Right Hand Text UI
    public TextMeshProUGUI output;

    private int numCoins;

    void Start()
    {
        numCoins = 0;

        if (gameController == null) gameController = GameObject.FindWithTag("GameController");

        if (gameController != null)
        {
            Debug.Log($"Game Controller found");
            playerCoins = gameController.GetComponent<PlayerCoins>();
        }
        else
        {
            Debug.LogError("Game Controller NULL!");
            output.text = "playerCoins is NULL!";
            return;
        }

        numCoins = playerCoins.GetCoinBalance();

        // if (playerCoins != null)
        // {
        //     numCoins = playerCoins.GetTotalCoins();
        //     output.text = $"Player has {numCoins}.";
        //     Debug.Log($"Player has {numCoins}.");
        // }
        // else
        // {
        //     Debug.LogError("playerCoins is NULL!");
        //     output.text = "playerCoins is NULL!";
        //     return;
        // }

        Debug.Log($"STB Start: Player has {numCoins}.");
    }

    // Update is called once per frame
    void Update()
    {
        // numCoins = playerCoins.GetTotalCoins();
        // output.text = $"Player has {numCoins}!";
    }

    void OnDestroy()
    {
        playerCoins.EarnCoins(5);
        numCoins = playerCoins.GetCoinBalance();
        output.text = $"Player has {numCoins}!";
    }
}
