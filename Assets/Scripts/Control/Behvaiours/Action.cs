using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public enum Action
    {
        Attacked,
        SeesTarget,
        SeesPlayerInArea,
        RestrictedActionSeen,
        LeaderDied,
        LostTarget,
        Killed,
        Revived,
        TargetInRange
    }
}
