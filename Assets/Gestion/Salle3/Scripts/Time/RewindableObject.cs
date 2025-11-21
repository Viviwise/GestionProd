using System.Collections.Generic;
using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class RewindableObject : MonoBehaviour
    {
        private struct StateData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
        }
        [SerializeField] private float recordInterval= 0.05f;
        [SerializeField] private float maxRecordTime= 10f;
        
        private LinkedList<StateData> stateHistory = new LinkedList<StateData>();
        private float recordTimer;
        private int maxStates;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            maxStates= Mathf.CeilToInt(maxRecordTime/recordInterval);
            Debug.Log($"RewindableObject initialisé sur {gameObject.name}- MaxStates {maxStates}");
            
        }

        void FixedUpdate()
        {
            if (!Rewind.isRewinding)
            {
                RecordState();
            }
            else
            {
                PlaybackState();
            }
        }

        private void PlaybackState()
        {
            if (stateHistory.Count > 0)
            {
                StateData stata = stateHistory.First.Value;
                stateHistory.RemoveFirst();
                
                transform.position = stata.position;
                transform.rotation = stata.rotation;

                if (rb)
                {
                    rb.linearVelocity = stata.velocity;
                    rb.angularVelocity = stata.angularVelocity;
                }
            }
        }

        private void RecordState()
        {
           recordTimer += Time.fixedDeltaTime;

           if (recordTimer >= recordInterval)
           {
               recordTimer = 0;
               StateData stateData = new StateData
               {
                   position =  transform.position,
                   rotation =  transform.rotation,
                   velocity =  rb ? rb.linearVelocity : Vector3.zero,
                   angularVelocity =  rb ? rb.angularVelocity : Vector3.zero
               };
               stateHistory.AddFirst(stateData);

               if (stateHistory.Count > maxStates)
               {
                   stateHistory.RemoveLast();
               }

               if (stateHistory.Count % 20 == 0)
               {
                   Debug.Log($"{gameObject.name} Historique {stateHistory.Count} états" );
               }
               
              
               
           }
           
        }
    }
}