using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class ObjectInserter : MonoBehaviour
{
    public UEvent<string, Transform> OnObjectInserted = new UEvent<string, Transform>();
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Interactable") return;
        string objType = "default";
        if (other.TryGetComponent<Qualifier_Shock>(out Qualifier_Shock a))
            objType = nameof(Qualifier_Shock);

        OnObjectInserted.TryInvoke(objType, other.transform);

    }
}
