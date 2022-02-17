using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static PlayerSoundManager;
using static SoundUtils;

public class Obj_Engine : MonoBehaviour
{
    public ObjectInserter objectInserter;
    public VisualEffect visualEffect;
    public Sound explosionSound;
    public Sound shockSound;
    AudioSource audioSource;
    
    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        objectInserter.OnObjectInserted.Subscribe(eventHandler, ObjectInserted);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ObjectInserted(string objType, Transform objTransform)
    {
        switch (objType)
        {
            case nameof(Qualifier_Shock):
                Explode();
                break;
            default:
                TinyReact();
                break;
        }

        Destroy(objTransform.gameObject);
    }

    private void Explode()
    {
        Debug.Log("Boom!");
        visualEffect.SendEvent("OnExplosion");
        audioSource.PlaySound(explosionSound);
    }

    private void TinyReact()
    {
        Debug.Log("Tiny Boom!");
        visualEffect.SendEvent("OnTiny");
        audioSource.PlaySound(shockSound);
    }
}
