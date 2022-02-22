using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using static UEventHandler;

namespace NPC
{
    public class BaseNPC : MonoBehaviour
    {
        [Header("Refs")]
        public Transform target;
        public Animator animator;
        public NavMeshAgent agent;
        public ViewDetection viewDetection;
        public NPCAreaListener areaListener;

        [Header("Navigation")]
        public float minArrivalThreshold = 0.01f;
        public float extrapolationDistance = 1.5f;
        public float maxHeightLevel = 20f;

        [Header("Logic")]
        //public LogicTrigger moveTrigger;
        public LogicMachine logicMachine;
        public LogicParam a;

        protected UEventHandler eventHandler = new UEventHandler();

        protected Transform playerRef;
        protected Vector3 walkDestination;

        public Transform placholder;


        private Vector3 storedDestination;
        private NPCArea_Door doorApproached;

        public UEvent OnArrivedDestination = new UEvent();

        bool isMoving;
        bool isFollowing;

        protected void Awake()
        {
            logicMachine.OnChangedState.Subscribe(eventHandler, StateChanged);
            areaListener.onDoorApproach.Subscribe(eventHandler, DoorApproached);

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

        protected void FixedUpdate()
        {
            //Debug.Log("Base fixedupdate");

            //var dist1 = Vector3.Distance(agent.transform.position, agent.destination);
            //var dist2 = Vector3.Distance(agent.nextPosition, agent.destination);
            //Debug.Log($"Dis1 {dist1}");
            //Debug.Log($"Dis2 {dist2}");
            //if (agent.path.corners.Length > 1)
            //placholder.position = agent.transform.position + agent.transform.forward * agent.velocity.magnitude * extrapolationDistance;
            //placholder.position = agent.nextPosition;

            if (isFollowing && isMoving && !agent.isStopped)
            {
                agent.destination = target.position;
            }

            if (isMoving && (HasArrivedDestination() || agent.isStopped))
            {
                OnArrivedDestination.TryInvoke();
                StopWalking();
            }
        }




        [Button("Go to Target")]
        public void FollowTarget()
        {
            isFollowing = true;

            StartWalking(target.position);
        }




        public void StartWalking(Vector3 destination)
        {
            agent.destination = destination;

            agent.isStopped = false;
            walkDestination = destination;
            isMoving = true;
            animator.SetBool("Moving", true);
        }

        public void StopWalking()
        {
            Debug.Log("NPC Stopped");
            agent.isStopped = true;
            isMoving = false;
            animator.SetBool("Moving", false);
        }

        private bool HasArrivedDestination()
        {
            //If is not on the same level the destination 
            var a = GetCurrentPosition();
            if (Mathf.Abs(a.y - walkDestination.y) > maxHeightLevel) return false;
            var b = walkDestination;
            a.y = 0;
            b.y = 0;

            var dist = Vector3.Distance(a, b) < minArrivalThreshold;
            //Debug.LogWarning("Distancetest " + Vector3.Distance(a, b));
            //Debug.Log("Has arrived test " + (dist ? "yes" : "no"));
            return dist;
        }
        private Vector3 GetCurrentPosition()
        {
            return agent.transform.position;
        }
        private Vector3 GetFuturePosition()
        {
            return GetCurrentPosition() + agent.transform.forward * agent.velocity.magnitude * extrapolationDistance;
        }

        private void DoorApproached(NPCArea_Door doorArea, bool isEntering)
        {
            //Debug.Log($"Is at the door" + (isEntering ? "" : "not ") + " entering");

            //If the player is heading towards the wall
            if (Vector3.Distance(doorArea.transform.position, GetCurrentPosition()) >
                    Vector3.Distance(doorArea.transform.position, GetFuturePosition()))
            {
                Debug.LogWarning("Door here");
                if (doorArea.doorData.isOpen) return;

                storedDestination = agent.destination;
                doorApproached = doorArea;

                StartWalking(doorArea.swichPoint.position);
                OnArrivedDestination.Subscribe(eventHandler, OpenDoor);

                //StopWalking();
            }
        }

        private async void OpenDoor()
        {
            //agent.transform.LookAt(doorApproached.)
            OnArrivedDestination.Unsubscribe(OpenDoor);
            doorApproached.doorData.OpenDoor();
            await Task.Delay(1600);
            StartWalking(storedDestination);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.LogWarning("Collision");
        }
    }
}

