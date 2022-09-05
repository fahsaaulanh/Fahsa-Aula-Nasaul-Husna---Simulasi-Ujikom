using System.Collections.Generic;
using UnityEngine;

namespace MatchPicture.Global.Theme
{
    [System.Serializable]
    [CreateAssetMenu]
    public class ThemeDatabase : ScriptableObject
    {
        public List<string> ThemeList;
    }
}