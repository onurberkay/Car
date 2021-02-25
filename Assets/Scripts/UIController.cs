using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public GameObject carCount;
    public GameObject restart;
    public GameObject won;
    public GameObject fail;
    private bool once = true;
    private int count;
    void Start()
    {
        count = GameObject.FindGameObjectsWithTag("Car").Length;
        
    }

    // Update is called once per frame
    void Update()
    {
        carCount.GetComponent<TextMeshPro>().SetText("Cars " + count);


        if (count == 1)
        {
            if (once)
            {
                once = false;
                restart.SetActive(true);
                won.SetActive(true);
            }
        }

    }

    public void Fail()
    {
        if (once) {
            once = false;
            restart.SetActive(true);
            fail.SetActive(true);
        }
        
    }
    public void CountDown()
    {
        count--;
    }
    public void RestartGame()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

}
