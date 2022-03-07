using UnityEngine;

namespace GameBrains.Entities
{
    public class DestroyOnContact : MonoBehaviour
    {
        [SerializeField] float timeToDelay;
        public void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject, timeToDelay);
        }
    }
}