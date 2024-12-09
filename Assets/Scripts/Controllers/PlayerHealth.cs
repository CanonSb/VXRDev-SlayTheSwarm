using System.Collections;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int hitPoints = 10;
    public TMP_Text hpText;

    public GameObject hurtOverlay;
    public GameObject deathOverlay;


    // Start is called before the first frame update
    void Start()
    {
        hpText.text = string.Format("{0}", hitPoints);
        if (hurtOverlay == null) hurtOverlay = GameObject.FindWithTag("HurtOverlay");
        if (deathOverlay == null) deathOverlay = GameObject.FindWithTag("DeathOverlay");
        hurtOverlay?.SetActive(false);
        deathOverlay?.SetActive(false);
    }


    public void takeDamage()
    {
        hitPoints -= 1;
        hpText.text = string.Format("{0}", hitPoints);
        if (hitPoints <= 0) StartCoroutine(TriggerDeath());
        else StartCoroutine(triggerHurtOverlay());
    }

    public IEnumerator TriggerDeath()
    {
        deathOverlay.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public IEnumerator triggerHurtOverlay()
    {
        hurtOverlay.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hurtOverlay.SetActive(false);
    }
}
