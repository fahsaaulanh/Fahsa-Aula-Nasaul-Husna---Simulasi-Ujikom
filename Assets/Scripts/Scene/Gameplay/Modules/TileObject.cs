using System.Collections;
using UnityEngine;

namespace MatchPicture.Scene.Gameplay
{
    public class TileObject : MonoBehaviour
    {
        public AudioClip PressSound;
        private Material _firstMaterial;
        private Material _secondMaterial;

        private Quaternion _currentRotation;

        [HideInInspector] public bool Revealed = false;
        private TileGroup _tileGroup;
        private bool _clicked = false;
        private int _index;

        public void SetIndex(int id) { _index = id; }
        public int GetIndex() { return _index; }



        void Start()
        {
            Revealed = false;
            _clicked = false;
            _tileGroup = GameObject.Find("TileGroup").GetComponent<TileGroup>();
            _currentRotation = gameObject.transform.rotation;
        }


        void Update()
        {

        }

        private void OnMouseDown()
        {
            if (_clicked == false)
            {
                _tileGroup.CurrentPuzzleState = TileGroup.PuzzleState.PuzzleRotating;
                StartCoroutine(LoopRotation(45, false));
                _clicked = true;
            }
        }

        public void FlipBack()
        {
            if (gameObject.activeSelf)
            {
                _tileGroup.CurrentPuzzleState = TileGroup.PuzzleState.PuzzleRotating;
                Revealed = false;
                StartCoroutine(LoopRotation(45, true));
            }

        }

        IEnumerator LoopRotation(float angle, bool firstMat)
        {
            var rot = 0f;
            const float dir = 1f;
            const float rotSpeed = 100.0f;
            var startAngle = angle;
            var assigned = false;

            if (firstMat)
            {
                while (rot < angle)
                {
                    var step = Time.deltaTime * rotSpeed;
                    gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                    if (rot >= (startAngle - 2) && assigned == false)
                    {
                        ApplyFirstMaterial();
                        assigned = true;
                    }

                    rot += (1 * step * dir);
                    yield return null;
                }
            }
            else
            {
                while (angle > 0)
                {
                    float step = Time.deltaTime * rotSpeed;
                    gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                    angle -= (1 * step * dir);
                    yield return null;

                }
            }

            gameObject.GetComponent<Transform>().rotation = _currentRotation;

            if (!firstMat)
            {
                Revealed = true;
                ApplySecondMaterial();
                _tileGroup.CheckPicture();
            }
            else
            {
                _tileGroup.PuzzleRevealedNumber = TileGroup.RevealedState.NoRevealed;
                _tileGroup.CurrentPuzzleState = TileGroup.PuzzleState.CanRotate;
            }

            _clicked = false;
        }

        public void SetFirstMaterial(Material mat, string texturePath)
        {
            _firstMaterial = mat;
            Debug.Log(texturePath);
            _firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
        }

        public void SetSecondtMaterial(Material mat, string texturePath)
        {
            _secondMaterial = mat;
            _secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
        }

        public void ApplyFirstMaterial()
        {
            gameObject.GetComponent<Renderer>().material = _firstMaterial;
        }

        public void ApplySecondMaterial()
        {
            gameObject.GetComponent<Renderer>().material = _secondMaterial;
        }

        public void Deactivate()
        {
            StartCoroutine(DeactivateCorutine());
        }

        private IEnumerator DeactivateCorutine()
        {
            Revealed = false;
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
