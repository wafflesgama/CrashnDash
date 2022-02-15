using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

namespace Utils
{
    //[Serializable]
    //public class LogicTrigger
    //{
    //    [SerializeField]
    //    public string Id;
    //    public LogicMachine Machine { get; private set; }
    //    public LogicTrigger(string id, LogicMachine machine)
    //    {
    //        Id = id;
    //        Machine = machine;
    //    }
    //    public void Raise() => Machine.SetTrigger(Id);
    //}

    //[Serializable]
    //public class LogicParam<T>
    //{
    //    public string Id { get; private set; }
    //    public LogicMachine Machine { get; private set; }

    //    public T Value
    //    {
    //        get { return Value; }   
    //        set { Value = value; Machine.SetParameter(Value, Id); }
    //    }

    //    public LogicParam(string id, LogicMachine machine)
    //    {
    //        Id = id;
    //        Machine = machine;
    //    }

    //}

    [Serializable]
    public class LogicParam
    {
        public string Id { get; set; }
        public LogicMachine Machine { get; private set; }


        public object Value
        {
            get { return Value; }
            set { Value = value; Machine.SetParameter(Value, Id); }
        }

        public LogicParam(string id, LogicMachine machine)
        {
            Id = id;
            Machine = machine;
        }

    }


    [RequireComponent(typeof(Animator))]
    public class LogicMachine : MonoBehaviour
    {

        public UEvent<string> OnChangedState= new UEvent<string>(); 
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            foreach (var item in animator.parameters)
            {
               
            } 
        }

        internal void SetTrigger(string id) =>
        animator.SetTrigger(id);


        internal void SetParameter(object param, string id)
        {
            var paramType = param.GetType();
            if (paramType == typeof(bool))
                animator.SetBool(id, (bool)param);
            else if (paramType == typeof(int))
                animator.SetInteger(id, (int)param);
            else if (paramType == typeof(float))
                animator.SetFloat(id, (float)param);
        }

       
    }
}
