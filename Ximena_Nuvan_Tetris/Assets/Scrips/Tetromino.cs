using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{ //Code tutorials Reference The Weekly Coder. Youtube Videos.
    static float fall = 0;
    public float fallspeed = 1; //fall 1sc
    public bool allowRotation= true;
    public bool LimitRotation= false;
    public Game game;
    public int individualScore = 100;
    private float individualScoreTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput(); //Call the function
        UpdateIndividualScore();
    }

    public void UpdateIndividualScore()//set score per time
    {
        if (individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;
        }
        else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0); //avoid drops below 0
        }


    }


    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            } else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        } 
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (LimitRotation)// limit rotation
                {
                    if(transform.rotation.eulerAngles.z  >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
           
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            } else
            {
                if (LimitRotation)//just apply if have a limit rotation
                {
                    if (transform.rotation.eulerAngles.z >= 90)//CHECK to the  position
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }else
                {
                    transform.Rotate(0, 0, -90); //else just do a normal rotation
                }
               
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallspeed) //move the piece down by 1
        {
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                FindFirstObjectByType<Game>().DeleteRow();

                if (FindObjectOfType<Game>().CheckIsAboveGrid(this))//Check all the minos
                {
                    FindObjectOfType<Game>().GameOver();//if is true = game over
                }
                if (gameObject.name.Contains("TetrominoTExtra"))
                {

                }
                //spawn next Piece
                FindObjectOfType<Game>().SpawnNextTetromino(); //Call function
                Game.currentScore += individualScore;//check how long took the player set that piece
                enabled = false; //disable this actual piece
            }
            fall = Time.time; //update the fall timer with the current fall time
        }
        
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform) 
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
               return false;
            }
            if (FindObjectOfType<Game>().GetTransFormAtGridPosition(pos) != null &&
                FindObjectOfType<Game>().GetTransFormAtGridPosition(pos).parent != transform)
            {
                return false; //piece can't move
            }
        }
        return true;
    }
}
