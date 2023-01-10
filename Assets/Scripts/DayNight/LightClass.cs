using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DayNight
{
    [CreateAssetMenu(fileName = "New Light", menuName = "Light", order = 1)]
    public class LightClass : ScriptableObject
    {
        public Color ambientLight;
        public Material skybox;
    }
}

