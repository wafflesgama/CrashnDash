using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVfxManager : MonoBehaviour
{
    public PlayerMovementController movementController;

    //[Header("Dust effect")]
    //public VisualEffect dustVisualEffect;
    //public float speedStartThreshold = 8;
    //public float particleRate = 8;


    UEventHandler eventHandler = new UEventHandler();


    void Start()
    {
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

}
