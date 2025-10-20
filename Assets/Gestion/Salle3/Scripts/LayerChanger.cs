using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LayerChanger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private List<GameObject> meshObject = new List<GameObject>();
    [SerializeField] private string layerName = "Default";
    [SerializeField] private bool isChangedChildren = false;
    
    [Header("Events")]
    public UnityEvent onLayerChange;

    private int indexLayer = -1;

    void Start()
    {
        ValidateLayer();
    }

    private void ValidateLayer()
    {
        indexLayer = LayerMask.NameToLayer(layerName);
        
        if (indexLayer == -1)
        {
            Debug.LogError($"❌ Layer '{layerName}' n'existe pas! Allez dans Edit > Project Settings > Tags and Layers pour l'ajouter.", this);
        }
        else
        {
            Debug.Log($"✓ Layer '{layerName}' trouvé (Index: {indexLayer})", this);
        }
    }

    public void ChangeLayer()
    {
        // Vérifications de sécurité
        if (meshObject == null || meshObject.Count == 0)
        {
            Debug.LogWarning("⚠️ Liste d'objets vide. Aucun changement effectué.", this);
            return;
        }

        if (indexLayer == -1)
        {
            Debug.LogError($"❌ Layer '{layerName}' invalide. Impossible de changer.", this);
            return;
        }

        // Changement des layers
        int changedCount = 0;
        
        foreach (GameObject obj in meshObject)
        {
            if (obj == null)
            {
                Debug.LogWarning("⚠️ Un objet null détecté dans la liste.", this);
                continue;
            }

            if (isChangedChildren)
            {
                changedCount += ChangeLayerRecursive(obj.transform, indexLayer);
            }
            else
            {
                obj.layer = indexLayer;
                changedCount++;
            }
        }

        Debug.Log($"✓ {changedCount} objet(s) changé(s) vers le layer '{layerName}'", this);
        
        // Invoke l'event
        onLayerChange?.Invoke();
    }

    private int ChangeLayerRecursive(Transform obj, int layer)
    {
        if (obj == null) return 0;

        obj.gameObject.layer = layer;
        int count = 1;

        foreach (Transform child in obj)
        {
            count += ChangeLayerRecursive(child, layer);
        }

        return count;
    }
    

    public void ClearObjects()
    {
        meshObject.Clear();
        Debug.Log("✓ Liste d'objets vidée", this);
    }

    
}
