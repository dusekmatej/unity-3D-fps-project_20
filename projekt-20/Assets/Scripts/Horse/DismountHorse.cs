using UnityEngine;

namespace Horse
{
    public class DismountHorse : MonoBehaviour
    {
        public GameObject self;
        public GameObject player;
        public GameObject horsePlaceHolder;
    
        private bool _isMounted;


        void Start()
        {
            _isMounted = true;
        }
    
        void Update()
        {
            player.transform.position = self.transform.position;
        
            if (_isMounted && Input.GetKeyDown(KeyCode.E))
            {
                self.SetActive(false);
                player.SetActive(true);
                horsePlaceHolder.SetActive(true);
            }
        }
    }
}
