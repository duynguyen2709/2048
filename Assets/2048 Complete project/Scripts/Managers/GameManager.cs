using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    GameOver,
    WaitingForMoveToEnd
}

public class GameManager : MonoBehaviour
{
    #region FieldDeclaration
    public GameState state;

    [Range(0, 2f)]
    public float delay;
    private bool moveMade;
    private bool[] lineMoveComplete = new bool[4] { true, true, true, true };

    public GameObject youWinText;
    public GameObject gameOverText;
    public Text gameOverScoreText;
    public GameObject gameOverPanel;

    // sound buttons:
    public GameObject btSoundOff;
    public GameObject btSoundOn;

    private Tile[,] AllTiles = new Tile[4, 4];

    private List<Tile[]> subDiagon = new List<Tile[]>();
    private List<Tile[]> mainDiagon = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile[]> columns = new List<Tile[]>();

    private List<Tile> EmptyTiles = new List<Tile>();

    [HideInInspector]
    public SoundManager soundManager;
    #endregion

    // Use this for initialization
    void Start()
    {
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        foreach (Tile tile in AllTilesOneDim)
        {
            tile.Number = 0;
            AllTiles[tile.indRow, tile.indCol] = tile;
            EmptyTiles.Add(tile);
        }


        #region AddTiles

        subDiagon.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0] });
        subDiagon.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1] });
        subDiagon.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2] });
        subDiagon.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3] });

        mainDiagon.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });
        mainDiagon.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3] });
        mainDiagon.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3] });
        mainDiagon.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3] });

        columns.Add(new Tile[] { AllTiles[3, 0] });
        columns.Add(new Tile[] { AllTiles[2, 0], AllTiles[3, 1] });
        columns.Add(new Tile[] { AllTiles[1, 0], AllTiles[2, 1], AllTiles[3, 2] });
        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 1], AllTiles[2, 2], AllTiles[3, 3] });
        columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 2], AllTiles[2, 3] });
        columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 3] });
        columns.Add(new Tile[] { AllTiles[0, 3] });

        rows.Add(new Tile[] { AllTiles[0, 0] });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[0, 1] });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[1, 1], AllTiles[0, 2] });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[2, 1], AllTiles[1, 2], AllTiles[0, 3] });
        rows.Add(new Tile[] { AllTiles[3, 1], AllTiles[2, 2], AllTiles[1, 3] });
        rows.Add(new Tile[] { AllTiles[3, 2], AllTiles[2, 3] });
        rows.Add(new Tile[] { AllTiles[3, 3] });
        #endregion

        Generate();
        Generate();
    }



    #region ButtonHandler
    public void NewGameButtonHandler()
    {
        //		Application.LoadLevel(Application.loadLevel);

        PlaySound(soundManager.clickButton, 1.0f);

        StartCoroutine(WaitForLoadScene(0.5f, "2048 Complete project/Scenes/mainScene"));

    }

    public void CheckSoundFxHandler()
    {

        soundManager.ToggleFx();

        if (!btSoundOff.activeSelf)
        {
            btSoundOff.SetActive(true);
        }
        else
        {
            btSoundOff.SetActive(false);
        }

        if (!btSoundOn.activeSelf)
        {
            btSoundOn.SetActive(true);
        }
        else
        {
            btSoundOn.SetActive(false);
        }
    }

    #endregion

    public void Move(MoveDirection moveDirection)
    {

        bool moveMade = false;

        ResetMergedFlags();

        if (moveDirection == MoveDirection.Down || moveDirection == MoveDirection.Up ||
            moveDirection == MoveDirection.Right || moveDirection == MoveDirection.Left)
        {
            for (int i = 0 ; i < columns.Count ; i++)
            {
                switch (moveDirection)
                {
                    case MoveDirection.Left:
                        while (MakeOneMoveDownIndex(rows [i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Right:
                        while (MakeOneMoveUpIndex(rows [i]))
                        {
                            moveMade = true;
                        }
                        break;

                    case MoveDirection.Down:
                        while (MakeOneMoveUpIndex(columns [i]))
                        {
                            moveMade = true;
                        }
                        break;

                    case MoveDirection.Up:
                        while (MakeOneMoveDownIndex(columns [i]))
                        {
                            moveMade = true;
                        }
                        break;
                }
            }

        }
        else
        {


            for (int i = 0 ; i < mainDiagon.Count ; i++)
            {
                switch (moveDirection)
                {
                    case MoveDirection.DownLeft:
                        while (MakeOneMoveUpIndex(subDiagon [i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.UpLeft:
                        while (MakeOneMoveDownIndex(subDiagon [i]))
                        {
                            moveMade = true;
                        }
                        break;

                    case MoveDirection.UpRight:
                        while (MakeOneMoveDownIndex(mainDiagon [i]))
                        {
                            moveMade = true;
                        }
                        break;

                    case MoveDirection.DownRight:
                        while (MakeOneMoveUpIndex(mainDiagon [i]))
                        {
                            moveMade = true;
                        }
                        break;


                }

            }

        }
        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();

            if (!CanMove())
            {
                GameOver();
            }
        }
    }


    private void Generate()
    {
        if (EmptyTiles.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, EmptyTiles.Count);
            int randomNum = Random.Range(0, 10);

            if (randomNum == 0)
            {
                EmptyTiles[indexForNewNumber].Number = 4;
            }
            else
            {
                EmptyTiles[indexForNewNumber].Number = 2;
            }

            EmptyTiles[indexForNewNumber].PlayAppearAnimation();
            EmptyTiles.RemoveAt(indexForNewNumber);
        }
    }

    private bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length - 1; i++)
        {
            // MOVE BLOCK
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i + 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;

                return true;
            }

            // MERGE BLOCK
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i + 1].Number &&
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i + 1].mergedThisTurn == false)
            {

                LineOfTiles[i].Number *= 2;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker.instance.Score += LineOfTiles[i].Number;

                if (LineOfTiles[i].Number == 2048)
                {
                    YouWin();
                }

                return true;
            }
        }
        return false;
    }

    private bool MakeOneMoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length - 1; i > 0; i--)
        {
            // MOVE BLOCK
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;

                return true;
            }

            // MERGE BLOCK
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number &&
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i - 1].mergedThisTurn == false)
            {

                LineOfTiles[i].Number *= 2;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker.instance.Score += LineOfTiles[i].Number;


                if (LineOfTiles[i].Number == 2048)
                {
                    YouWin();
                }

                return true;
            }
        }
        return false;
    }


    private void ResetMergedFlags()
    {
        foreach (Tile tile in AllTiles)
        {
            tile.mergedThisTurn = false;
        }
    }

    private void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();

        foreach (Tile tile in AllTiles)
        {
            if (tile.Number == 0)
            {
                EmptyTiles.Add(tile);
            }
        }
    }

    private void YouWin()
    {
        gameOverText.SetActive(false);
        youWinText.SetActive(true);
        gameOverScoreText.text = ScoreTracker.instance.Score.ToString();
        gameOverPanel.SetActive(true);

        state = GameState.GameOver;

        PlaySound(soundManager.gameWin, 1.0f);
    }

    private void GameOver()
    {
        gameOverScoreText.text = ScoreTracker.instance.Score.ToString();
        gameOverPanel.SetActive(true);

        state = GameState.GameOver;

        PlaySound(soundManager.gameOver, 1.0f);
    }

    private bool CanMove()
    {
        if (EmptyTiles.Count > 0)
        {
            return true;
        }
        else
        {
            // check columns:
            for (int i = 0; i < subDiagon.Count; i++)
            {
                for (int j = 0; j < mainDiagon.Count - 1; j++)
                {
                    if (AllTiles[j, i].Number == AllTiles[j + 1, i].Number)
                    {
                        return true;
                    }
                }
            }

            // check rows:
            for (int i = 0; i < mainDiagon.Count; i++)
            {
                for (int j = 0; j < subDiagon.Count - 1; j++)
                {
                    if (AllTiles[i, j].Number == AllTiles[i, j + 1].Number)
                    {
                        return true;
                    }
                }
            }
            
        }

        return false;
    }

    // play FxSound function:
    public void PlaySound(AudioClip clip, float volMultiplier = 1.0f)
    {
        if (clip && soundManager.fxEnable)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    private IEnumerator WaitForLoadScene(float waitTime, string sceneName)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneName);
    }
}

