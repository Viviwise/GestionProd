using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    public GameObject[] objectsToChange;
    public string targetLayerName = "Default";

    public void ChangeLayer()
    {
        if (!Application.isPlaying) return; // ✅ Protection

        int layerId = LayerMask.NameToLayer(targetLayerName);

        if (layerId == -1)
        {
            Debug.LogError($"❌ Layer '{targetLayerName}' introuvable!");
            return;
        }

        foreach (var obj in objectsToChange)
        {
            if (obj != null)
            {
                obj.layer = layerId;
                Debug.Log($"✅ {obj.name} → Layer '{targetLayerName}'");
            }
        }
    }
}