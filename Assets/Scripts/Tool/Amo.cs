#if UNITY_EDITOR
using Framework.Patterns;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Amo : LazyMonoSingleton<Amo>,IInitializable
{
    protected  void Awake()
    {
        EditorSceneManager.preventCrossSceneReferences = false;
    }

    [HideInCallstack]
    public void Log(object message)
    {
        Log(message, Color.yellow);
    }
    
    [HideInCallstack]
    public void Log(object message, Color color )
    {
        Debug.LogFormat("<color=#{0}>{1}</color>", UnityEngine.ColorUtility.ToHtmlStringRGB(color), message);
    }
    
    [HideInCallstack]
    public void Todo(object message)
    {
        Debug.Log($"<b><Color=red>ToDo:</color></b> <i>{message}</i>");
    }

    public void Initialize()
    {
        Log("Gua Gua ! \ud83d\udc38");
    }
}
#endif
