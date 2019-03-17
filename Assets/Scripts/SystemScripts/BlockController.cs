using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class BlockController : MonoBehaviour
{
    public TouchHandler touchHandler;
    public GameObject parentCanvas;
    public GameObject blockPrefeb;
    public int blockSeriesRow;
    public int blockSeriesCol;
    public float blockSize;
    private GameObject[,] blockSeries = new GameObject[27, 15];
    public int touchedColBlock;
    public int touchedRowBlock;

    public float threshold;
    public string touchStr = "";
    public string touchStr2 = "";
    public string tmpTest = "";
    private string lastTouchStr = "";
    private string lastTouchStr2 = "";

    private float[] rowData;
    private float[] colData;
    private Vector2 tmpRowPos;
    private Vector2 tmpColPos;


    // Start is called before the first frame update
    void Start()
    {
        rowData = new float[blockSeriesRow];
        colData = new float[blockSeriesCol];

        blockSeriesRow++;
        blockSeriesCol++;
        blockSeries = new GameObject[blockSeriesRow + 1, blockSeriesCol + 1];

        for (int i = 0; i < blockSeriesRow + 1; i++)
        {
            for (int j = 0; j < blockSeriesCol + 1; j++)
            {
                blockSeries[i, j] = Instantiate(blockPrefeb, parentCanvas.transform);
                blockSeries[i, j].transform.localPosition = new Vector3((i - (blockSeriesRow - 1) / 2) * 35, (j - (blockSeriesCol - 1) / 2) * 35, 0);
                blockSeries[i, j].transform.Find("Text").GetComponent<Text>().text = "0";
                blockSeries[i, j].SetActive(false);
            }
        }

        //blockSeries[i].GetComponent<Image>().color = new Color(1, 1, 1);
        //blockSeries[i].transform.Find("Text").GetComponent<Text>().text = "0";

        blockSeriesRow--;
        blockSeriesCol--;
        for (int i = 0; i < blockSeriesCol; i++)
        {
            rowData[i] = 0;
            blockSeries[blockSeriesRow, i].GetComponent<Image>().color = new Color(0.5f, 1, 0.5f);
            blockSeries[blockSeriesRow, i].transform.Find("Text").GetComponent<Text>().text = rowData[i].ToString();
            Vector2 tmpPos = blockSeries[blockSeriesRow, i].GetComponent<RectTransform>().anchoredPosition;
            blockSeries[blockSeriesRow, i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x + 20, tmpPos.y);
            blockSeries[blockSeriesRow + 1, i].GetComponent<Image>().color = new Color(0.5f, 1, 0.5f);
            blockSeries[blockSeriesRow + 1, i].transform.Find("Text").GetComponent<Text>().text = "";
            tmpPos = blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition;
            tmpRowPos = tmpPos;
            tmpRowPos.x += 20;
            blockSeries[blockSeriesRow + 1, i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x + 20, tmpPos.y);

            colData[i] = 0;
            blockSeries[i, blockSeriesCol].GetComponent<Image>().color = new Color(0.5f, 0.5f, 1);
            blockSeries[i, blockSeriesCol].transform.Find("Text").GetComponent<Text>().text = colData[i].ToString();
            tmpPos = blockSeries[i, blockSeriesCol].GetComponent<RectTransform>().anchoredPosition;
            blockSeries[i, blockSeriesCol].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpPos.y + 20);
            blockSeries[i, blockSeriesCol + 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 1);
            blockSeries[i, blockSeriesCol + 1].transform.Find("Text").GetComponent<Text>().text = "";
            tmpPos = blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition;
            tmpColPos = tmpPos;
            tmpColPos.y += 20;
            blockSeries[i, blockSeriesCol + 1].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpPos.y + 20);
        }
        blockSeries[blockSeriesRow, blockSeriesCol].GetComponent<Image>().color = Color.black;
        blockSeries[blockSeriesRow, blockSeriesCol].transform.Find("Text").GetComponent<Text>().text = "";
        blockSeries[blockSeriesRow + 1, blockSeriesCol].GetComponent<Image>().color = Color.black;
        blockSeries[blockSeriesRow + 1, blockSeriesCol].transform.Find("Text").GetComponent<Text>().text = "";
        blockSeries[blockSeriesRow, blockSeriesCol + 1].GetComponent<Image>().color = Color.black;
        blockSeries[blockSeriesRow, blockSeriesCol + 1].transform.Find("Text").GetComponent<Text>().text = "";
        blockSeries[blockSeriesRow + 1, blockSeriesCol + 1].GetComponent<Image>().color = Color.black;
        blockSeries[blockSeriesRow + 1, blockSeriesCol + 1].transform.Find("Text").GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        int touchedRow = 0, touchedCol = 0;
        float touchedRowData = 0, touchedColData = 0;

        string[] newData = touchStr.Split(',');
        if (touchStr != lastTouchStr)
        {
            newData = touchStr.Split(',');
            if (newData[0] == "R")
            {
                try
                {
                    for (int j = 0; j < blockSeriesRow; j++)
                    {
                        int i = blockSeriesRow -1 - j;
                        //if (float.Parse(newData[i + 1]) != 0)
                            rowData[i] = float.Parse(newData[j + 1]);
                        blockSeries[blockSeriesRow, i].GetComponent<Image>().color = new Color(1 - rowData[i] / 255, 1, 1 - rowData[i] / 255);
                        blockSeries[blockSeriesRow, i].transform.Find("Text").GetComponent<Text>().text = rowData[i].ToString();
                        blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().sizeDelta = new Vector2(rowData[i] / 10000.0f * blockSize, blockSize);
                        Vector2 tmpPos = blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition;
                        blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpRowPos.x - (blockSize - (rowData[i] / 10000.0f * blockSize)) / 2, tmpPos.y);
                    }
                }
                catch (Exception)
                {
                    Debug.Log("Invalid Row data -- " + newData);

                }
            }
            else if (newData[0] == "C")
            {
                try
                {
                    for (int i = 0; i < blockSeriesCol; i++)
                    {
                        //if (int.Parse(newData[i + 1]) != 0)
                            colData[i] = float.Parse(newData[i + 1]);
                        blockSeries[i, blockSeriesCol].GetComponent<Image>().color = new Color(1 - colData[i] / 255, 1 - colData[i] / 255, 1);
                        blockSeries[i, blockSeriesCol].transform.Find("Text").GetComponent<Text>().text = "<color=white>" + colData[i].ToString() + "</color>";
                        blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().sizeDelta = new Vector2(blockSize, colData[i] / 10000.0f * blockSize);
                        Vector2 tmpPos = blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition;
                        blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpColPos.y - (blockSize - (colData[i] / 10000.0f * blockSize)) / 2);
                    }
                }
                catch (Exception)
                {
                    Debug.Log("Invalid Col data -- " + newData);

                }
            }
            else if (newData[0] == "P")
            {
                touchedCol = int.Parse(newData[1]);
                touchedRow = int.Parse(newData[2]);
                touchedRowData = float.Parse(newData[3]);
                touchedColData = touchedRowData;
            }
            else
            {
                Debug.Log("Invalid Touch String" + newData);
            }
        }

        //TODO Test code
        newData = tmpTest.Split(',');
        if (newData[0] == "P")
        {
            touchedCol = int.Parse(newData[1]);
            touchedRow = int.Parse(newData[2]);
            touchedRowData = float.Parse(newData[3]);
            touchedColData = touchedRowData;
        }

        if (touchStr2 != lastTouchStr2)
        {
            newData = touchStr2.Split(',');
            if (newData[0] == "R")
            {
                try
                {
                    for (int j = 0; j < blockSeriesRow; j++)
                    {
                        int i = blockSeriesRow - 1 - j;
                        //if (int.Parse(newData[i + 1]) != 0)
                        rowData[i] = float.Parse(newData[j + 1]);
                        blockSeries[blockSeriesRow, i].GetComponent<Image>().color = new Color(1 - rowData[i] / 255, 1, 1 - rowData[i] / 255);
                        blockSeries[blockSeriesRow, i].transform.Find("Text").GetComponent<Text>().text = rowData[i].ToString();
                        blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().sizeDelta = new Vector2(rowData[i] / 10000.0f * blockSize, blockSize);
                        Vector2 tmpPos = blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition;
                        blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpRowPos.x - (blockSize - (rowData[i] / 10000.0f * blockSize)) / 2, tmpPos.y);
                    }
                }
                catch (Exception)
                {
                    Debug.Log("Invalid Row data -- " + newData);

                }
            }
            else if (newData[0] == "C")
            {
                try
                {
                    for (int i = 0; i < blockSeriesCol; i++)
                    {
                        //if (int.Parse(newData[i + 1]) != 0)
                            colData[i] = float.Parse(newData[i + 1]);
                        blockSeries[i, blockSeriesCol].GetComponent<Image>().color = new Color(1 - colData[i] / 255, 1 - colData[i] / 255, 1);
                        blockSeries[i, blockSeriesCol].transform.Find("Text").GetComponent<Text>().text = "<color=white>" + colData[i].ToString() + "</color>";
                        blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().sizeDelta = new Vector2(blockSize, colData[i] / 10000.0f * blockSize);
                        Vector2 tmpPos = blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition;
                        blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpColPos.y - (blockSize - (colData[i] / 10000.0f * blockSize)) / 2);
                    }
                }
                catch (Exception)
                {
                    Debug.Log("Invalid Col data -- " + newData);

                }
            }
            else if (newData[0] == "P")
            {
                touchedCol = int.Parse(newData[1]);
                touchedRow = int.Parse(newData[2]);
                touchedRowData = float.Parse(newData[3]);
                touchedColData = touchedRowData;
                touchedRowBlock = touchedRow;
            }
            else
            {
                Debug.Log("Invalid Touch String" + newData);
            }
        }

        
        for (int i = 0; i < blockSeriesRow; i++)
        {
            if(rowData[i]>touchedRowData){
                touchedRow = i;
                touchedRowData = rowData[i];
            }
        }
        for (int i = 0; i < blockSeriesCol; i++)
        {
            if (colData[i] > touchedColData)
            {
                touchedCol = i;
                touchedColData = colData[i];
            }
        }
        

        for (int i = 0; i < blockSeriesRow; i++)
        {
            for (int j = 0; j < blockSeriesCol; j++)
            {
                if (i == touchedCol && j == touchedRow && touchedRowData > threshold && touchedColData > threshold) continue;
                blockSeries[i, j].GetComponent<Image>().color = Color.white;
                blockSeries[i, j].transform.Find("Text").GetComponent<Text>().text = "0";
            }
        }

        if ((touchedRowData > threshold && touchedColData > threshold) && touchedCol!=-1)
        {
            blockSeries[touchedCol, touchedRow].GetComponent<Image>().color = new Color(1, 1 - (touchedRowData + touchedColData) / 2550, 1 - (touchedRowData + touchedColData) / 2550);
            blockSeries[touchedCol, touchedRow].transform.Find("Text").GetComponent<Text>().text = ((touchedRowData + touchedColData) / 10).ToString();
            touchedColBlock = touchedCol;
            touchedRowBlock = touchedRow;
            //touchedColBlock = 16;
            //touchedRowBlock = 7;

            //touchHandler.touchSensing[((touchedRowBlock / 3) * (-1) + 4) * 9 + (touchedColBlock / 3)] = true;
        }
        else
        {
            touchedColBlock = -1;
            touchedRowBlock = -1;

            for (int i = 0; i < 45; i++)
            {
                //touchHandler.touchSensing[i] = false;
            }
        }

        lastTouchStr = touchStr;
    }
}
