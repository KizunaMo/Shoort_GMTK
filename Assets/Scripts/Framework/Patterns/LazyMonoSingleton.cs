using UnityEngine;

namespace Framework.Patterns
{
    public class LazyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = FindFirstObjectByType<T>();

                if (instance == null)
                {
                    GameObject obj = new GameObject($"[Singleton]::{typeof(T).Name}");
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);

                    if (instance is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }

                }

                return instance;
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }


    public interface IInitializable
    {
        void Initialize();
    }
}