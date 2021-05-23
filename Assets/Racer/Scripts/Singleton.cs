using UnityEngine;

namespace Racer
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Two or more singletons!", Instance);
                Destroy(Instance);
            }
            Instance = (T) this;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}