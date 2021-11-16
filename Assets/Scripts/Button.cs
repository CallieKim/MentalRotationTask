using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //public GameObject buttonYes;
    //public GameObject buttonNo;
    public UnityEngine.UI.Button buttonYes;
    public UnityEngine.UI.Button buttonNo;
    // Start is called before the first frame update
    void Start()
    {
        //bY = buttonYes.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            buttonYes.onClick.Invoke();
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("right");
            buttonNo.onClick.Invoke();
        }
    }
}
