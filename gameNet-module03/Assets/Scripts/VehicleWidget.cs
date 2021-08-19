using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleWidget : MonoBehaviour
{
    [Header("Set Up")]
    public Vehicle trackedVehicle;
    public Canvas widgetCanvas;
    public TextMeshProUGUI displayName;
    public Image healthBar;

    //private float referenceDistance;
    //private bool willDisplayHealthBar = false;

    // Start is called before the first frame update
    void Start() {
        //referenceDistance = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
    }

    public void SetUp(bool showHealth = false) {
        displayName.text = trackedVehicle.playerName;
        healthBar.gameObject.SetActive(showHealth);
    }

    // Update is called once per frame
    void Update() {
        if (Camera.main == null) return;

        //float dist = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.back);
        //widgetCanvas.transform.localScale = Vector3.one * dist / referenceDistance;

    }
    
    public void HideHealth() {
        healthBar.gameObject.SetActive(false);
    }
}
