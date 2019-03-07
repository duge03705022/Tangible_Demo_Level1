using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public GameObject parentTransform;

    public GameObject[] cards;

    public bool[] canPlaceCard;
    public int[] stackSensing;

    public bool[] waitDestroy;

    private GameObject[] cardInstance;
    public int[] lastStack;
    private bool[] hasPlaced;

    // Start is called before the first frame update
    void Start()
    {
        cards = new GameObject[GameParameter.maxCardNum];

        canPlaceCard = new bool[GameParameter.blockNum];
        stackSensing = new int[GameParameter.blockNum];

        waitDestroy = new bool[GameParameter.blockNum];

        cardInstance = new GameObject[GameParameter.blockNum];
        lastStack = new int[GameParameter.blockNum];
        hasPlaced = new bool[GameParameter.blockNum];

        for (int i = 0; i < GameParameter.blockNum; i++)
        {
            canPlaceCard[i] = false;
            stackSensing[i] = -1;

            waitDestroy[i] = false;

            hasPlaced[i] = false;
        }

        //hasPlaced[8] = false;
        //hasPlaced[42] = false;
        //hasPlaced[43] = false;
        //hasPlaced[44] = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateCards();
    }

    void updateCards()
    {
        for (int i = 0; i < GameParameter.blockNum; i++)
        {
            if (stackSensing[i] != -1 && canPlaceCard[i] && !hasPlaced[i])
            {
                placeCard(i);
            }
            else if ((waitDestroy[i] || stackSensing[i] == -1) && hasPlaced[i])
            {
                destroyCard(i);
            }
        }
    }

    void placeCard(int num)
    {
        cardInstance[num] = Instantiate(cards[stackSensing[num]], parentTransform.transform);
        cardInstance[num].transform.localPosition = new Vector3(
            num % GameParameter.stageCol * GameParameter.stageGap,
            num / GameParameter.stageCol * GameParameter.stageGap,
            0);
        hasPlaced[num] = true;
        lastStack[num] = stackSensing[num];
    }

    void destroyCard(int num)
    {
        Destroy(cardInstance[num]);
        cardInstance[num] = null;
        hasPlaced[num] = false;
        if (stackSensing[num] == lastStack[num])
        {
            stackSensing[num] = -1;
        }
        waitDestroy[num] = false;
    }

    public void setCanPlaceCard(int[] series, bool TorF)
    {
        for (int i = 0; i < series.Length; i++)
        {
            canPlaceCard[series[i]] = TorF;
        }
    }

    public void setCards(GameObject[] series)
    {
        for (int i = 0; i < series.Length; i++)
        {
            cards[i] = series[i];
        }
    }
}
