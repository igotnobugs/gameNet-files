using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

//RPC = calls function in EVERY GAMEOBJECT!!!
//client only stuff must not be used in the RPC

public class PlayerUnit : MonoBehaviourPunCallbacks 
{  
    private bool isControllable = false;

    [Header("Unit Settings")]
    private string unitNickname = "Unknown"; //<= Player Nickname
    public Image healthBar;
    public float startHealth = 100;
    public Camera playerCamera;
    //public Camera thirdCamera;
    public GameObject hitEffect;
    //public GameObject fpsModel;
    //public GameObject nonFpsModel;
    //public Avatar fpsAvatar, nonFpsAvatar;

    [Header("Stats")]
    public TextMeshProUGUI playerNameText;   
    public float health; 
    public int kills;

    public GameManager gm;
    public PlayerUI pui; 

    private Animator animator;
    private RigidbodyFirstPersonController rbFPController;
    private Joystick joystick;
    private FixedTouchField fixedTouchField;
    

    private void Awake() {
        rbFPController = GetComponent<RigidbodyFirstPersonController>();
        rbFPController.enabled = false;
        animator = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //thirdCamera.enabled = false;
    }

    // Start is called before the first frame update
    private void Start()
    {            
        health = startHealth;
        healthBar.fillAmount = health / startHealth;    
    }

    //Only called by each of the client
    public void SetupPlayer(PlayerUI playerUI) {
        unitNickname = PhotonNetwork.NickName;
        gameObject.name = PhotonNetwork.NickName + photonView.Owner.UserId;
        
        // Set ui
        pui = playerUI;
        pui.SetOwner(this);
        joystick = pui.moveJoystick;
        fixedTouchField = pui.lookTouchField;
        pui.fireButton.onClick.AddListener(() => Fire());

        animator.SetBool("isLocalPlayer", true);

        isControllable = true;
        rbFPController.enabled = true;

        Camera.main.enabled = false;

        photonView.RPC("NotifyNameChange", RpcTarget.AllBuffered,
            gameObject.name, unitNickname);
    }

    [PunRPC] // Notify everyone that this gameObject has changed/spawned
    public void NotifyNameChange(string name, string uName, PhotonMessageInfo info) {   
        gameObject.name = name;
        unitNickname = uName;
        playerNameText.text = unitNickname;
        AddThisToGm(name);
    }

    // Add the gameObject to each client's gm
    public void AddThisToGm(string name) {
        //gm.AddPlayer(name);
        Debug.Log(name + "has spawned");
        gm.players.Add(this);
    }

    private void FixedUpdate() {
        if (!isControllable) return;

        rbFPController.joystickInputAxis.x = joystick.Horizontal;
        rbFPController.joystickInputAxis.y = joystick.Vertical;
        rbFPController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;

        animator.SetFloat("horizontal", joystick.Horizontal);
        animator.SetFloat("vertical", joystick.Vertical);

        if (joystick.Direction.magnitude > 0.9f) {
            animator.SetBool("isRunning", true);
            rbFPController.movementSettings.ForwardSpeed = 10;
        }
        else {
            animator.SetBool("isRunning", false);
            rbFPController.movementSettings.ForwardSpeed = 5;
        }
    }

    public float GetHealth() {
        return health;
    }

    public void Fire() {
        if (!isControllable) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, 200)) {
            GameObject hitObject = hit.collider.gameObject;

            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hitObject.CompareTag("Player") && !hitObject.GetComponent<PhotonView>().IsMine) {
                hitObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 24, photonView.ViewID);
            }

        }
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position) {
        GameObject hitVfx = Instantiate(hitEffect, position, Quaternion.identity);
    }

    [PunRPC]
    public void TakeDamage(int damage, int shooterId, PhotonMessageInfo info) {

        if (health <= 0) return; //already dead;

        health -= damage;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0) {
            animator.SetBool("isDead", true);           
            GameObject killer = PhotonView.Find(shooterId).gameObject;
            //if (killer != gameObject) {
                killer.GetComponent<PlayerUnit>().AddKill(info.photonView.Owner.NickName);
            //} else {
                //gm.CreateKillFeed(unitNickname, victim);
            //}
            Die();
        } 
    }

    public void AddKill(string victim = "None") {
        kills++;
        gm.CreateKillFeed(unitNickname, victim);
        if (kills >= gm.killsToWin) {
            gm.DisplayWinner(unitNickname);
        }
    }

    public void AddKillFeed(string killer, string victim) {
        gm.CreateKillFeed(killer, victim); 
    }

    public void Die() {
        if (photonView.IsMine) {
            isControllable = false;
            //nonFpsModel.SetActive(true);
            //fpsModel.SetActive(false);
            //animator.avatar = nonFpsAvatar;
            //thirdCamera.enabled = true;
            //playerCamera.enabled = false;       
            StartCoroutine(RespawnCoundown());
        }
    }

    IEnumerator RespawnCoundown() {
        pui.respawnPanel.SetActive(true);
        float respawnTime = gm.respawnTime;
        while (respawnTime > 0) {
            pui.respawnText.text = "Respawn in " + respawnTime + " secs";
            yield return new WaitForSeconds(1.0f);
            respawnTime--;                
        }

        //animator.avatar = fpsAvatar;
        animator.SetBool("isDead", false);
        pui.respawnPanel.SetActive(false);

        transform.position = gm.GetRandomSpawnPoint();    
        isControllable = true;

        //nonFpsModel.SetActive(false);
        //fpsModel.SetActive(true);
        
        //playerCamera.enabled = true;
        //thirdCamera.enabled = false;

        photonView.RPC("Respawned", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Respawned() { 
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
    }


    #region Debugs
    public void KillSelf() {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 9999, photonView.ViewID);
    }

    public void TeleportRandomlyToAnyone() {
        PlayerUnit[] pus = FindObjectsOfType<PlayerUnit>();

        for (int i = 0; i < pus.Length; i++) {
            if (pus[i] == this) continue;
            Vector3 backwards = pus[i].transform.forward * -1;
            transform.position = pus[i].transform.position + (backwards * 2.0f);
            
            return;
        }

        Debug.Log("No one to teleport to");

    }
    #endregion
}
