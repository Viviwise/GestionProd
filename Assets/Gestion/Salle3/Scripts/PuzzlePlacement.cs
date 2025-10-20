using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class PuzzlePlacement : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private PuzzleTrigger _puzzleTrigger;
        [SerializeField] private GameObject requiredObject;
        
        [Header("Détection par Layer")]
        [Tooltip("Layer(s) des objets acceptés (ex: Placeable, Interactive)")]
        [SerializeField] private LayerMask placeableLayer;
        
        [Header("Options")]
        [SerializeField] private float snapDistance = 1f;
        [SerializeField] private bool disableGrabbableOnPlace = true;

        private bool isOccupied = false;
        private GameObject placedObject;

        void Start()
        {
            if (_puzzleTrigger == null)
            {
                Debug.LogError($"❌ {gameObject.name}: PuzzleTrigger non assigné!");
            }

          
            Collider col = GetComponent<Collider>();
            if (col == null)
            {
                BoxCollider box = gameObject.AddComponent<BoxCollider>();
                box.isTrigger = true;
                Debug.LogWarning($"⚠️ {gameObject.name}: BoxCollider auto-ajouté");
            }
            else
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isOccupied || _puzzleTrigger == null) return;

            GameObject obj = other.gameObject;

            if (IsValidObject(obj))
            {
                PlaceObject(obj);
            }
        }

        private bool IsValidObject(GameObject obj)
        {
            
            if (requiredObject != null)
            {
                return obj == requiredObject;
            }

          
            int objLayer = obj.layer;
            return ((1 << objLayer) & placeableLayer) != 0;
        }

        private void PlaceObject(GameObject obj)
        {
            placedObject = obj;
            isOccupied = true;

     
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

        
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;

                #if UNITY_6000_0_OR_NEWER
                    rb.linearVelocity = Vector3.zero;
                #else
                    rb.velocity = Vector3.zero;
                #endif
            }

            
            if (disableGrabbableOnPlace)
            {
                GrabbableObject grabbable = obj.GetComponent<GrabbableObject>();
                if (grabbable != null)
                {
                    grabbable.enabled = false;
                    Debug.Log($"🔒 {obj.name} n'est plus grabbable");
                }
            }

            Debug.Log($"✅ {obj.name} (Layer: {LayerMask.LayerToName(obj.layer)}) placé dans {gameObject.name}");

            // Notifier le PuzzleTrigger
            _puzzleTrigger.OnTriggerActivated();
        }

        public void ResetPlacement()
        {
            if (placedObject != null)
            {
                Rigidbody rb = placedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                // Réactiver GrabbableObject
                if (disableGrabbableOnPlace)
                {
                    GrabbableObject grabbable = placedObject.GetComponent<GrabbableObject>();
                    if (grabbable != null)
                    {
                        grabbable.enabled = true;
                        Debug.Log($"🔓 {placedObject.name} redevient grabbable");
                    }
                }

                placedObject = null;
            }

            isOccupied = false;
            Debug.Log($"🔄 {gameObject.name} réinitialisé");
        }

        public bool IsOccupied() => isOccupied;
        public GameObject GetPlacedObject() => placedObject;

       
        private void OnDrawGizmos()
        {
            Gizmos.color = isOccupied ? Color.blue : Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one * snapDistance);
           
            if (placeableLayer != 0)
            {
                UnityEditor.Handles.Label(
                    transform.position + Vector3.up * 0.5f,
                    $"Layer: {placeableLayer.value}"
                );
            }
        }

        // Validation en éditeur
        private void OnValidate()
        {
            if (placeableLayer == 0 && requiredObject == null)
            {
                Debug.LogWarning($"⚠️ {gameObject.name}: Aucun layer ni objet requis défini!");
            }
        }
    }
}
