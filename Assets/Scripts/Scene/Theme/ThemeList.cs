using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatchPicture.Global.Theme;

namespace MatchPicture.Scene.Theme
{
    public class ThemeList : MonoBehaviour
    {
        [SerializeField] private ThemeButton _themePrefabBtn;
        [SerializeField] private ThemeDatabase _themeData;

        private void Start()
        {
            SpawnTheme();
        }

        private void SpawnTheme()
        {
            for(int i = 0; i < _themeData.ThemeList.Count; i++)
            {
                var obj = (ThemeButton)Instantiate(_themePrefabBtn, parent: gameObject.transform);

                string themeName = _themeData.ThemeList[i];
                obj.SetThemeName(themeName);
            }
        }
    }
}