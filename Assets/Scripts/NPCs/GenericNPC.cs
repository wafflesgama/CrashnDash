using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using static UEventHandler;

namespace NPC
{
    public class GenericNPC : MonoBehaviour
    {
        [Header("Refs")]
        public Transform target;
        public Animator animator;
        public NavMeshAgent agent;
        public ViewDetection viewDetection;


        [Header("Suspicion ")]
        public float susIncreaseRate;
        public float susDecreaseRate;
        public float susLevel = 0.0f;

        [Header("Logic")]
        //public LogicTrigger moveTrigger;
        public LogicMachine logicMachine;
        public LogicParam a;



        [Header("Dialogue")]
        [Multiline] public string[] dialogue;


        private UEventHandler eventHandler = new UEventHandler();

        Transform playerRef;

        public UEvent OnBeginSus = new UEvent();
        public UEvent OnEndSus = new UEvent();

        bool isPlayerinView;
        bool isObjSus;
        GameObject susPlayerObject;

        private void Awake()
        {
            logicMachine.OnChangedState.Subscribe(eventHandler, StateChanged);
            viewDetection.OnPlayerEnteredArea.Subscribe(eventHandler, PlayerInView);
            viewDetection.OnPlayerLeftArea.Subscribe(eventHandler, PlayerOutofView);
        }


        void Start()
        {
        }

        private void OnDestroy()
        {
            eventHandler.UnsubcribeAll();
        }

        private void StateChanged(string newState)
        {

        }

        private void FixedUpdate()
        {
            HandleSuspicion();

        }

        private void HandleSuspicion()
        {
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


        //[Button("Go to Target")]
        public void FollowTarget()
        {
            GotoPoint(target.position);
        }


        public void GotoPoint(Vector3 destination)
        {
            agent.destination = destination;
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

        private void OnCollisionEnter(Collision collision)
        {
            Debug.LogWarning("Collision");
        }
    }
}

