using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace NPC
{
    public class GenericNPC : MonoBehaviour
    {
        public Animator animator;
        public Transform target;

        [Header("Logic")]

        //public LogicTrigger moveTrigger;
        public LogicMachine logicMachine;
        public LogicParam a;
        

        [Multiline] public string[] dialogue;
        NavMeshAgent agent;
       

        private UEventHandler eventHandler = new UEventHandler();

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }
            

        void Start()
        {
            logicMachine.OnChangedState.Subscribe(eventHandler, StateChanged);

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



        }


        [Button("Go to Target")]
        public void FollowTarget()
        {
            GotoPoint(target.position);
        }


        public void GotoPoint(Vector3 destination)
        {
            agent.destination = destination;
        }


        private void OnCollisionEnter(Collision collision)
        {
            Debug.LogWarning("Collision");
        }
    }
}

