using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    public interface IAttackable
    {
        void OnAttacked(float damage, GameObject instigator);
        bool Attackable();
    }
}

