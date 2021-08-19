using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillFeedList : MonoBehaviour
{
    public TextMeshProUGUI killerText;
    public TextMeshProUGUI victimText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUp(string killer, string victim) {
        killerText.text = killer;
        victimText.text = victim;
    }

}
