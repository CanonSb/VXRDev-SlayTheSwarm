using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int hitPoints = 10;
    public TMP_Text hpText;

    public GameObject hurtOverlay;


    // Start is called before the first frame update
    void Start()
    {
        hpText.text = string.Format("{0}", hitPoints);
        if (hurtOverlay == null) hurtOverlay = GameObject.FindWithTag("HurtOverlay");
        hurtOverlay.SetActive(false);
    }


    public void takeDamage()
    {
        hitPoints -= 1;
        hpText.text = string.Format("{0}", hitPoints);
        StartCoroutine(triggerHurtOverlay());
    }


    public IEnumerator triggerHurtOverlay()
    {
        hurtOverlay.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hurtOverlay.SetActive(false);
    }
}
