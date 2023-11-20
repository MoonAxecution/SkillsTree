using UnityEngine.SceneManagement;

namespace Game
{
    public static class ScenesLoader
    {
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}