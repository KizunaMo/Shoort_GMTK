using UnityEngine;

namespace ManagerDomain
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            InitializeAllModule();
        }

        private void InitializeAllModule()
        {
            var uiManager = UIManager.Instance;
            
            
            
            
#if UNITY_EDITOR
            var amo = Amo.Instance;
#endif
        }
    }
}