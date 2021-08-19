using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class VehicleSelection : MonoBehaviour
{
    [Header("UI Selection")]
    public int selectedVehicleIndex;
    public Text carName;
    public Text carDescription;

    [Header("Garage Show")]
    public GameObject carShow;
    public Vehicle shownCar;
    public Image acclerationBar;
    public Image speedBar;
    public Image handlingBar;
    public Image massBar;

    [Header("Racing Selection")]
    public Vehicle[] availableVehicles;

    [Header("Combat Selection")]
    public Vehicle[] availableCombatVehicles;

    public bool deathRaceMode = false;

    // Start is called before the first frame update
    void Start()
    {
        selectedVehicleIndex = 0;
        
    }

    public void ShowCars() {
        ActivatePlayer(selectedVehicleIndex);
    }

    private void ActivatePlayer(int x) {

        //for(int i = 0; i < availableVehicles.Length; i++) {
        //availableVehicles[i].SetActive(i == x);
        //}

        if (shownCar != null) 
            Destroy(shownCar.gameObject);

        if (!deathRaceMode) {
            shownCar = Instantiate(availableVehicles[x], carShow.transform);
        } else {
            shownCar = Instantiate(availableCombatVehicles[x], carShow.transform);
        }
        
        shownCar.enabled = true;

        carName.text = shownCar.vehicleStats.name.ToString();
        carDescription.text = shownCar.vehicleStats.description.ToString();

        acclerationBar.fillAmount = shownCar.vehicleStats.acceleration / 100.0f;
        speedBar.fillAmount = shownCar.vehicleStats.maxSpeed / 100.0f;
        handlingBar.fillAmount = shownCar.vehicleStats.handling / 100.0f;
        massBar.fillAmount = shownCar.vehicleStats.mass / 100.0f;

        ExitGames.Client.Photon.Hashtable playerSelectionProperties 
            = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_SELECTION_INDEX , x} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperties);
    }

    public void GoToNextSelection() {
        selectedVehicleIndex++;

        if (!deathRaceMode) {
            if (selectedVehicleIndex >= availableVehicles.Length) {
                selectedVehicleIndex = 0;
            }
        } else {
            if (selectedVehicleIndex >= availableCombatVehicles.Length) {
                selectedVehicleIndex = 0;
            }
        }

        ActivatePlayer(selectedVehicleIndex);
    }

    public void GoToPreviousSelection() {
        selectedVehicleIndex--;

        if (!deathRaceMode) {
            if (selectedVehicleIndex < 0) {
                selectedVehicleIndex = availableVehicles.Length - 1;
            }
        } else {
            if (selectedVehicleIndex < 0) {
                selectedVehicleIndex = availableCombatVehicles.Length - 1;
            }
        }
        ActivatePlayer(selectedVehicleIndex);
    }
}
