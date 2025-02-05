using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{ //Code tutorials Reference The Weekly Coder. Youtube Videos.
    public void PlayAgain()
    {
       SceneManager.LoadScene("Level");
    }
}
