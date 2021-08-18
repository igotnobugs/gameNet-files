using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TakingDamage : MonoBehaviourPunCallbacks
{
    private float maxHealth = 100;
    public float currentHealth;

    [SerializeField]
    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0) {
            Die();
        }
        
    }

    private void Die() {
        if (photonView.IsMine) {
            GameManager.instance.LeaveRoom();          
        }
    }
}
