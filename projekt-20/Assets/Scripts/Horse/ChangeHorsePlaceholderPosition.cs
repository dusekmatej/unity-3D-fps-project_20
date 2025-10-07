using UnityEngine;

namespace Horse
{
    public class ChangeHorsePlaceholderPosition : MonoBehaviour
    {
        public GameObject horse;
        public GameObject horsePlaceHolder;
        void Update()
        {
            horsePlaceHolder.transform.position = horse.transform.position - new Vector3(0, 1.5f, 0f);
        }
    }
}
