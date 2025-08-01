using Framework.CustomAttribute;
using Framework.Interface;
using UnityEngine;

namespace Tool
{
    public interface IRequestComponent
    {
        public T GetTargetComponent<T>() where T : Component;
        public Component GetTargetComponent();
    }

    public class IdentifyComponent : MonoBehaviour, IVariableKeyInInspector<string>, IRequestComponent
    {
        public string VariableKey => inspectorKey;

        [SerializeField] private string inspectorKey;

        [ComponentType][SerializeField] private string targetComponentType;

        [SerializeField] private Component cachedComponent;

        public T GetTargetComponent<T>() where T : Component
        {
            return cachedComponent as T;
        }

        public Component GetTargetComponent()
        {
            return cachedComponent;
        }
    }
}