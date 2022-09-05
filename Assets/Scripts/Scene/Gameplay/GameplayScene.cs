using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatchPicture.Scene.Gameplay
{
    public class GameplayScene : MonoBehaviour
    {
        [SerializeField] private Button _backBtn;

        private void Start()
        {
            _backBtn.onClick.AddListener(HomeLauncher);
        }

        private void HomeLauncher()
        {
            SceneManager.LoadScene("Home");
        }
    }
}
