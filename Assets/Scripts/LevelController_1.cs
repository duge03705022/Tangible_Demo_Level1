using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController_1 : MonoBehaviour
{
    # region Level Parameter
    public RFIBManager rFIBManager;
    public GameController gameController;
    public CardHandler cardHandler;
    public LevelParameter levelParameter;

    public int startX;
    public int startY;

    public GameObject[] chef;
    public GameObject[] ingredientStep;

    public GameObject dish;
    public GameObject basket;

    //public GameObject[] levelCards;
    public GameObject[] hints;
    //public int[] availablePlace;
    public int[] answer;

    private bool gameFail;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        //cardHandler.SetCanPlaceCard(availablePlace, true);

        ResetDish();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCooking()
    {
        gameFail = false;
        SetHints(false);
        Debug.Log("Game Start!");
        StartCoroutine(CookingProcess());
    }

    IEnumerator CookingProcess()
    {
        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 3));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(CheckProcess(0));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 2));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(CheckProcess(1));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 3));
        }

        if (!gameFail)
        {
            FinishCooking();
        }
    }

    IEnumerator DishMove(string direction, int step)
    {
        for (int i = 0; i < step * 75; i++)
        {
            yield return new WaitForSeconds(0.01f);
            switch (direction)
            {
                case "Up":
                    dish.transform.localPosition += new Vector3(0f, 0.1f, 0f);
                    break;
                case "Left":
                    dish.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
                    break;
                case "Down":
                    dish.transform.localPosition += new Vector3(0f, -0.1f, 0f);
                    break;
                case "Right":
                    dish.transform.localPosition += new Vector3(0.1f, 0f, 0f);
                    break;
                default:
                    Debug.Log("Move direction error");
                    break;
            }
        }
    }

    IEnumerator CheckProcess(int chefNum)
    {
        string[] chefPosXY = levelParameter.canPlaceCardPos[chefNum].Split(',');
        if (RFIBParameter.SearchCard(rFIBManager.blockId[int.Parse(chefPosXY[0]), int.Parse(chefPosXY[1]), 0]) == answer[chefNum])
        {
            ingredientStep[chefNum].SetActive(false);
            chef[chefNum].SendMessage("StartAct");
            yield return new WaitForSeconds(2f);
            ingredientStep[chefNum + 1].SetActive(true);
        }
        else
        {
            yield return StartCoroutine(FailCooking());
        }
    }

    private void FinishCooking()
    {
        basket.SetActive(false);
        Debug.Log("Game Finish!!!");

        gameController.playing = false;
        //cardHandler.setCardTrans = true;
        SetHints(true);
    }

    IEnumerator FailCooking()
    {
        Debug.Log("Game Over...");
        yield return new WaitForSeconds(1f);

        gameFail = true;
        gameController.playing = false;
        //cardHandler.setCardTrans = true;
        SetHints(true);
        ResetDish();
    }

    private void ResetDish()
    {
        ingredientStep[0].SetActive(true);
        ingredientStep[1].SetActive(false);
        ingredientStep[2].SetActive(false);
        dish.transform.localPosition = new Vector3(
            startX * GameParameter.stageGap,
            startY * GameParameter.stageGap,
            0);
    }

    private void SetHints(bool onOrOff)
    {
        for (int i = 0; i < hints.Length; i++)
        {
            hints[i].SetActive(onOrOff);
        }
    }
}
