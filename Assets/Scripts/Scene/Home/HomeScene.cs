using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace MatchPicture.Scene.Home
{
    public class HomeScene : MonoBehaviour
    {
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _themeBtn;

        private void Start()
        {
            _playBtn.onClick.AddListener(GameplayLauncher);
            _themeBtn.onClick.AddListener(ThemeLauncher);
        }

        private void GameplayLauncher()
        {
            SceneManager.LoadScene("Gameplay");
        }

        private void ThemeLauncher()
        {
            SceneManager.LoadScene("Theme");
        }

    }
}