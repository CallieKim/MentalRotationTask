using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{

    public UnityEngine.UI.Button buttonYes;
    public UnityEngine.UI.Button buttonNo;
    public UnityEngine.UI.Button buttonStart;
    public GameObject pairState;
    public GameObject question;

    float curr_time;
    float update_time;

    // randomizing trials
    public int trials;
    public int trial = 1;
    public int num_cubes = 8;
    public GameObject[] cubes;
    GameObject pair;
    public int[] angles;
    int num = 0;

    // when mirrored change to true, need this for checking answer also
    bool mirrored = false;
    public bool training_start = false;

    // data to save to csv file for data collection
    struct TrialData
    {
        public int trial;
        public GameObject cube;
        public float reactionTime;
        public int angle;
        // correct is true if selected the right answer
        public bool correct;
    }


    // Start is called before the first frame update
    void Start()
    {
        curr_time = 0;
        //cubes = new GameObject[num_cubes];
        //angles = new int[4] { 0, 50, 100, 150};
        cubes[0].gameObject.SetActive(false);
        cubes[1].gameObject.SetActive(false);
        cubes[2].gameObject.SetActive(false);
        cubes[3].gameObject.SetActive(false);
        cubes[4].gameObject.SetActive(false);
        buttonNo.gameObject.SetActive(false);
        buttonYes.gameObject.SetActive(false);
        pairState.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && training_start)
        {
            //Debug.Log("left");
            buttonYes.GetComponent<Image>().color = Color.red;

            question.GetComponent<Text>().text = "Are these two shapes the same except their rotations?";

            update_time = Time.time;

            if(mirrored)
            {
                Debug.Log("Wrong, reaction time : " + (update_time - curr_time).ToString());
            }
            else
            {
                Debug.Log("Correct, reaction time : " + (update_time - curr_time).ToString());
            }

            curr_time = update_time;

            cubes[num].gameObject.SetActive(false);
            pair.gameObject.SetActive(false);
            num++;
            if(num == 5)
            {
                pairState.GetComponent<Text>().text = "";
                if(trial < trials)
                {
                    trial++;
                    num = 0;
                    //question.GetComponent<Text>().text = "Press right or left to continue";
                    TrainingStart();
                }
                else
                {
                    training_start = false;
                }
            }
            else
            {
                cubes[num].gameObject.SetActive(true);
                createPair();
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            buttonYes.GetComponent<Image>().color = Color.white;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && training_start)
        {
            //Debug.Log("right");
            buttonNo.GetComponent<Image>().color = Color.red;

            question.GetComponent<Text>().text = "Are these two shapes the same except their rotations?";

            update_time = Time.time;

            if (mirrored)
            {
                Debug.Log("Correct, reaction time : " + (update_time - curr_time).ToString());
            }
            else
            {
                Debug.Log("Wrong, reaction time : " + (update_time - curr_time).ToString());
            }

            curr_time = update_time;

            cubes[num].gameObject.SetActive(false);
            pair.gameObject.SetActive(false);
            num++;
            if (num == 5)
            {
                pairState.GetComponent<Text>().text = "";
                if (trial < trials)
                {
                    trial++;
                    num = 0;
                    //question.GetComponent<Text>().text = "Press right or left to continue";
                    TrainingStart();
                }
                else
                {
                    training_start = false;
                }
            }
            else
            {
                cubes[num].gameObject.SetActive(true);
                createPair();
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            buttonNo.GetComponent<Image>().color = Color.white;
        }
        else if(Input.GetKey(KeyCode.Escape))
        {
            //Debug.Log("exit game");
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public void TrainingStart()
    {
        //Debug.Log("start");
        buttonNo.gameObject.SetActive(true);
        buttonYes.gameObject.SetActive(true);
        cubes[0].gameObject.SetActive(true);
        createPair();
        buttonStart.gameObject.SetActive(false);
        training_start = true;
        curr_time = Time.time;
    }

    // create pair of cube, angle and mirrored state should be random
    public void createPair()
    {
        pair = GameObject.Instantiate(cubes[num]);
        pair.transform.position = new Vector3(7, 0, 0);
        // need to randomize mirrored and rotation
        // if mirrored is true, flip horizontally
        // random range is min inclusive and max exclusive
        int rot_num = Random.Range(0, 5);
        if (mirrored = (Random.value > 0.5f))
        {
            pair.transform.localScale = new Vector3(-1, 1, 1);
            pair.transform.rotation = Quaternion.Euler(0, angles[rot_num], 0);
            pairState.GetComponent<Text>().text = "Flipped " + angles[rot_num].ToString() + " degrees";

        }
        else
        {
            pair.transform.rotation = Quaternion.Euler(0, angles[rot_num], 0);
            pairState.GetComponent<Text>().text = "Rotated " + angles[rot_num].ToString() + " degrees";
        }
    }
}
