using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class ToCSV : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();

    public void Save()
    {
        // Creating First row of titles manually
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Reaction Time";
        rowDataTemp[1] = "Accuracy";
        rowData.Add(rowDataTemp);

        // get stored list from Button script
        List<Button.TrialData> trialTemp = gameObject.GetComponent<Button>().dataList;

        // store list to temp array
        for (int i = 0; i < trialTemp.Count; i++)
        {
            rowDataTemp = new string[2];
            rowDataTemp[0] = trialTemp[i].reactionTime.ToString(); // name
            rowDataTemp[1] = trialTemp[i].correct.ToString(); // ID
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }
}
