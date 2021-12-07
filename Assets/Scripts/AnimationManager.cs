using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public UnityEngine.UI.Button buttonStart;

    float timer;
    // second to set timer, default should be 60 seconds
    public float seconds;

    // array to store cubes
    public GameObject[] cubes;
 
    int num = 0;

    // stop timer when training is finished
    bool training_start = false;


    // Start is called before the first frame update
    void Start()
    {
        //cubes = new GameObject[num_cubes];
        //angles = new int[4] { 0, 50, 100, 150};
        cubes[0].gameObject.SetActive(false);
        cubes[1].gameObject.SetActive(false);
        cubes[2].gameObject.SetActive(false);
        cubes[3].gameObject.SetActive(false);
        cubes[4].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // training start
        if(Input.GetKeyDown(KeyCode.Return))
        {
            TrainingStart();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            //Debug.Log("exit game");
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }

        // time limit should be 1 minute
        if (Time.time - timer > seconds && training_start)
        {
            timer = timer + seconds;
            cubes[num].gameObject.SetActive(false);
            num++;
            //when num reaches last cube, stop training
            if (num == 5)
            {
                training_start = false;

            }
            else
            {
                cubes[num].gameObject.SetActive(true);
                // automatically starts rotation
                StartCoroutine(cubes[num].GetComponent<RotateAnim>().RotateCube());
            }

        }
    }

    public void TrainingStart()
    {
        //Debug.Log("start");
        cubes[0].gameObject.SetActive(true);
        StartCoroutine(cubes[0].GetComponent<RotateAnim>().RotateCube());
        buttonStart.gameObject.SetActive(false);
        training_start = true;
        timer = Time.time;
    }
}
