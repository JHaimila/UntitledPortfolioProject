using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue selectedDialogue = null;
        
        [NonSerialized]
        private GUIStyle nodeStyle;
        [NonSerialized]
        private GUIStyle playerNodeStyle;
        [NonSerialized]
        private DialogueNode draggingNode = null;
        [NonSerialized]
        private Vector2 draggingOffset;

        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;

        [NonSerialized]
        DialogueNode linkingParentNode = null;

        private Vector2 scrollPosition;

        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        float canvasSize = 4000f;
        float backgroundSize = 50f;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor", false);
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogueEditor = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if(dialogueEditor != null)
            {
                ShowEditorWindow();
            }
            return false;
        }
        private void OnEnable() 
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12,12,12,12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12,12,12,12);
        }
        private void OnSelectionChange() 
        {
            if(Selection.activeObject is Dialogue)
            {
                selectedDialogue = Selection.activeObject as Dialogue;
                Repaint();
            }
        }
        private void OnGUI() 
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvents();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // You need to set the size because unity begin scroll view relies on autolayout elements to determin the size of the screen
                // since the nodes are absolute layout the scrollview can't determine when it needs more space.
                Rect canvas = GUILayoutUtility.GetRect(canvasSize,canvasSize);
                Texture2D bgTexture = Resources.Load("background") as Texture2D;
                Rect textCoords = new Rect(0,0, canvasSize/backgroundSize,canvasSize/backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, bgTexture, textCoords);

                foreach(var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                    
                }
                foreach(var node in selectedDialogue.GetAllNodes())
                {
                    DrawConnnections(node);
                }
                if(creatingNode != null)
                {
                    
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null)
                {
                    selectedDialogue.RemoveNode(deletingNode);
                    deletingNode = null;
                }
                EditorGUILayout.EndScrollView();
            }
            
        }

        

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    GUI.FocusControl(null);
                    GUI.changed = true;
                    Selection.activeObject = selectedDialogue;

                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
                GUI.FocusControl(null);
            }
            else if(Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
                
            }
            else if(Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
            
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            // draggingNode.NodeRect.Contains();
            DialogueNode foundNode = null;
            foreach(var node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = node.IsPlayerSpeaking() ? playerNodeStyle : nodeStyle;

            GUILayout.BeginArea(node.GetRect(), style);
            string newText = EditorGUILayout.TextField(node.GetText());
            node.SetText(newText);


            if(node != null && node.GetChildren().Count > 0)
            {
                EditorGUILayout.LabelField("Children:");
                foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
                {
                    EditorGUILayout.LabelField(childNode.GetText());
                }
            }
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            if(linkingParentNode == null)
            {
                if(GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else
            {
                if(linkingParentNode.name == node.name)
                {
                    if(GUILayout.Button("Cancel"))
                    {
                        linkingParentNode = null;
                    }
                }
                else if(!linkingParentNode.GetChildren().Contains(node.name))
                {   
                    if(GUILayout.Button("Child"))
                    {
                        linkingParentNode.AddChild(node.name);
                        linkingParentNode = null;
                    }
                }
                else
                {
                    if(GUILayout.Button("Unlink"))
                    {
                        linkingParentNode.RemoveChild(node.name);
                        linkingParentNode = null;
                    }
                }
            }
            if(GUILayout.Button("-"))
            {
                deletingNode = node;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        private void DrawConnnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach(var childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition =new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                
                Handles.DrawBezier(startPosition, endPosition, 
                startPosition + controlPointOffset, 
                endPosition - controlPointOffset, 
                Color.white, null, 4f);

            }
        }
    }
}