using TMPro;
using UnityEngine;

namespace MatchPicture.Scene.Gameplay
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        private float _timeLeft = 120;
        private float _minutes;
        private float _seconds;
        private float _oneSecondDown;
        private bool _stopTimer;

        private void Start()
        {
            _stopTimer = false;
            _oneSecondDown = _timeLeft - 1f;
        }

        private void Update()
        {
            if (_stopTimer == false)
                _timeLeft -= Time.deltaTime;
            if (_timeLeft <= _oneSecondDown)
            {
                _oneSecondDown = _timeLeft - 1f;
            }

        }

        private void OnGUI()
        {
            if (_timeLeft > 0)
            {
                _minutes = Mathf.Floor(_timeLeft / 60);
                _seconds = Mathf.RoundToInt(_timeLeft % 60);
                _timerText.text = _minutes.ToString("00") + ":" + _seconds.ToString("00");
            }
            else
            {
                _stopTimer = true;
            }
        }
    }
}