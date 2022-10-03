using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ExtendedHotkeys : ScriptableObject
{
    [MenuItem("MPUSP/Set Anchors %_q")]
    static void SetAnchors()
    {
        var selectionObjects = Selection.gameObjects;
        foreach (var t in selectionObjects)
        {
            if (t.GetComponent<RectTransform>())
            {
                var aspect = t.GetComponent<AspectRatioFitter>();
                bool f = aspect != null;

                if (t.GetComponent<AnchorTool>())
                {
                    DestroyImmediate(t.GetComponent<AnchorTool>());
                }

                if (f)
                {
                    aspect.enabled = false;
                }
                t.AddComponent(typeof(AnchorTool));
                t.GetComponent<AnchorTool>().status = "Del";


                Debug.Log("Anchors set for\"" + t.name + "\"");
            }
            else { Debug.Log("\"" + t.name + "\" have no anchors"); }
        }
    }
}