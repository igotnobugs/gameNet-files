using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    protected Hero trackedHero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrackedHero(Hero hero) {
        trackedHero = hero;
        Initialize();
    }

    public virtual void Initialize() {

    }
}
