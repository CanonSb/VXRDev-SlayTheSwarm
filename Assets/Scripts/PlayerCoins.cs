using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{

    public int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        coins = 10;
        Debug.Log($"PlayerCoins Start: Player has {coins}.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <returns>Interger</returns>
    public int GetCoinBalance()
    {
        Debug.Log($"GetCoinBalance: Player has {coins}.");
        return coins;
    }

    /// <summary>
    /// Adds the specified number of coins to the player's total.
    /// </summary>
    /// <returns>Void</returns>
    public void EarnCoins(int num)
    {
        coins += num;
        Debug.Log($"EarnCoins: Player has {coins}.");
    }

    /// <summary>
    /// Subtracts the specified number of coins to the player's total.
    /// </summary>
    /// <returns>Boolean</returns>
    public bool SpendCoins(int cost)
    {
        int balance = GetCoinBalance();
        if (balance >= cost)
        {
            Debug.Log($"SpendCoins: Player has enough coins ({coins}).");
            coins -= cost;
            Debug.Log($"SpendCoins: Player new balance ({coins}).");
            return true;
        }
        else
        {
            Debug.Log($"SpendCoins: Player has does not have enough coins ({coins}).");
            return false;
        }
    }
}
