using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class ObjectiveStatus : MonoBehaviour
    {
        public ObjectiveStatus(Objective objective, bool complete, bool failed)
        {
            _objective = objective;
            _complete = complete;
            _failed = failed;
        }
        
        private Objective _objective;
        private bool _complete = false;
        private bool _failed = false;

        public Objective GetObjective(){return _objective;}
        public void SetObjective(Objective objective){this._objective = objective;}

        public bool IsCompleted(){return _complete;}
        public void SetCompleted(bool status){_complete = status;}

        public bool IsFailed(){return _failed;}
        public void SetFailed(bool status){_failed = status;}

    }
}

