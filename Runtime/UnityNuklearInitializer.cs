using UnityEngine;

namespace Chutpot.Nuklear.Loader
{
    public static class UnityNuklearInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethodAfterSceneLoad() 
        {
            Debug.developerConsoleVisible = false;
            GameObject loader = MonoBehaviour.Instantiate(Resources.Load("UnityNuklearManager", typeof(GameObject))) as GameObject;
        }
    }
}
