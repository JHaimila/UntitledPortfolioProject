using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RPG.Core;

namespace RPG.Dialogue
{
    public class DialogueNode:ScriptableObject
    {
        // [field:SerializeField] public string UniqueID{get; set;}
        [SerializeField] private string text;

        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect nodeRect = new Rect(0,0,200,250);
        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] string EnterActions;
        [SerializeField] string ExitActions;
        [SerializeField] Condition condition;


        public Rect GetRect()
        {
            return nodeRect;
        }
        public string GetText()
        {
            return text;
        }
        public List<string> GetChildren()
        {
            return children;
        }
        public bool IsPlayerSpeaking(){return isPlayerSpeaking;}
        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators);
        }
        public string GetEnterActions()
        {
            return EnterActions;
        }
        public string GetExitActions()
        {
            return ExitActions;
        }
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Changed Node Position");
            nodeRect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Changed Node Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void SetPlayerSpeaking(bool value)
        {
            Undo.RecordObject(this, "Changed is player speaking");
            isPlayerSpeaking = value;
            EditorUtility.SetDirty(this);
        }
        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add dialogue link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Add dialogue link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
        

        
#endif
    }
}