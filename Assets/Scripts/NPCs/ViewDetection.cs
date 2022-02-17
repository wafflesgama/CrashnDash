using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

namespace NPC
{
    public class ViewDetection : MonoBehaviour
    {
        public UEvent<Transform, Transform> OnPlayerEnteredArea = new UEvent<Transform, Transform>();
        public UEvent OnPlayerLeftArea = new UEvent();

        public UEventHandler eventHandler= new UEventHandler();

        public bool isPlayerInArea;

        Transform playerRef;
        void Start()
        {

        }

        private void OnDestroy()
        {
            eventHandler.UnsubcribeAll();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 6 || isPlayerInArea) return;

            Debug.Log("Player entered area");
            isPlayerInArea = true;
            other.GetComponent<PlayerGrabHandler>().OnObjectGrabbed.Subscribe(eventHandler, ObjectGrabbed);
            other.GetComponent<PlayerGrabHandler>().OnObjectReleased.Subscribe(eventHandler, ObjectReleased);
            var objectGrabbing = other.GetComponent<PlayerGrabHandler>().grabbingObject;
            playerRef = other.transform;
            OnPlayerEnteredArea.TryInvoke(other.transform, objectGrabbing);

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 6 || !isPlayerInArea) return;

            other.GetComponent<PlayerGrabHandler>().OnObjectGrabbed.Unsubscribe(ObjectGrabbed);
            other.GetComponent<PlayerGrabHandler>().OnObjectReleased.Unsubscribe(ObjectReleased);
            Debug.Log("Player left area");
            isPlayerInArea = false;
            OnPlayerLeftArea.TryInvoke();
        }

        private void ObjectGrabbed(GameObject obj)
        {
            OnPlayerEnteredArea.TryInvoke(playerRef, obj.transform);
        }

        private void ObjectReleased()
        {
            OnPlayerEnteredArea.TryInvoke(playerRef, null);
        }
    }
}
