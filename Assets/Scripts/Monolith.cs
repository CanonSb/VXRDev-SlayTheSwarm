using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    public GameObject gameController;
    public GameObject enemySpawner;
    public GameObject hurtOverlay;

    // Start is called before the first frame update
    void Start()
    {
        
        if (gameController == null) gameController = GameObject.FindWithTag("GameController");
        if (enemySpawner == null) enemySpawner = GameObject.FindWithTag("EnemySpawnController");
        if (hurtOverlay == null) hurtOverlay = GameObject.FindWithTag("HurtOverlay");
        hurtOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        gameController.SetActive(true);
        enemySpawner.SetActive(true);
    }
}
