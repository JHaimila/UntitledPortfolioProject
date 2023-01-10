using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public struct ProgressionFormula
    {
        // [Range(1,1000)]
        // [SerializeField] private float startingValue;
        // [Range(0,1)]
        // [SerializeField] private float percentageAdded;
        // [Range(0,1000)]
        // [SerializeField] private float absoluteAdded;
        [SerializeField] private  AnimationCurve curve;

        public float Calculate(int level)
        {
            // if (level <= 1) return startingValue;
            // var c = Calculate(level - 1);
            // var value = c + (c * percentageAdded) + absoluteAdded;
            // return value + c * curve.Evaluate(1f / level);
            return curve.Evaluate(level);
        }
    }
}