using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPunCallbacks
{
    [Header("Settings")]
    public bool ishitscan; // Will not fire projectile if hitscan
    public Projectile projectile; 

    [Header("Weapon Stats")]
    public float fireRate = 2.0f; // per second;
    public int damage = 10;

    [Header("Set Up")]
    public GameObject barrel;
    public GameObject muzzleBlast;
    public GameObject hitParticle; // if Hitscan

    //public Vehicle target;

    private float trueFireRate;
    private float cooldownFireRate;

    // Start is called before the first frame update
    void Start()
    {
        trueFireRate = 1.0f / fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownFireRate > 0) {
            cooldownFireRate -= Time.deltaTime;
        }
    }

    public void Fire(int instigator) {
        if (cooldownFireRate > 0) return;
        cooldownFireRate = trueFireRate;

        photonView.RPC("CreateMuzzleFlash", RpcTarget.All);

        if (ishitscan) {
            if (Physics.Raycast(barrel.transform.position, barrel.transform.forward, out RaycastHit hit, 1000)) {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log(hitObject.name);
                photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

                if (hitObject.CompareTag("Player") && !hitObject.GetComponent<PhotonView>().IsMine) {
                    hitObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, instigator, damage);
                }

                Debug.DrawLine(barrel.transform.position, hit.point,
                    new Color(1.0f, 1.0f, 0.0f), 0.1f);

            } else {
                //Debug.DrawLine(barrel.transform.position, barrel.transform.forward * 20.0f,
                //    new Color(1.0f, 1.0f, 0.0f), 0.1f);
            }
        } else {

            Collider[] hitColliders = Physics.OverlapSphere(barrel.transform.position, 20);
            int targetId = 0;
            foreach (var collider in hitColliders) {
                if (collider.CompareTag("Player") && !collider.GetComponent<PhotonView>().IsMine) {
                    targetId = collider.gameObject.GetPhotonView().ViewID;
                    break;
                }
            }

            gameObject.GetPhotonView().RPC("SpawnProjectile", RpcTarget.AllBuffered, instigator, targetId);
        }
    }


    [PunRPC]
    public void SpawnProjectile(int shooterId, int targetId) {
        Projectile spawnedProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
        GameObject target = PhotonView.Find(targetId).gameObject;

        spawnedProjectile.Init(shooterId, target.GetComponent<Vehicle>());
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position) {
        GameObject hitVfx = Instantiate(hitParticle, position, Quaternion.identity);
    }

    [PunRPC]
    public void CreateMuzzleFlash() {
        GameObject flashVfx = Instantiate(muzzleBlast, barrel.transform.position, barrel.transform.rotation);
    }
}
