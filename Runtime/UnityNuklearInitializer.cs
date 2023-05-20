using UnityEngine;

namespace Chutpot.Nuklear.Loader
{
    public static class UnityNuklearInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethodAfterSceneLoad() 
        {
            Debug.developerConsoleVisible = true;
            GameObject loader = MonoBehaviour.Instantiate(Resources.Load("UnityNuklearLoader", typeof(GameObject))) as GameObject;
            MonoBehaviour.DontDestroyOnLoad(loader);
        }
    }
}
