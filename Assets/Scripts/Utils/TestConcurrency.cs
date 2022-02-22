using NPC;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestConcurrency : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("Go to door")]
    public async void GotoDoor()
    {
        var npcs = GameObject.FindObjectsOfType<CrewmateNPC>();
        foreach (var item in npcs)
        {
            await Task.Delay(150);
            item.FollowTarget();
        }
    }
}
