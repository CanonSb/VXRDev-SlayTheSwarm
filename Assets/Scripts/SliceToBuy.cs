using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

public class SliceToBuy : MonoBehaviour
{   
    public GameObject gameController;

    // PlayerCoins script
    public PlayerCoins playerCoins;

    // Reference to Right Hand Text UI
    public TextMeshProUGUI output;

    // Player's coin balance
    private int coinBalance;

    // Item cost
    public int itemCost;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize coinBalance
        coinBalance = 0;

        if (gameController == null) gameController = GameObject.FindWithTag("GameController");

        if (gameController != null)
        {
            Debug.Log($"STB GameController EXISTS.");
            playerCoins = gameController.GetComponent<PlayerCoins>();
        }
        else
        {
            Debug.LogError("STB GameController NULL!");
            // output.text = "STB playerCoins is NULL!";
            return;
        }

        if (playerCoins != null)
        {
            coinBalance = playerCoins.GetCoinBalance();
            // output.text = $"STB Player has {coinBalance}.";
            Debug.Log($"STB playerCoins EXISTS.");
        }
        else
        {
            Debug.LogError("STB playerCoins NULL!");
            // output.text = "STB playerCoins NULL!";
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        itemCost = 5;
        Debug.Log($"STB OnDestroy: itemCost ({itemCost}).");

        coinBalance = playerCoins.GetCoinBalance();
        output.text = $"Player has {coinBalance} coins.";
        Debug.Log($"STB OnDestroy: coinBalance ({coinBalance}).");

        if (playerCoins.SpendCoins(itemCost))
        {
            Debug.Log("STB OnDestroy: Item purchased");
            coinBalance = playerCoins.GetCoinBalance();
            output.text = $"STB OnDestroy: Item Purchased new coinBalance ({coinBalance}).";
            Debug.Log($"STB OnDestroy: new coinBalance ({coinBalance}).");
        }
        else
        {
            Debug.Log("STB OnDestroy: Insufficient Balance!");
            output.text = $"STB OnDestroy: Insufficient Balance! ({coinBalance})";
        }
    }
}
