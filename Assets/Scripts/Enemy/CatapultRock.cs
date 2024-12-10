using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatapultRock : MonoBehaviour
{

    private PlayerHealth hpController;
    public ParticleSystem dustParticles;
    private ParticleSystem _activePS;


    // Start is called before the first frame update
    void Start()
    {
        hpController = GameObject.FindWithTag("GameController")?.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            print("hit player");
            hpController.takeDamage(2);
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        _activePS = Instantiate(dustParticles, transform.position, dustParticles.transform.rotation);
        _activePS.Play();
        Destroy(_activePS, 2);
    }
}
