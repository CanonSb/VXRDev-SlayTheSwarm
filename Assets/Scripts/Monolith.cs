using UnityEngine;
using UnityEngine.InputSystem;

public class Monolith : MonoBehaviour
{
    public GameObject gameController;
    public GameObject enemySpawner;
    public GameObject hurtOverlay;
    public GameObject deathOverlay;

    // Start is called before the first frame update
    void Start()
    {
        
        if (gameController == null) gameController = GameObject.FindWithTag("GameController");
        if (enemySpawner == null) enemySpawner = GameObject.FindWithTag("EnemySpawnController");
        if (hurtOverlay == null) hurtOverlay = GameObject.FindWithTag("HurtOverlay");
        if (deathOverlay == null) deathOverlay = GameObject.FindWithTag("DeathOverlay");
        hurtOverlay.SetActive(false);
        deathOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        gameController?.SetActive(true);
        enemySpawner?.SetActive(true);
    }
}
