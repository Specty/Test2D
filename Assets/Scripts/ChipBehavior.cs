using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChipBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float speed = 0.5f;
    public int chipNum;
    public int initialSpotNum;
    public int currentSpotNum;
    public int endSpotNum;
    private bool isMoving;
    public bool atTheEnd;
    private int[] arr;

    Transform chld;
    Vector3 target;
    GameObject handler;
    GameObject[] chips;
    GameObject[] spots;
    void Start()
    {
        handler = GameObject.FindWithTag("GameController");
        chld = gameObject.transform.GetChild(0);
        chld.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 0.5f, 0f);
        spots = GameInit.GetSpots();
    }

    private void OnMouseDown()
    {
        DeselectChipsAndSpots();
        arr = GameInit.GetJoints(currentSpotNum);
        chld.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 0.5f, 1f);
        SelectSpots();
    }

    private void SelectSpots()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            spots[arr[i] - 1].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 0.5f, 1f);
            spots[arr[i] - 1].GetComponent<SpotsBehavior>().SetIsPreSelected(true);
            spots[arr[i] - 1].GetComponent<SpotsBehavior>().SetChipNumber(chipNum);
        }
    }
    private void DeselectChipsAndSpots()
    {
        chips = GameInit.GetChips();
        foreach (GameObject obj in spots)
        {
            obj.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            obj.GetComponent<SpotsBehavior>().SetIsPreSelected(false);
        }
        foreach (GameObject obj in chips)
        {
            if (obj.GetComponent<ChipBehavior>().atTheEnd == false)
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 0.5f, 0f);
        }
    }

    public void MoveTo(int x)
    {
        if (atTheEnd == true)
        {
            handler.GetComponent<Gameplay>().DecreaseChipsCountAtTheEnd();
            atTheEnd = false;
        }
        currentSpotNum = x;
        target = new Vector3(spots[x - 1].transform.position.x, spots[x - 1].transform.position.y, 0);
        DeselectChipsAndSpots();
        isMoving = true;
    }

    private void Update()
    {

        if (isMoving == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (transform.position == target)
            {
                isMoving = false;
                if (currentSpotNum == endSpotNum)
                {
                    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(0.2f, 1f, 0.2f, 1f);

                    atTheEnd = true;
                    handler.GetComponent<Gameplay>().IncreaseChipsCountAtTheEnd();
                }
            }
        }

    }

    public void SetInitialSpotNum(int x)
    {
        initialSpotNum = x;
    }

    public int GetInitialSpotNum()
    {
        return initialSpotNum;
    }
    public void SetEndSpotNum(int x)
    {
        endSpotNum = x;
    }

    public int GetEndSpotNum()
    {
        return endSpotNum;
    }

    public void SetCurrentSpotNum(int x)
    {
        currentSpotNum = x;
    }

    public int GetCurrentSpotNum()
    {
        return currentSpotNum;
    }

    public void SetChipNum(int x)
    {
        chipNum = x;
    }

    public int GetChipNum()
    {
        return chipNum;
    }
}
