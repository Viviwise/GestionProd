using UnityEngine;
using UnityEngine.Events;

public class PuzzlePlacementZone : MonoBehaviour
{
    [Header("Configuration")]
    public string zoneName = "Zone 1";
    public string requiredTag = "PuzzleObject";

    [Header("Visuel")]
    public Color emptyColor = new Color(0, 1, 0, 0.3f);
    public Color occupiedColor = new Color(0, 0, 1, 0.3f);
    public Color wrongColor = new Color(1, 0, 0, 0.3f);

    [Header("Audio")]
    public AudioClip placementSound;
    public AudioClip errorSound;

    [Header("État")]
    [SerializeField] private bool isOccupied = false;
    [SerializeField] private GameObject placedObject;

    [Header("Events")]
    public UnityEvent onObjectPlaced;
    public UnityEvent onObjectRemoved;

    private Renderer zoneRenderer;
    private AudioSource audioSource;
    private Collider triggerZone;

    void Start()
    {
        InitializeZone();
    }
    
    void Awake()
    {
        // Vérifier les doublons
        PuzzlePlacementZone[] allZones = FindObjectsOfType<PuzzlePlacementZone>();
        foreach (var zone in allZones)
        {
            if (zone != this && zone.zoneName == this.zoneName)
            {
                Debug.LogError($"⚠️ DOUBLON DÉTECTÉ : Deux zones nommées '{zoneName}'!");
                Debug.LogError($"Zone 1: {this.gameObject.name}, Zone 2: {zone.gameObject.name}", this);
            }
        }
    }

    void InitializeZone()
    {
        // Setup Collider
        triggerZone = GetComponent<Collider>();
        if (triggerZone != null)
        {
            triggerZone.isTrigger = true;
        }

        // Setup Renderer
        zoneRenderer = GetComponent<Renderer>();
        if (zoneRenderer != null)
        {
            UpdateVisualState();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOccupied)
        {
            Debug.Log($"⚠️ {zoneName} déjà occupée");
            return;
        }

        if (other.CompareTag(requiredTag))
        {
            PlaceObject(other.gameObject);
        }
        else
        {
            Debug.Log($"❌ {zoneName} : Mauvais tag ({other.tag})");
            FlashWrongColor();
        }
    }

    void PlaceObject(GameObject obj)
    {
        placedObject = obj;
        isOccupied = true;

        // Désactiver la physique de l'objet
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Centrer l'objet sur la zone
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        UpdateVisualState();

        Debug.Log($"✅ {zoneName} : Objet placé ({obj.name})");

        onObjectPlaced?.Invoke();
    }

    void UpdateVisualState()
    {
        if (zoneRenderer == null) return;

        zoneRenderer.material.color = isOccupied ? occupiedColor : emptyColor;
    }

    void FlashWrongColor()
    {
        if (zoneRenderer == null) return;

        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        Color original = zoneRenderer.material.color;
        zoneRenderer.material.color = wrongColor;
        yield return new WaitForSeconds(0.5f);
        zoneRenderer.material.color = original;
    }

   

  
    [ContextMenu("Reset Zone")]
    public void ResetZone()
    {
        if (placedObject != null)
        {
           
            Rigidbody rb = placedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            placedObject = null;
        }

        isOccupied = false;
        UpdateVisualState();

        onObjectRemoved?.Invoke();

        Debug.Log($"🔄 {zoneName} réinitialisée");
    }

  
    public bool IsOccupied() => isOccupied;
    public GameObject GetPlacedObject() => placedObject;
    public string GetZoneName() => zoneName;

  
    public void RemoveObject()
    {
        if (!isOccupied) return;

        if (placedObject != null)
        {
            Rigidbody rb = placedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        placedObject = null;
        isOccupied = false;
        UpdateVisualState();

        onObjectRemoved?.Invoke();

        Debug.Log($"➖ {zoneName} : Objet retiré");
    }


    void OnValidate()
    {
        if (string.IsNullOrEmpty(zoneName))
        {
            zoneName = gameObject.name;
        }

        if (string.IsNullOrEmpty(requiredTag))
        {
            requiredTag = "PuzzleObject";
        }
    }

  
    void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? occupiedColor : emptyColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
