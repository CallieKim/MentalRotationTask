using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAnimationManager : MonoBehaviour
{
    public UnityEngine.UI.Button buttonStart;
    public GameObject pairState;

    float timer;
    // second to set timer, default should be 60 seconds
    public float seconds;

    // array to store cubes
    public GameObject[] cubes;
    public GameObject[] cubes_noWire;
    GameObject pair;
    GameObject pair_noWire;
    // num to use in array
    int num = 0;

    // stop timer when training is finished
    bool training_start = false;
    // if mirrored should be true
    bool mirrored = false;

    // Start is called before the first frame update
    void Start()
    {
        //cubes = new GameObject[num_cubes];
        //angles = new int[4] { 0, 50, 100, 150};
        cubes[0].gameObject.SetActive(false);
        cubes[1].gameObject.SetActive(false);
        cubes[2].gameObject.SetActive(false);
        cubes_noWire[0].gameObject.SetActive(false);
        cubes_noWire[1].gameObject.SetActive(false);
        cubes_noWire[2].gameObject.SetActive(false);
        pairState.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
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
            cubes_noWire[num].gameObject.SetActive(false);
            pair.gameObject.SetActive(false);
            pair_noWire.gameObject.SetActive(false);
            num++;
            //when num reaches last cube, stop training
            if (num == 3)
            {
                training_start = false;
                pairState.GetComponent<Text>().text = "";

            }
            else
            {
                cubes[num].gameObject.SetActive(true);
                cubes_noWire[num].gameObject.SetActive(true);
                createPair();
            }

        }
    }

    public void TrainingStart()
    {
        //Debug.Log("start");
        cubes[0].gameObject.SetActive(true);
        cubes_noWire[0].gameObject.SetActive(true);
        createPair();
        buttonStart.gameObject.SetActive(false);
        training_start = true;
        timer = Time.time;
    }

    // create pair of cube, angle and mirrored state should be random
    public void createPair()
    {
        pair = GameObject.Instantiate(cubes[num]);
        pair_noWire = GameObject.Instantiate(cubes_noWire[num]);
        pair.transform.position = new Vector3(7, 0, 0);
        pair_noWire.transform.position = new Vector3(7, 0, 0);
        // need to randomize mirrored and rotation
        // if mirrored is true, flip horizontally
        // random range is min inclusive and max exclusive
        float rot = Random.Range(0, 181);
        if(mirrored = (Random.value > 0.5f))
        {
            pair.transform.localScale = new Vector3(-1, 1, 1);
            pair.transform.rotation = Quaternion.Euler(0, rot, 0);
            pair_noWire.transform.localScale = new Vector3(-1, 1, 1);
            pair_noWire.transform.rotation = Quaternion.Euler(0, rot, 0);
            pairState.GetComponent<Text>().text = "Flipped "+rot.ToString()+" degrees";

        }
        else
        {
            pair.transform.rotation = Quaternion.Euler(0, rot, 0);
            pair_noWire.transform.rotation = Quaternion.Euler(0, rot, 0);
            pairState.GetComponent<Text>().text = "Rotated " + rot.ToString() + " degrees";
        }
    }
}
