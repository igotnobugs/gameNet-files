using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject fpsModel;
    public GameObject nonFpsModel;

    public PlayerUI playerUiPrefab;
    public Camera fpsCamera;

    private Animator animator;
    public Avatar fpsAvatar, nonFpsAvatar;

    public PlayerUnit myUnit;

    // Start is called before the first frame update
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        fpsModel.SetActive(photonView.IsMine);
        nonFpsModel.SetActive(!photonView.IsMine);
              
        if (photonView.IsMine) {
            animator.avatar = fpsAvatar;
            PlayerUI playerui = Instantiate(playerUiPrefab);
            myUnit.SetupPlayer(playerui);

        } else {
            animator.avatar = nonFpsAvatar;
        }

        fpsCamera.enabled = photonView.IsMine;
        fpsCamera.GetComponent<AudioListener>().enabled = photonView.IsMine;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
