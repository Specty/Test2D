using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotsBehavior : MonoBehaviour
{
    public int spotNumber;
    public bool isObstructed;
    private bool isPreSelected;
    private int chipNumber;
    GameObject[] chips;

    private void OnMouseDown()
    {
        if (isPreSelected == true)
        {
            chips = GameInit.GetChips();
            foreach (GameObject chip in chips)
            {
                if (chip.GetComponent<ChipBehavior>().GetChipNum() == chipNumber)
                {
                    chip.GetComponent<ChipBehavior>().MoveTo(spotNumber);
                    break;
                }
            }
            //move chip to this spot
        }
    }
    public void SetChipNumber(int x)
    {
        chipNumber = x;
    }

    public int GetChipNumber()
    {
        return chipNumber;
    }
    public void SetSpotNumber(int x)
    {
        spotNumber = x;
    }

    public int GetSpotNumber()
    {
        return spotNumber;
    }

    public void SetIsObstructed(bool x)
    {
        isObstructed = x;
    }

    public bool GetIsObstructed()
    {
        return isObstructed;
    }
    public void SetIsPreSelected(bool x)
    {
        isPreSelected = x;
    }

    public bool GetIsPreSelected()
    {
        return isPreSelected;
    }
}
