using UnityEngine;

namespace Horse
{
    public class MountHorse : MonoBehaviour
    {
        public GameObject self;
        public GameObject horse;
        public GameObject horsePlaceholder;

        public float interactionDistance = 10f;

        private bool _isMounted = false;
    
        void Update()
        {
            if (horse != null && Input.GetKeyDown(KeyCode.E))
            {
                float distance = Vector3.Distance(transform.position, horse.transform.position);
            
                if (distance <= interactionDistance)
                {
                    _isMounted = true;
                }
            }

            if (_isMounted)
            {
                horse.SetActive(true);
            
                self.SetActive(false);
                horsePlaceholder.SetActive(false);
                _isMounted = false;
            }
        }
    }
}
