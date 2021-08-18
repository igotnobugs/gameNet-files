using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    private void Start() {
        if (photonView.IsMine) {
            transform.GetComponent<MovementController>().enabled = true;
            playerCamera.GetComponent<Camera>().enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = true;
        } else {
            transform.GetComponent<MovementController>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }

        playerNameText.text = photonView.Owner.NickName;
    }


}
