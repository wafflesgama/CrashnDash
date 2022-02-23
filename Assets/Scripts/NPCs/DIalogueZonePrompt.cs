using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIalogueZonePrompt : MonoBehaviour
{
    [Multiline] public string[] dialogue;
    public bool justOnce = true;

    int count;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.LogError("OnTriggerEnter zonepromtper");


        if (other.tag != "Player") return;

        count++;

        if (justOnce && count > 1) return;

        PlayerUIManager.currentManager.DisplayMentorMessage(dialogue);


    }
}
