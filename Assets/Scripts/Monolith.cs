using UnityEngine;
using UnityEngine.InputSystem;

public class Monolith : MonoBehaviour
{
    public GameObject gameController;
    private WaveController _waveController;
    public GameObject enemySpawner;
    public GameObject hurtOverlay;
    public GameObject deathOverlay;

    // Start is called before the first frame update
    void Start()
    {
        if (gameController == null) gameController = GameObject.FindWithTag("GameController");
        if (gameController != null) _waveController = gameController.GetComponent<WaveController>();
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
        if (_waveController != null) _waveController.StartNextWave();
    }
}
