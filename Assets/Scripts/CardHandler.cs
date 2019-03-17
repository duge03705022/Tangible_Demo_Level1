using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    # region Card Parameter
    public GameController gameController;
    public bool setCardTrans;

    public GameObject parentTransform;

    public GameObject[] cards;

    public bool[] canPlaceCard;
    public int[,,] stackSensing;

    public bool[] waitDestroy;

    private GameObject[] cardInstance;
    public int[] lastStack;
    private bool[] hasPlaced;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        canPlaceCard = new bool[GameParameter.blockNum];
        stackSensing = new int[GameParameter.stageCol, GameParameter.stageRow, GameParameter.maxHight];

        waitDestroy = new bool[GameParameter.blockNum];

        cardInstance = new GameObject[GameParameter.blockNum];
        lastStack = new int[GameParameter.blockNum];
        hasPlaced = new bool[GameParameter.blockNum];

        setCardTrans = true;

        for (int i = 0; i < GameParameter.blockNum; i++)
        {
            canPlaceCard[i] = false;
            waitDestroy[i] = false;
            hasPlaced[i] = false;

            for (int k = 0; k < GameParameter.maxHight; k++)
            {
                stackSensing[i % GameParameter.stageCol, i / GameParameter.stageCol, k] = -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCards();
    }

    private void updateCards()
    {
        for (int i = 0; i < GameParameter.blockNum; i++)
        {
            if (stackSensing[i % GameParameter.stageCol, i / GameParameter.stageCol, 0] != -1 && canPlaceCard[i] && !hasPlaced[i])
            {
                placeCard(i);
            }
            else if ((waitDestroy[i] || stackSensing[i % GameParameter.stageCol, i / GameParameter.stageCol, 0] == -1) && hasPlaced[i])
            {
                destroyCard(i);
            }
            
            if (setCardTrans && gameController.playing && hasPlaced[i])
            {
                cardInstance[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (setCardTrans && !gameController.playing && hasPlaced[i])
            {
                cardInstance[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }

        if (setCardTrans)
        {
            setCardTrans = false;
        }
    }

    private void placeCard(int num)
    {
        cardInstance[num] = Instantiate(cards[stackSensing[num % GameParameter.stageCol, num / GameParameter.stageCol, 0]], parentTransform.transform);
        cardInstance[num].transform.localPosition = new Vector3(
            num % GameParameter.stageCol * GameParameter.stageGap,
            num / GameParameter.stageCol * GameParameter.stageGap,
            0);
        hasPlaced[num] = true;
        lastStack[num] = stackSensing[num % GameParameter.stageCol, num / GameParameter.stageCol, 0];
    }

    private void destroyCard(int num)
    {
        Destroy(cardInstance[num]);
        cardInstance[num] = null;
        hasPlaced[num] = false;
        if (stackSensing[num % GameParameter.stageCol, num / GameParameter.stageCol, 0] == lastStack[num])
        {
            stackSensing[num % GameParameter.stageCol, num / GameParameter.stageCol, 0] = -1;
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
}
