using UnityEngine;

/// <summary>
/// Script additionnel pour gérer le placement des objets dans les puzzles
/// À ajouter EN PLUS de GrabbableObject (ne le remplace pas)
/// </summary>
public class PuzzleObjectController : MonoBehaviour
{
    [Header("🧩 Configuration Puzzle")]
    [Tooltip("ID unique pour identifier cet objet")]
    public string objectID = "Cube1";
    
    [Tooltip("Type d'objet (Cube, Sphere, etc.)")]
    public string objectType = "Cube";
    
    [Tooltip("Couleur quand correctement placé")]
    public Color placedColor = Color.green;
    
    [Tooltip("Renderer de l'objet (auto-détecté si vide)")]
    public Renderer objectRenderer;

    private Color originalColor;
    private bool isPlaced = false;
    private GrabbableObject grabbableScript;
    private Rigidbody rb;

    void Start()
    {
     
        if (objectRenderer == null)
            objectRenderer = GetComponent<Renderer>();
        
        grabbableScript = GetComponent<GrabbableObject>();
        rb = GetComponent<Rigidbody>();

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;

        
        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogError($"❌ {gameObject.name} : objectID est vide !");
        }

        if (grabbableScript == null)
        {
            Debug.LogError($"❌ {gameObject.name} : GrabbableObject manquant !");
        }
    }

 
    public void OnPlacedCorrectly()
    {
        if (isPlaced) return;

        Debug.Log($"✅ {objectID} placé correctement !");

        isPlaced = true;

      
        if (objectRenderer != null)
        {
            objectRenderer.material.color = placedColor;
        }

        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        
        if (grabbableScript != null && grabbableScript.beingCarried)
        {
           
            transform.parent = null;
        }

      
        if (gameObject.CompareTag("Interactable"))
        {
            gameObject.tag = "Untagged";
        }
    }

  
    public void OnRemovedFromZone()
    {
        if (!isPlaced) return;

        Debug.Log($"🔄 {objectID} retiré de la zone");

        isPlaced = false;

        
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }

        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        gameObject.tag = "Interactable";
    }

    
    public bool CanBePlaced()
    {
        return !isPlaced;
    }

   
    public string GetObjectID() => objectID;
    public string GetObjectType() => objectType;
    public bool IsPlaced() => isPlaced;

   
    void OnDrawGizmos()
    {
        if (isPlaced)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}
