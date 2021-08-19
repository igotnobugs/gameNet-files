using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerControl : Control
{
    [Header("Prefab Setup")]
    [SerializeField] private PlayerCamera playerCameraPrefab;
    [SerializeField] private ParticleSystem moveBlipPrefab = null;
    [SerializeField] private ParticleSystem attackBlipPrefab = null;
    [SerializeField] private ParticleSystem attackCursorPrefab = null;

    protected InputActions heroAction;   
    private bool attackButtonPressed = false;
    private ParticleSystem spawnedAttackCursor;
    public PlayerCamera spawnedPlayerCamera = null;

    // Start is called before the first frame update
    private void Start()
    {
        //EnableControl();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlled) {
            Vector2 moveCamera = heroAction.CameraControl.WASD.ReadValue<Vector2>();
            spawnedPlayerCamera.Move(moveCamera);
        }
    }

    public void EnableControl() {
        controlled = true;
        spawnedPlayerCamera = Instantiate(playerCameraPrefab, owner.transform.position, Quaternion.identity);
        spawnedPlayerCamera.SetConstraintSource(owner);

        heroAction = new InputActions();
        if (controlled) {
            heroAction.HeroMoveControl.MoveClick.performed += _ => MoveClick();
            heroAction.HeroMoveControl.AttackClick.performed += _ => AttackClick();
            heroAction.HeroMoveControl.Attack.performed += _ => SetAttackMode();
            heroAction.CameraControl.WASD.performed += _ => AnyWASDPressed();
            heroAction.CameraControl.TrackPlayerHero.performed += _ => TrackPlayerHero();
            heroAction.Enable();
        }
    }

    private void MoveClick() {

        if (!owner.photonView.IsMine) return;

        Vector2 mpos = heroAction.HeroMoveControl.MovePosition.ReadValue<Vector2>();
        Ray ray = spawnedPlayerCamera.GetRayFromScreen(mpos);

        if (Physics.Raycast(ray, out RaycastHit hit, 90000.0f)) {
            if (!IsPointerOverUI(mpos)) {
                if (hit.collider.gameObject.tag == "Ground") {
                    if (attackButtonPressed) {
                        Instantiate(attackBlipPrefab, hit.point, Quaternion.identity);
                        AttackMove(hit.point);
                        attackButtonPressed = false;
                    } else {
                        Instantiate(moveBlipPrefab, hit.point, Quaternion.identity);
                        Move(hit.point);
                    }
                }
            }
        }

    }

    private void AttackClick() {
        Vector2 mpos = heroAction.HeroMoveControl.MovePosition.ReadValue<Vector2>();
        Ray ray = spawnedPlayerCamera.GetRayFromScreen(mpos);

        if (Physics.Raycast(ray, out RaycastHit hit, 90000.0f)) {
            if (!IsPointerOverUI(mpos)) {
                // Specify attack target
                if (hit.collider.TryGetComponent(out Unit unit) && unit != owner) {
                    if (spawnedAttackCursor != null) {
                        Destroy(spawnedAttackCursor);
                    }
                    spawnedAttackCursor = Instantiate(attackCursorPrefab, unit.transform);
                    owner.Attack(unit);
                } else {

                    // Attack move
                    if (hit.collider.gameObject.tag == "Ground") {
                        if (spawnedAttackCursor != null) {
                            Destroy(spawnedAttackCursor);
                        }
                        Move(hit.point);
                        Instantiate(attackBlipPrefab, hit.point, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void SetAttackMode() {
        attackButtonPressed = true;
    }

    private void AnyWASDPressed() {
        spawnedPlayerCamera.ToggleLock(false);
    }  

    public void TrackPlayerHero() {
        spawnedPlayerCamera.ToggleLock(true);
    }

    public void ToggleTrackPlayerHero(bool state) {
        spawnedPlayerCamera.ToggleLock(state);
    }

    public void ToggleControlAndMovement(bool state) {
        agent.enabled = state;
    }

    //Prevent clicking through UI
    private bool IsPointerOverUI(Vector2 mpos) {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mpos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
