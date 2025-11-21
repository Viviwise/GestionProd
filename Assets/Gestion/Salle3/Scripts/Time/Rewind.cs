using System;
using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class Rewind : MonoBehaviour
    {
        public static Rewind Instance {get; private set;}
        
        public static bool isRewinding{get; private set;}
        
        [SerializeField] private KeyCode rewindKey =  KeyCode.R;
        
        
        [SerializeField] private float maxRewindDuration = 10f;
        
        private float currentRewindTime = 0f;

        void Awake()
        {
            if (Instance == null)
                Instance= this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKey(rewindKey))
            {
                if (!isRewinding)
                {
                    StartRewind();
                }
                currentRewindTime += Time.deltaTime;
                if (currentRewindTime >= maxRewindDuration)
                {
                    StopRewind();
                }
                else if (Input.GetKeyUp(rewindKey))
                {
                    StopRewind();
                }
            }
        }

        private void StopRewind()
        {
            isRewinding = false; 
            currentRewindTime = 0f;
        }

        private void StartRewind()
        {
            isRewinding = true;
            currentRewindTime = 0f;
        }

        public float GetRewindProgress()
        {
            return currentRewindTime/maxRewindDuration;
        }
    }
}