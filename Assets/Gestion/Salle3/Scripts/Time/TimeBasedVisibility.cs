using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class TimeBasedVisibility : MonoBehaviour
    {
        [Header("Visibility Time Range")]
        [Tooltip("Heure d'apparition (0-24)")]
        [SerializeField] private float startHour = 8f; 
        
        [Tooltip("Heure de disparition (0-24)")]
        [SerializeField] private float endHour = 18f;   

        [Header("What to Control")]
        [SerializeField] private bool controlRenderer = true;
        [SerializeField] private bool controlCollider = false;
        [SerializeField] private bool controlChildren = true;

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = false;

        private Renderer[] renderers;
        private Collider[] colliders;
        private bool isVisible = true;

        void Start()
        {
            
            if (controlRenderer)
            {
                renderers = controlChildren ? 
                    GetComponentsInChildren<Renderer>() : 
                    GetComponents<Renderer>();
            }

            if (controlCollider)
            {
                colliders = controlChildren ? 
                    GetComponentsInChildren<Collider>() : 
                    GetComponents<Collider>();
            }

           
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnTimeChanged += UpdateVisibility;
                UpdateVisibility(TimeManager.Instance.CurrentHour);
            }
            else
            {
                Debug.LogError($"TimeManager non trouvé pour {gameObject.name}!");
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] Visible de {startHour}h à {endHour}h");
            }
        }

        void OnDestroy()
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnTimeChanged -= UpdateVisibility;
            }
        }

        private void UpdateVisibility(float currentHour)
        {
            bool shouldBeVisible = IsInTimeRange(currentHour);

            if (shouldBeVisible != isVisible)
            {
                isVisible = shouldBeVisible;
                SetVisibility(isVisible);

                if (showDebugLogs)
                {
                    Debug.Log($"[{gameObject.name}] {currentHour:F2}h - " +
                              $"{(isVisible ? "VISIBLE" : "INVISIBLE")} " +
                              $"(Plage: {startHour}h-{endHour}h)");
                }
            }
        }

        private bool IsInTimeRange(float hour)
        {
            
            hour = hour % 24f;
            if (hour < 0) hour += 24f;

            float normalizedStart = startHour % 24f;
            float normalizedEnd = endHour % 24f;

            if (normalizedStart <= normalizedEnd)
            {
                
                return hour >= normalizedStart && hour < normalizedEnd;
            }
            else
            {
                
                return hour >= normalizedStart || hour < normalizedEnd;
            }
        }

        private void SetVisibility(bool visible)
        {
           
            if (renderers != null)
            {
                foreach (var renderer in renderers)
                {
                    if (renderer != null)
                        renderer.enabled = visible;
                }
            }

           
            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    if (collider != null)
                        collider.enabled = visible;
                }
            }
        }

        
        public void ForceVisible()
        {
            isVisible = true;
            SetVisibility(true);
        }

        public void ForceInvisible()
        {
            isVisible = false;
            SetVisibility(false);
        }

        public bool IsCurrentlyVisible()
        {
            return isVisible;
        }

        public string GetTimeRange()
        {
            return $"{startHour:F0}h - {endHour:F0}h";
        }

      
        void OnValidate()
        {
            startHour = Mathf.Clamp(startHour, 0f, 24f);
            endHour = Mathf.Clamp(endHour, 0f, 24f);
        }

       
    
    }
}
