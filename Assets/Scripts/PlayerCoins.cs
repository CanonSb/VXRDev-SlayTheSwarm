using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{

    public int coins = 0;
    // Reference to Right Hand Text UI
    public TextMeshProUGUI output;
    // Canvas text
    public TMP_Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log($"PC PlayerCoins Start: Player has {coins}.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <returns>Interger</returns>
    public int GetCoinBalance()
    {
        // Debug.Log($"PC GetCoinBalance: Player has {coins}.");
        return coins;
    }

    /// <summary>
    /// Adds the specified number of coins to the player's total.
    /// </summary>
    /// <returns>Void</returns>
    public void EarnCoins(int num)
    {
        coins += num;
        // Debug.Log($"PC EarnCoins: Player has {coins}.");
        if (output != null) output.text = $"STB Player has {coins}.";
        coinText.text = string.Format("{0}", coins);
    }

    /// <summary>
    /// Gets the cost of item by name.
    /// </summary>
    /// <returns>Integer</returns>
    private int itemCosts(string name)
    {
        // Store costs of purchasable items
        var itemCostMapping = new Dictionary<string, int>
        {
            {"dagger", 5},
            {"greatsword", 5},
            {"shield", 5},
            {"sword", 5}
        };

        // Return cost of item or an insanely high value if it's not found
        return itemCostMapping.TryGetValue(name, out int cost) ? cost : 9999;
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
            // Debug.Log($"PC SpendCoins: Player has enough coins ({coins}).");
            coins -= cost;
            // Debug.Log($"PC SpendCoins: Player new balance ({coins}).");
            coinText.text = string.Format("{0}", coins);
            return true;
        }
        else
        {
            // Debug.Log($"PC SpendCoins: Player has does not have enough coins ({coins}).");
            return false;
        }
    }
}
