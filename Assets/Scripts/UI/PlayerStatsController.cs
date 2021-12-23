using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class Stat
{
    //OnStatChanged(float previousValue, float newValue);
    public UEvent<float,float>  OnStatChanged = new UEvent<float, float>();
    public UEvent<float, float> OnMaxStatChanged =new UEvent<float, float>();

    public float MinStatValue { get; set; }

    public float MaxStatValue
    {
        get { return MaxStatValue; }
        set { OnMaxStatChanged.TryInvoke(MaxStatValue, value); MaxStatValue = value; }
    }

    public float StatValue
    {
        get { return StatValue; }
        set { OnStatChanged.TryInvoke(StatValue, value); StatValue = value; }
    }

    public void AddToValue(float valueToAdd)
    {
        var newValue = StatValue + valueToAdd;
        if (newValue < MinStatValue)
            newValue = MinStatValue;
        else if (newValue > MaxStatValue)
            newValue = MaxStatValue;

        StatValue = newValue;
    }

}
public class PlayerStatsController : MonoBehaviour
{
    public Stat health;
    public Stat stamina;

    private
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
