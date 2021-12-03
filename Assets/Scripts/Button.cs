using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    public GameObject[] cubes_right;
    //public GameObject[] cubes2 = cubes.Clone();
    GameObject pair;
    public int[] angles;
    int num = 0;

    // when mirrored change to true, need this for checking answer also
    bool mirrored = false;
    public bool training_start = false;

    // data to save to csv file for data collection
    struct TrialData
    {
        //public int trial;
        //public GameObject cube;
        public float reactionTime;
        //public int angle;
        // correct is true if selected the right answer
        public bool correct;
    }

    public struct ObjectConfig
    {
        public GameObject gobject;
        public int rotation_degree;
        public bool flipped;
    }

    public struct ObjectPairConfig
    {
        public ObjectConfig leftObject;
        public ObjectConfig rightObject;
    }

    // linked list to store Trial data..
    LinkedList<TrialData> dataList = new LinkedList<TrialData>();
    TrialData data;

    // Shubamb
    LinkedList<ObjectPairConfig> configList;
    LinkedListNode<ObjectPairConfig> head;
    LinkedListNode<ObjectPairConfig> temp;

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
        data = new TrialData();
        configList = getPairList();
        //Debug.Log(configList.First.Value.leftObject.gobject);
        //Debug.Log(configList.First.Next.Value.leftObject.gobject);
    }

    // Update is called once per frame
    void Update()
    {
        // training start
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TrainingStart();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && training_start)
        {
            //Debug.Log("left");
            //buttonYes.GetComponent<Image>().color = Color.red;

            question.GetComponent<Text>().text = "Are these two shapes the same except their rotations?";

            update_time = Time.time;

            if(temp.Value.rightObject.gobject.transform.localScale.x * temp.Value.leftObject.gobject.transform.localScale.x == 1)
            {
                data.reactionTime = update_time - curr_time;
                data.correct = false;
                Debug.Log("Wrong, reaction time : " + (data.reactionTime).ToString());
                dataList.AddLast(data);
                buttonYes.GetComponent<Image>().color = Color.green;
            }
            else
            {
                data.reactionTime = update_time - curr_time;
                data.correct = false;
                Debug.Log("Correct, reaction time : " + (data.reactionTime).ToString());
                dataList.AddLast(data);
                buttonYes.GetComponent<Image>().color = Color.red;
            }

            curr_time = update_time;

            temp.Value.leftObject.gobject.gameObject.SetActive(false);
            temp.Value.rightObject.gobject.gameObject.SetActive(false);
            Debug.Log(temp.Value.leftObject.gobject);
            temp = temp.Next;
            if (temp == null)
            {
                training_start = false;
            }
            temp.Value.leftObject.gobject.gameObject.SetActive(true);
            temp.Value.leftObject.gobject.transform.rotation = Quaternion.Euler(0, temp.Value.leftObject.rotation_degree, 0);
            temp.Value.rightObject.gobject.gameObject.SetActive(true);
            temp.Value.rightObject.gobject.transform.position = new Vector3(3, 0, 12);
            if (temp.Value.rightObject.flipped)
            {
                temp.Value.rightObject.gobject.transform.localScale = new Vector3(temp.Value.rightObject.gobject.transform.localScale.x * -1, 1, 1);
            }
            temp.Value.rightObject.gobject.transform.rotation = Quaternion.Euler(0, temp.Value.rightObject.rotation_degree, 0);

        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            buttonYes.GetComponent<Image>().color = Color.white;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && training_start)
        {
            //Debug.Log("right");
            //buttonNo.GetComponent<Image>().color = Color.red;

            question.GetComponent<Text>().text = "Are these two shapes the same except their rotations?";

            update_time = Time.time;

            if (temp.Value.rightObject.gobject.transform.localScale.x * temp.Value.leftObject.gobject.transform.localScale.x == -1)
            {
                Debug.Log("Correct, reaction time : " + (update_time - curr_time).ToString());
                buttonNo.GetComponent<Image>().color = Color.green;
            }
            else
            {
                Debug.Log("Wrong, reaction time : " + (update_time - curr_time).ToString());
                buttonNo.GetComponent<Image>().color = Color.red;
            }

            curr_time = update_time;

            temp.Value.leftObject.gobject.gameObject.SetActive(false);
            temp.Value.rightObject.gobject.gameObject.SetActive(false);
            //Debug.Log(temp.Value.leftObject.gobject);
            temp = temp.Next;
            if(temp==null)
            {
                training_start = false;
            }
            temp.Value.leftObject.gobject.gameObject.SetActive(true);
            temp.Value.leftObject.gobject.transform.rotation = Quaternion.Euler(0, temp.Value.leftObject.rotation_degree, 0);
            temp.Value.rightObject.gobject.gameObject.SetActive(true);
            temp.Value.rightObject.gobject.transform.position = new Vector3(3, 0, 12);
            Debug.Log(temp.Value.rightObject.flipped);
            if (temp.Value.rightObject.flipped)
            {
                temp.Value.rightObject.gobject.transform.localScale = new Vector3(temp.Value.rightObject.gobject.transform.localScale.x * -1, 1, 1);
            }
            temp.Value.rightObject.gobject.transform.rotation = Quaternion.Euler(0, temp.Value.rightObject.rotation_degree, 0);

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
        //cubes[0].gameObject.SetActive(true);
        //createPair();
        buttonStart.gameObject.SetActive(false);
        training_start = true;
        curr_time = Time.time;

        head = configList.First;
        temp = head;
        //configList.First.Value.leftObject.gobject.gameObject.SetActive(true);
        head.Value.leftObject.gobject.gameObject.SetActive(true);
        //configList.First.Value.leftObject.gobject.transform.rotation = Quaternion.Euler(0, configList.First.Value.leftObject.rotation_degree, 0);
        head.Value.leftObject.gobject.transform.rotation = Quaternion.Euler(0, head.Value.leftObject.rotation_degree, 0);
        //configList.First.Value.rightObject.gobject.gameObject.SetActive(true);
        head.Value.rightObject.gobject.gameObject.SetActive(true);
        //configList.First.Value.rightObject.gobject.transform.position = new Vector3(3, 0, 12);
        head.Value.rightObject.gobject.transform.position = new Vector3(3, 0, 12);
        if (head.Value.rightObject.flipped)
        {
            head.Value.rightObject.gobject.transform.localScale = new Vector3(-1, 1, 1);
        }
        head.Value.rightObject.gobject.transform.rotation = Quaternion.Euler(0, head.Value.rightObject.rotation_degree, 0);

    }

    public LinkedList<ObjectPairConfig> getPairList()
    {

        LinkedList<ObjectPairConfig> allTrialObjectPairConfigList = new LinkedList<ObjectPairConfig>();

        for (int trial = 0; trial < 3; trial++)
        {

            LinkedList<ObjectPairConfig> trialObjectPairConfigList = new LinkedList<ObjectPairConfig>();

            for (int obj = 0; obj < 5; obj++)
            {

                List<int> firstRotationList = new List<int>() { 10, 50, 90, 130, 170 };
                //List<int> secondRotationList = new List<int>() { 10, 50, 90, 130, 170 };

                for (int firstRotationIndex = 0; firstRotationIndex < 4; firstRotationIndex++)
                {
                    ObjectConfig leftObject = new ObjectConfig();   // there are two objects on the canvas screen leftObject and rightObject
                    leftObject.gobject = cubes[obj];
                    leftObject.rotation_degree = firstRotationList[firstRotationIndex];
                    leftObject.flipped = false;

                    for (int secondRotationIndex = firstRotationIndex + 1; secondRotationIndex < 5; secondRotationIndex++)
                    {
                        ObjectConfig rightObject = new ObjectConfig();
                        rightObject.gobject = cubes_right[obj];
                        rightObject.rotation_degree = firstRotationList[secondRotationIndex];
                        rightObject.flipped = false;

                        ObjectConfig rightObjectFlipped = new ObjectConfig();
                        rightObjectFlipped.gobject = cubes_right[obj];
                        rightObjectFlipped.rotation_degree = firstRotationList[secondRotationIndex];
                        rightObjectFlipped.flipped = true;

                        ObjectPairConfig objectPairConfig = new ObjectPairConfig();
                        objectPairConfig.leftObject = leftObject;
                        objectPairConfig.rightObject = rightObject;

                        ObjectPairConfig objectPairFlippedConfig = new ObjectPairConfig();
                        objectPairFlippedConfig.leftObject = leftObject;
                        objectPairFlippedConfig.rightObject = rightObjectFlipped;

                        trialObjectPairConfigList.AddLast(objectPairConfig);
                        //Debug.Log(objectPairConfig.leftObject.gobject);
                        trialObjectPairConfigList.AddLast(objectPairFlippedConfig);
                        //Debug.Log(objectPairFlippedConfig.leftObject.gobject);
                    }
                }
            }
            LinkedList<ObjectPairConfig> perumutedObjectPairConfigList = shuffle(trialObjectPairConfigList);

            foreach (var item in perumutedObjectPairConfigList)
            {
                allTrialObjectPairConfigList.AddLast(item);
                //Debug.Log(item.leftObject.gobject);
            }

        }
        return allTrialObjectPairConfigList;
    }

    public LinkedList<ObjectPairConfig> shuffle(LinkedList<ObjectPairConfig> list)
    {
        //Random Rand = new Random();

        List<int> numlist = new List<int>();
        for(int i =0; i<300; i++)
        {
            numlist.Add(i);
        }
        int size = numlist.Count;

        //Shuffle the list


        //list = list.OrderBy<ObjectPairConfig>(x => Random.value).ToList();
        //Shuffle<int>(numlist);
        // shuffle index
        IListExtensions.Shuffle<int>(numlist);
        return list;
    }
    /*
    public void Shuffle(List<int> list)
    {
        //Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    */
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
