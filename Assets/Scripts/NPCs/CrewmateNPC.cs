using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

namespace NPC
{
    public class CrewmateNPC : BaseNPC
    {

        [Header("Suspicion ")]
        public float susIncreaseRate = 0.006f;
        public float susDecreaseRate = 0.006f;
        public float susLevel = 0.0f;




        public UEvent OnBeginSus = new UEvent();
        public UEvent OnEndSus = new UEvent();

        bool isObjSus, isPlayerinView;
        GameObject susPlayerObject;

        void Awake()
        {
            base.Awake();
            viewDetection.OnPlayerEnteredArea.Subscribe(eventHandler, PlayerInView);
            viewDetection.OnPlayerLeftArea.Subscribe(eventHandler, PlayerOutofView);
        }


        protected new void FixedUpdate()
        {
            base.FixedUpdate();
            HandleSuspicion();
        }


        private void HandleSuspicion()
        {
            Debug.Log("Handle SUs");
            //In case Player is acting sus
            if (isPlayerinView && isObjSus)
            {
                //In case NPC needs to start suspecting
                if (susLevel <= 0)
                    OnBeginSus.TryInvoke();

                susLevel += susIncreaseRate;
            }

            //In case NPC suspecting and Player is no longer acting sus
            if (susLevel > 0 && (!isPlayerinView || !isObjSus))
            {
                susLevel -= susDecreaseRate;
                if (susLevel <= 0)
                {
                    OnEndSus.TryInvoke();
                    susLevel = 0;
                }
            }
        }

        public (int, float) GetSusLevel()
        {
            return ((int)susLevel, susLevel % 1);
        }
        private void PlayerInView(Transform player, Transform objectGrabbing)
        {

            if (objectGrabbing != null)
            {

                if (susPlayerObject != null && isObjSus &&
                    susPlayerObject.GetInstanceID() == objectGrabbing.gameObject.GetInstanceID())
                    isObjSus = true;
                else
                    isObjSus = objectGrabbing.TryGetComponent<Qualifier_Shock>(out Qualifier_Shock _);

            }
            else
                isObjSus = false;

            playerRef = player;

            susPlayerObject = objectGrabbing == null ? null : objectGrabbing.gameObject;
            isPlayerinView = true;
        }

        private void PlayerOutofView()
        {
            isPlayerinView = false;
        }



   
    }
}
