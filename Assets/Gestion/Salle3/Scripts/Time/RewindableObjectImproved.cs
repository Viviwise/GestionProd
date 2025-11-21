using System.Collections.Generic;
using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class RewindableObjectImproved : MonoBehaviour
    {
        private struct StateData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
            public float timestamp;
        }

        [SerializeField] private float recordInterval = 0.05f;
        [SerializeField] private float maxRecordTime = 10f;
        [SerializeField] private bool recordWhenInvisible = true;

        private LinkedList<StateData> stateHistory = new LinkedList<StateData>();
        private float recordTimer;
        private int maxStates;
        private Rigidbody rb;
        private TimeBasedVisibility visibilityController;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            visibilityController = GetComponent<TimeBasedVisibility>();
            maxStates = Mathf.CeilToInt(maxRecordTime / recordInterval);
            
            Debug.Log($"RewindableObject initialisé sur {gameObject.name} - MaxStates: {maxStates}");
        }

        void FixedUpdate()
        {
            if (TimeManager.IsRewinding)
            {
                PlaybackState();
            }
            else
            {
                RecordState();
            }
        }

        private void RecordState()
        {
            // Ne pas enregistrer si invisible (sauf si forcé)
            if (!recordWhenInvisible && visibilityController != null)
            {
                // Vérifier si l'objet est visible
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && !renderer.enabled)
                    return;
            }

            recordTimer += Time.fixedDeltaTime;

            if (recordTimer >= recordInterval)
            {
                recordTimer = 0;
                
                StateData stateData = new StateData
                {
                    position = transform.position,
                    rotation = transform.rotation,
                    velocity = rb ? rb.linearVelocity : Vector3.zero,
                    angularVelocity = rb ? rb.angularVelocity : Vector3.zero,
                    timestamp = TimeManager.Instance ? TimeManager.Instance.CurrentHour : 0f
                };
                
                stateHistory.AddFirst(stateData);

                if (stateHistory.Count > maxStates)
                {
                    stateHistory.RemoveLast();
                }
            }
        }

        private void PlaybackState()
        {
            if (stateHistory.Count > 0)
            {
                StateData state = stateHistory.First.Value;
                stateHistory.RemoveFirst();

                transform.position = state.position;
                transform.rotation = state.rotation;

                if (rb)
                {
                    rb.linearVelocity = -state.velocity; // Inverser pour effet rewind
                    rb.angularVelocity = -state.angularVelocity;
                }
            }
        }

        public int GetHistoryCount()
        {
            return stateHistory.Count;
        }
    }
}
