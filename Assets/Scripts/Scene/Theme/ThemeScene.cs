using MatchPicture.Global.SaveData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MatchPicture.Scene.Theme
{
    public class ThemeScene : MonoBehaviour
    {
        [SerializeField] private Button _backBtn;
        [SerializeField] private SaveData _saveData;
        [SerializeField] private TMP_Text _infoCurrentTheme;

        private void Start()
        {
            _backBtn.onClick.AddListener(HomeLauncher);
            SetInfoTheme();
        }

        private void OnEnable()
        {
            ThemeButton.OnButtonClick += SetInfoTheme;
        }

        private void OnDisable()
        {
            ThemeButton.OnButtonClick -= SetInfoTheme;
        }

        private void SetInfoTheme()
        { 
            _infoCurrentTheme.text = "Theme Selected: " + _saveData.SelectedCurrentTheme;
        }

        private void HomeLauncher()
        {
            SceneManager.LoadScene("Home");
        }
    }
}