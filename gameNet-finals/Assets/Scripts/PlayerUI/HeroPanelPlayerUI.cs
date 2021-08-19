using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanelPlayerUI : PlayerUI 
{

    [Header("Bars")]
    public Image healthBar;
    public Image manaBar;
    public Image expBar;

    private bool isInitialized = false;



    public override void Initialize() {
        base.Initialize();

        SetHealthBar(trackedHero.health.currentHealth / trackedHero.health.maxHealth);
        isInitialized = true;

        Debug.Log(trackedHero.name + " health setup for " + trackedHero.playerName);
    }

    public void Update() {
        if (!isInitialized) return;

        SetHealthBar(trackedHero.health.currentHealth / trackedHero.health.maxHealth);
        SetExperienceBar(trackedHero.experience);
    }

    public void SetHealthBar(float percentage) {
        healthBar.fillAmount = percentage;
    }

    public void SetManaBar(float percentage) {
        manaBar.fillAmount = percentage;
    }

    public void SetExperienceBar(float percentage) {
        expBar.fillAmount = percentage;
    }

}
