using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue")]
    public class Dialogue : ScriptableObject
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
    {
        [field: SerializeField] public List<DialogueNode> nodes{get; private set;} = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        private Dictionary<string,DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();
        
        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach(var childID in parentNode.GetChildren())
            {
                if(_nodeLookup.ContainsKey(childID))
                {
                    yield return _nodeLookup[childID];
                }
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        private void OnValidate() 
        {
            CreateLookupTable();
        }


        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = MakeNode(parentNode);
            // Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            // Undo.RecordObject(this, "Added Dialogue Node");

            AddNode(newNode);
        }

        public void RemoveNode(DialogueNode deleteNode)
        {
            nodes.Remove(deleteNode);
            foreach(var node in GetAllNodes())
            {
                node.RemoveChild(deleteNode.name);
            }
            CreateLookupTable();
            // Undo.DestroyObjectImmediate(deleteNode);
            // Undo.RecordObject(this, "Removed Dialogue Node");
        }
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if(!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
            {
                if(nodes.Count == 0)
                {
                    DialogueNode newNode = MakeNode(null);
                    AddNode(newNode);
                }
                foreach(DialogueNode node in GetAllNodes())
                {
                    if(string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }
#endif
        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();

            newNode.name = System.Guid.NewGuid().ToString();
            newNode.SetText("[Unwritten Dialogue]");
            newNode.SetPlayerSpeaking(!parentNode.IsPlayerSpeaking());
            newNode.SetPosition(parentNode.GetRect().position + newNodeOffset);

            if (parentNode != null)
            {
                parentNode.AddChild(newNode.name);
            }

            return newNode;
        }
        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            CreateLookupTable();
        }
        public void OnAfterDeserialize(){}

        
        
        
        

        public void CreateLookupTable()
        {
            _nodeLookup.Clear();
            foreach(var node in nodes)
            {
                _nodeLookup.Add(node.name, node);
            }
        }
        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach(DialogueNode node in GetAllChildren(currentNode))
            {
                if(node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach(DialogueNode node in GetAllChildren(currentNode))
            {
                if(!node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }
    }

    
}