using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchPicture.Global.SaveData
{
    [System.Serializable]
    [CreateAssetMenu]
    public class SaveData : ScriptableObject
    {
        public string SelectedCurrentTheme = "Fruit";
        public List<string> ThemeAvailable;
    }
}