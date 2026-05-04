using System.IO;
using com.Sal77.BehaviourExecution;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BehaviourBlackboard))]
public class BehaviourBlackboardEditorController : Editor
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.sal77.behaviourexecution/Resources/BehaviourBlackboardIcon.png");
        
        return icon;
        if (icon != null)
        {
            Texture2D preview = new Texture2D(width, height);
            EditorUtility.CopySerialized(icon, preview);
            return preview;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        m_VisualTreeAsset.CloneTree(root);

        return root;
    }
}
