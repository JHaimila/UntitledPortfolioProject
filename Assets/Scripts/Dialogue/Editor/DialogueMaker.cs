using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogueMaker : EditorWindow
{
    [MenuItem("Tools/DialogueMaker")]
    public static void OpenWindow()
    {
        DialogueMaker wnd = GetWindow<DialogueMaker>();
        wnd.titleContent = new GUIContent("DialogueMaker");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Dialogue/Editor/DialogueMaker.uxml");
        visualTree.CloneTree(root); //Using CloneTree instead of instantitate b/c with instantiate you have an intermediary object that you have to assign the instantitated object. THis skips that

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Dialogue/Editor/DialogueMaker.uss");
        root.styleSheets.Add(styleSheet);
    }
}