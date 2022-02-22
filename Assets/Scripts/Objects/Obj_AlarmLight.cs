using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obj_AlarmLight : MonoBehaviour
{
    Light light;
    public Ease ease;
    public float speed=2;

    private void Awake()
    {
        light = GetComponent<Light>();
    }
    void Start()
    {
        light.DOIntensity(0, speed).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
