using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

namespace NPC
{
    public class NPCAreaListener : MonoBehaviour
    {
        public UEvent<NPCArea_Door, bool> onDoorApproach = new UEvent<NPCArea_Door, bool>();
        public float doorMinThreshold = 0.4f;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTrigger arealistener" + other.name);

            if (!other.gameObject.TryGetComponent<NPCArea>(out NPCArea area)) return;

            var dot = Vector3.Dot(transform.forward, other.transform.forward);

            if (Mathf.Abs(dot) > doorMinThreshold) return;

            Debug.Log("Area type is " + area.GetType().ToString());

            if (area.GetType() == typeof(NPCArea_Door))
            {
                onDoorApproach.TryInvoke((NPCArea_Door)area, doorMinThreshold > 0);
            }



        }
    }
}
