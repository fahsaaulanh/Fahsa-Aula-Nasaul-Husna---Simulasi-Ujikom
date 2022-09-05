using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MatchPicture.Global.SaveData;
using UnityEngine.Events;

namespace MatchPicture.Scene.Theme
{
    public class ThemeButton : MonoBehaviour
    {
        public static UnityAction OnButtonClick;

        [SerializeField] private TMP_Text _themeNameText;
        [SerializeField] private Button _button;
        [SerializeField] private SaveData _saveData;
        private string _themeName;

        private void Start()
        {
            _button.onClick.AddListener(SetCurrentTheme);
            _themeNameText.text = _themeName;
        }

        private void SetCurrentTheme()
        {
            _saveData.SelectedCurrentTheme = _themeName;
            OnButtonClick?.Invoke();
        }

        public string SetThemeName(string name)
        {
            _themeName = name;
            return _themeName;
        }
    }
}