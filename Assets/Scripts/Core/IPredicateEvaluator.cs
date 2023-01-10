using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public enum EPredicate
    {
        Select,
        HasQuest,
        CompletedQuest,
        CompletedObjective,
        HasLevel,
        MinimumTrait,
        HasItem,
        HasItems,
        HasItemEquipped
    }
    public interface IPredicateEvaluator
    {
        bool? Evaluate(EPredicate predicate, List<string> parameters);
    }
}
