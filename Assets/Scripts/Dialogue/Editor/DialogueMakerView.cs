using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
public class DialogueMakerView : GraphView
{
    public new class UxmlFactory:UxmlFactory<DialogueMakerView, GraphView.UxmlTraits>{}
    public DialogueMakerView()
    {
        Insert(0, new GridBackground());
        
        //This is how you add in the manipulation to the graph
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Dialogue/Editor/DialogueMaker.uss");
        styleSheets.Add(styleSheet);
    }
}
