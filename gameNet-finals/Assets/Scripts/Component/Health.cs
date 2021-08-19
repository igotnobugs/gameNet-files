using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks 
{
    // Start is called before the first frame update

    public float maxHealth = 10.0f;
    public float currentHealth = 10.0f;

    public delegate void HealthEvent();
    public event HealthEvent OnZeroHealth;

    void Start()
    {
        
    }

    public void Initialize(float initHealth) {
        maxHealth = initHealth;
        currentHealth = initHealth;
    }

    public void Damage(float amount) {
        photonView.RPC("PunDamage", RpcTarget.AllBuffered, amount);
    }

    [PunRPC]
    public void PunDamage(float amount) {
        if (currentHealth <= 0) return;
        currentHealth -= amount;
        if (currentHealth <= 0) {
            OnZeroHealth?.Invoke();
        }
    }
}
