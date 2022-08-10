using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public GameObject win;
    private int chipsCount;
    private int chipsAtTheEnd=0;

    public void IncreaseChipsCountAtTheEnd()
    {
        chipsAtTheEnd++;
        if (chipsAtTheEnd == chipsCount)
        {
            win.SetActive(true);
            ClearChips();
            gameObject.GetComponent<GameInit>().Delete();
        }
    }

    public void DecreaseChipsCountAtTheEnd()
    {
        chipsAtTheEnd--;
    }

    public void SetChipsNum(int x)
    {
        chipsCount = x;
    }

    public void ClearChips()
    {
        chipsAtTheEnd = 0;
    }
}
