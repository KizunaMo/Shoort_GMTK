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
                }

                return instance;
            }
        }
    }


    public interface IInitializable
    {
        void Initialize();
    }
}