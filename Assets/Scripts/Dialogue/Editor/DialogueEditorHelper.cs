using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Dialogue.Editor
{
    public class DialogueEditorHelper : MonoBehaviour
    {
        private Vector2 newNodeOffset = new Vector2(250, 0);
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
    }
}