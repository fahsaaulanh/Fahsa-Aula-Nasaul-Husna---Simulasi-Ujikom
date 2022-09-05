using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchPicture.Scene.Gameplay
{
    public class TileGroup : MonoBehaviour
    {
        [SerializeField] TileObject _tilePrefabs;
        [SerializeField] Transform _picSpawnPos;
        [SerializeField] Vector3 _startPos;
        [SerializeField] Vector2 _offset;
        [SerializeField] int rows = 5;
        [SerializeField] int columns = 7;

        public enum GameState
        {
            NoAction,
            MovingOnPositions,
            DeletingPuzzles,
            FlipBack,
            Checking,
            GameEnd
        }

        public enum PuzzleState
        {
            PuzzleRotating,
            CanRotate
        }

        public enum RevealedState
        {
            NoRevealed,
            OnRevealed,
            TwoRevealed,
        }



        [HideInInspector]
        public GameState CurrentGameState;
        [HideInInspector]
        public PuzzleState CurrentPuzzleState;
        [HideInInspector]
        public RevealedState PuzzleRevealedNumber;

        private List<Material> _materialList = new List<Material>();
        private List<string> _texturePathList = new List<string>();
        private Material _firstMaterial;
        private string _firstTexturePath;

        private int _firstRevealedPic;
        private int _secondRevealedPic;
        private int _revealedPicNumber = 0;
        private int _picToDestroy1;
        private int _picToDestroy2;
        private int _removePairs;
        private bool _corutineStarted = false;

        [HideInInspector]
        public List<TileObject> _tiles;

        private void Start()
        {
            CurrentGameState = GameState.NoAction;
            CurrentPuzzleState = PuzzleState.CanRotate;
            PuzzleRevealedNumber = RevealedState.NoRevealed;
            _revealedPicNumber = 0;
            _firstRevealedPic = -1;
            _secondRevealedPic = -1;

            _removePairs = 0;

            LoadMaterials();

            CurrentGameState = GameState.MovingOnPositions;
            SpawnTileMesh(rows, columns);
            MoveTile(rows, columns, _startPos, _offset);
        }
        void Update()
        {
            if (CurrentGameState == GameState.DeletingPuzzles)
            {
                if (CurrentPuzzleState == PuzzleState.CanRotate)
                {
                    DestroyPicture();
                }
            }

            if (CurrentGameState == GameState.FlipBack)
            {
                if (CurrentPuzzleState == PuzzleState.CanRotate && _corutineStarted == false)
                {
                    StartCoroutine(FlipBack());
                }
            }

        }

        public void CheckPicture()
        {
            CurrentGameState = GameState.Checking;
            _revealedPicNumber = 0;

            for (int id = 0; id < _tiles.Count; id++)
            {
                if (_tiles[id].Revealed && _revealedPicNumber < 2)
                {
                    if (_revealedPicNumber == 0)
                    {
                        _firstRevealedPic = id;
                        _revealedPicNumber++;
                    }
                    else if (_revealedPicNumber == 1)
                    {
                        _secondRevealedPic = id;
                        _revealedPicNumber++;
                    }
                }

            }

            if (_revealedPicNumber == 2)
            {
                if (_tiles[_firstRevealedPic].GetIndex() == _tiles[_secondRevealedPic].GetIndex() && _firstRevealedPic != _secondRevealedPic)
                {
                    CurrentGameState = GameState.DeletingPuzzles;
                    _picToDestroy1 = _firstRevealedPic;
                    _picToDestroy2 = _secondRevealedPic;
                }
                else
                {
                    CurrentGameState = GameState.FlipBack;
                }

            }

            CurrentPuzzleState = TileGroup.PuzzleState.CanRotate;

            if (CurrentGameState == GameState.Checking)
            {
                CurrentGameState = GameState.NoAction;
            }
        }

        private void DestroyPicture()
        {
            PuzzleRevealedNumber = RevealedState.NoRevealed;
            _tiles[_picToDestroy1].Deactivate();
            _tiles[_picToDestroy2].Deactivate();
            _revealedPicNumber = 0;
            _removePairs++;
            CurrentGameState = GameState.NoAction;
            CurrentPuzzleState = PuzzleState.CanRotate;

        }

        private IEnumerator FlipBack()
        {
            _corutineStarted = true;
            yield return new WaitForSeconds(0.5f);

            _tiles[_firstRevealedPic].FlipBack();
            _tiles[_secondRevealedPic].FlipBack();

            _tiles[_firstRevealedPic].Revealed = false;
            _tiles[_secondRevealedPic].Revealed = false;

            PuzzleRevealedNumber = RevealedState.NoRevealed;
            CurrentGameState = GameState.NoAction;

            _corutineStarted = false;
        }

        private void LoadMaterials()
        {
            var materialFilePath = "Materials/";
            var textueFilePath = "Graphics/Fruit/";
            var pairNumber = (rows * columns) / 2;
            const string matBaseName = "Pic";
            var firstMaterialName = "Back";

            for (var index = 1; index <= pairNumber; index++)
            {
                var currentFIlePath = materialFilePath + matBaseName + index;
                Material mat = Resources.Load(currentFIlePath, typeof(Material)) as Material;
                _materialList.Add(mat);

                var currentTextureFilePath = textueFilePath + matBaseName + index;
                _texturePathList.Add(currentTextureFilePath);


                _firstTexturePath = textueFilePath + firstMaterialName;
                _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
            }

        }

        public void ApplyTextures()
        {
            var rndMatIndex = Random.Range(0, _materialList.Count);
            var AppliedTimes = new int[_materialList.Count];

            for (int i = 0; i < _materialList.Count; i++)
            {
                AppliedTimes[1] = 0;
            }

            foreach (var o in _tiles)
            {
                var rndPrevious = rndMatIndex;
                var counter = 0;
                var forceMat = false;

                while (AppliedTimes[rndMatIndex] >= 2 || ((rndPrevious == rndMatIndex) && !forceMat))
                {
                    rndMatIndex = Random.Range(0, _materialList.Count);
                    counter++;
                    if (counter > 100)
                    {
                        for (var j = 0; j < _materialList.Count; j++)
                        {
                            if (AppliedTimes[j] < 2)
                            {
                                rndMatIndex = j;
                                forceMat = true;
                            }
                        }
                        if (forceMat == false)
                        {
                            return;
                        }
                    }

                }

                o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
                o.ApplyFirstMaterial();
                o.SetSecondtMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);
                o.SetIndex(rndMatIndex);
                o.Revealed = false;
                AppliedTimes[rndMatIndex] += 1;
                forceMat = false;
            }
        }

        private void SpawnTileMesh(int r, int c)
        {
            for (int col = 0; col < c; col ++)
            {
                for (int row = 0; row < r; row++)
                {
                    var temp = (TileObject)Instantiate(_tilePrefabs, _picSpawnPos.position, _tilePrefabs.transform.rotation);
                    temp.name = "tile" + row + col;
                    _tiles.Add(temp);
                }
            }
            ApplyTextures();
        }

        private void MoveTile(int r, int c, Vector3 pos, Vector2 offset )
        {
            var index = 0;
            for (int col = 0; col < c; col++)
            {
                for (int row = 0; row < r; row++)
                {
                    var targetPostion = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                    StartCoroutine(MoveToPosition(targetPostion, _tiles[index]));
                    index++;
                }
            }
        }

        private IEnumerator MoveToPosition(Vector3 target, TileObject tile)
        {
            var randomDis = 7;
            while(tile.transform.position != target)
            {
                tile.transform.position = Vector3.MoveTowards(tile.transform.position, target, randomDis * Time.deltaTime);
                yield return 0;
            }
        }
    }
}
