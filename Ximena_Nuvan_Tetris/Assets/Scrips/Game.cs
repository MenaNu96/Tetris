using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{ //Code tutorials Reference The Weekly Coder. Youtube Videos.
    public int individualScore = 100;
    private float individualScoreTime;
    /// The width of the grid
    /// </summary>
    public static int GridWidth = 10;
   
    /// The Height of the grid
    /// </summary>
    public static int GridHeight = 20;
    /// The grid
    /// </summary>
    public static Transform[,] grid = new Transform [GridWidth, GridHeight];

    /// Set Score
    /// </summary>
    public int ScoreOneLine = 40;
    public int ScoreTwoLines = 100;
    public int ScoreThreeLines = 300;
    public int ScoreFourLines = 1000;
    public int SpecialScore = 300;
    
    /// Score Text
    /// </summary>
    public Text Hud_Score;
    public int NumerOfRowsThisTurn = 0;
    public static int currentScore = 0;
     
     /// Next Piece Visualize
     /// </summary>
    private GameObject PreviewTetromino;
    private GameObject NextTetromino;
    private bool gameStarted = false;
    private Vector2 PreviewTetrominoPos = new Vector2(-6.5f, 15);
    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino(); //Call the game method
    }
    void Update()
    {
        UpdateScore();
        UpdateUi();
    
    }

    public void UpdateScore()
    {
        if (NumerOfRowsThisTurn > 0)
        {
            if (NumerOfRowsThisTurn ==1)
            {
                ClearedOneLine();
            } 
            else if (NumerOfRowsThisTurn ==2)
            {
                ClearedTwoLines();
            }
            else if (NumerOfRowsThisTurn ==3)
            {
                ClearedThreeLines();
            }
            else if(NumerOfRowsThisTurn ==4)
            {
                ClearedFourLines();
            }

            NumerOfRowsThisTurn = 0;
        }
    }
    public void UpdateUi()
    {
        Hud_Score.text = currentScore.ToString();
    }
    public void ClearedOneLine ()
    {
        currentScore += ScoreOneLine;
    }
    public void ClearedTwoLines()
    {
        currentScore += ScoreTwoLines;
    }
    public void ClearedThreeLines()
    {
        currentScore += ScoreThreeLines;
    }
    public void ClearedFourLines()
    {
        currentScore += ScoreFourLines;
    }
    /// Check above grid...
    /// </summary>
    public bool CheckIsAboveGrid(Tetromino tetromino)
    {
        for (int x = 0; x < GridWidth; x++)
        {
            foreach (Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);
                if (pos.y > GridHeight - 1)
                {
                    return true; //Check
                }
            }
        }
        return false; //Dont have any minos above the grid
    }

    /// Check Row is full or No
    /// </summary>
    public bool IsFullRowAt (int y)
    {
        for (int x = 0; x < GridWidth; x++)
        {
            if (grid[x, y]== null)
            {
                return false;
            }
        }
        //we found a full row +  row variable
        NumerOfRowsThisTurn++;
        return true; 
    }

    public void DeleteMinoAt(int y)
    {
        for(int x = 0;x < GridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

       
    }
    public void specialScore(int y )
    {
        foreach (Transform child in transform)
        {
            if (child.position.y == y && child.CompareTag("Special"))
            {
                currentScore += SpecialScore; // Extra Points 50
                Debug.Log("Extra Bonus");
            }

        }
    }
    public void MoveRowDown(int y)
    {
        for (int x =0;x < GridWidth; x++)
        {
            if (grid[x, y] != null) 
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0); //moving the row down update
            }
        }
    }
    /// Move All Minos Down
    /// </summary>
    public void MoveAllRowDown(int y) 
    {
        for (int i = y; i < GridHeight; i++) 
        {
            MoveRowDown(i);
        }
    }
    /// Sumarize all the functions
    /// </summary>
    public void DeleteRow() //sumarize all the process
    {
        for (int y = 0; y < GridHeight; ++y) //checking all row if are full, if the row if full -> delete it 
        {
            if (IsFullRowAt(y))
            {
                specialScore(y);
                DeleteMinoAt(y); //delete all the minos on that row
                MoveAllRowDown(y + 1); //Move all rows down
                --y; //Because that row is full Y dicrease
            }
        }
    }
    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                if (grid[x, y]!= null)
                {
                    if (grid[x,y].parent == tetromino.transform)
                    {
                        grid[x, y] = null; //that position is null
                    }
                }
            }
        }

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < GridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino; 
            }
        }
    }

    public Transform GetTransFormAtGridPosition(Vector2 pos) 
    {
        if (pos.y > GridHeight - 1)
        {
            return null;

        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }
    public void SpawnNextTetromino()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            NextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)),
           new Vector2(5.0f, 20.0f), Quaternion.identity);//Tetrominoes always going to spawn on vector2 pos, Rotation set to current rotation
            PreviewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(),typeof(GameObject)), PreviewTetrominoPos, Quaternion.identity);
            PreviewTetromino.GetComponent<Tetromino>().enabled = false;
        }
        else
        {
            PreviewTetromino.transform.localPosition = new Vector2(5.0f, 20.0f);
            NextTetromino = PreviewTetromino;
            NextTetromino.GetComponent<Tetromino>().enabled=true;

            PreviewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), PreviewTetrominoPos, Quaternion.identity);
            PreviewTetromino.GetComponent<Tetromino>().enabled = false;
        }

        

    }

    /// Check is inside grid...
    /// </summary>
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return((int)pos.x >= 0 && (int)pos.x < GridWidth && (int) pos.y >= 0); //Check if is inside grid
    }
    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    string GetRandomTetromino()
    {
        int randomTetromino = Random.Range(1,9); //random Range
        string randomTetrominoName = "Prefabs/TetrominoT"; //Found the object inside the prefabs folder
       
        switch (randomTetromino)
        {
                case 1:
                randomTetrominoName = "Prefabs/TetrominoI"; //Found the object inside the prefabs folder
                break;
                case 2:
                randomTetrominoName = "Prefabs/TetrominoJ"; //Found the object inside the prefabs folder
                break;
                case 3:
                randomTetrominoName = "Prefabs/TetrominoL"; //Found the object inside the prefabs folder
                break;
                case 4:
                randomTetrominoName = "Prefabs/TetrominoO"; //Found the object inside the prefabs folder
                break;
                case 5:
                randomTetrominoName = "Prefabs/TetrominoS"; //Found the object inside the prefabs folder
                break;
                case 6:
                randomTetrominoName = "Prefabs/TetrominoZ"; //Found the object inside the prefabs folder
                break;
                case 7:
                randomTetrominoName = "Prefabs/TetrominoTExtra"; //Found the object inside the prefabs folder
                break;
                case 8:
                randomTetrominoName = "Prefabs/TetrominoT"; //Found the object inside the prefabs folder
                break;
        }
        return randomTetrominoName;
    }

    public void GameOver() //Game over method Load game over scene
    {
       SceneManager.LoadScene("GameOver");
    }
}
