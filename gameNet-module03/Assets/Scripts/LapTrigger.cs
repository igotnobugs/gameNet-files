using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTrigger : MonoBehaviour
{

    public LapTrigger nextLapTrigger;
    public bool isFinishLine = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos() {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position,           
            GetComponent<BoxCollider>().size          
            );

        Gizmos.color = new Color(0, 0, 1, 0.7f);
        Gizmos.DrawLine(
            transform.position,
            nextLapTrigger.gameObject.transform.position
            );
    }
}
