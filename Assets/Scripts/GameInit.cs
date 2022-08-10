using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameInit : MonoBehaviour
{
    public GameObject chipsPref;
    public GameObject spotsPref;
    public GameObject dotsPref;
    public GameObject solutionChipPref;
    public GameObject restartButton;
    public GameObject winPanel;
    public GameObject solution;
    private static List<GameObject> spots;
    private static List<GameObject> dots;
    private static List<GameObject> chips;
    private static List<GameObject> solChips;
    private static List<string> spotsCoords;
    private static List<string> joints;

    //public GameObject spotsPref;
    Quaternion qua = new Quaternion(0, 0, 0, 0);
    private void Start()
    {
        restartButton.SetActive(false);
        winPanel.SetActive(false);
        spots = new List<GameObject>();
        dots = new List<GameObject>();
        chips = new List<GameObject>();
        solChips = new List<GameObject>();

        spotsCoords = new List<string>();
        joints = new List<string>();
    }

    public void ReadData()
    {
        string path = Application.dataPath + "/test.txt";
        string[] lines = File.ReadAllLines(path);

        //number of chips
        int chipsNum = Convert.ToInt32(lines[0]);
        gameObject.GetComponent<Gameplay>().SetChipsNum(chipsNum);
        //number of spots chips can be in
        int spotsNum = Convert.ToInt32(lines[1]);

        //check if everything is ok
        if (chipsNum >= spotsNum)
        {
            Debug.Log("Unplayable (number of chips must be greater than number of spots!");
            return;
        }

        //coords here
        for (int i = 0; i < spotsNum; i++)
        {
            spotsCoords.Add(lines[i + 2]);
        }

        //chips init position
        string chipsPos = lines[spotsNum + 2];
        //chips position to win the game
        string chipsPos2win = lines[spotsNum + 3];

        //number of joints
        int jointsNum = Convert.ToInt32(lines[spotsNum + 4]);
        if (spotsNum < jointsNum)
        {
            Debug.Log("Unplayable");
            return;
        }

        //joints here

        for (int i = 0; i < jointsNum; i++)
        {
            joints.Add(lines[spotsNum + 5 + i]);
        }

        //spawners
        string[] spotsArr = spotsCoords.ToArray();
        SpawnSpots(spotsArr);


        string[] jointsArr = joints.ToArray();
        SpawnJoints(jointsArr);


        SpawnChips(chipsPos, chipsPos2win);
    }

    private void SpawnSpots(string[] arr)
    {
        int x, y;
        for (int i = 1; i <= arr.Length; i++)
        {
            x = Convert.ToInt32(arr[i - 1].Substring(0, arr[i - 1].IndexOf(","))) / 100;
            y = Convert.ToInt32(arr[i - 1].Substring(arr[i - 1].IndexOf(",") + 1)) / 100;
            Vector3 vec = new Vector3(x, y, 1);
            GameObject spot = Instantiate(spotsPref, vec, qua);
            spot.GetComponent<SpotsBehavior>().SetSpotNumber(i);
            spots.Add(spot);
        }
    }

    private void SpawnJoints(string[] arr)
    {
        int x, y;
        GameObject[] objArr = spots.ToArray();
        for (int j = 0; j < arr.Length; j++)
        {
            x = Convert.ToInt32(arr[j].Substring(0, arr[j].IndexOf(",")));
            y = Convert.ToInt32(arr[j].Substring(arr[j].IndexOf(",") + 1));
            Vector3 instPos;
            float lerpVal = 0.1f;
            Vector3 vec3init = new Vector3(objArr[x - 1].transform.position.x, objArr[x - 1].transform.position.y, 2);
            Vector3 vec3end = new Vector3(objArr[y - 1].transform.position.x, objArr[y - 1].transform.position.y, 2);
            for (int i = 0; i < 10; i++)
            {
                instPos = Vector3.Lerp(vec3init, vec3end, lerpVal);
                GameObject dot = Instantiate(dotsPref, instPos, transform.rotation);
                lerpVal += 0.1f;
                dots.Add(dot);
            }
        }
        //Need to figure out how to properly spawn them
    }

    private void SpawnChips(string chipsPos, string chipsPos2win)
    {
        string[] x = chipsPos.Split(',');
        string[] y = chipsPos2win.Split(',');
        GameObject[] spotArr = spots.ToArray();
        int z;

        for (int i = 0; i < spotArr.Length; i++)
        {
            for (int j = 0; j < x.Length; j++)
            {
                if (i + 1 == Convert.ToInt32(x[j]))
                {
                    //playable chips
                    Vector3 vec3 = new Vector3(spotArr[i].transform.position.x, spotArr[i].transform.position.y, 0);
                    GameObject chip = Instantiate(chipsPref, vec3, qua);
                    chip.GetComponent<SpriteRenderer>().material.color = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), 1f);

                    SpotsBehavior spotBeh = spots[i].GetComponent<SpotsBehavior>();//.SetIsObstructed(true);
                    ChipBehavior chipBeh = chip.GetComponent<ChipBehavior>();
                    spotBeh.SetIsObstructed(true);
                    z = spotBeh.GetSpotNumber();
                    chipBeh.SetInitialSpotNum(z);
                    chipBeh.SetCurrentSpotNum(z);
                    chipBeh.SetEndSpotNum(Convert.ToInt32(y[j]));
                    chipBeh.SetChipNum(j);
                    chips.Add(chip);
                    break;
                }

            }
        }
        for (int i = 0; i < spotArr.Length; i++)
        {
            for (int j = 0; j < y.Length; j++)
            {
                if (i + 1 == Convert.ToInt32(y[j]))
                {
                    Vector3 vec3 = new Vector3(spotArr[i].transform.position.x, spotArr[i].transform.position.y, 0);
                    GameObject solChip = Instantiate(solutionChipPref, solution.transform);
                    solChip.transform.localPosition = vec3;
                    solChip.transform.localRotation = qua;
                    solChip.GetComponent<SpriteRenderer>().material.color = chips[j].GetComponent<SpriteRenderer>().material.color;
                    solChips.Add(solChip);
                }
            }
        }
    }

    public static int[] GetJoints(int x)
    {
        string[] arrJoints = joints.ToArray();
        List<int> lst = new List<int>();
        for (int i = 1; i <= arrJoints.Length; i++)
        {
            if (arrJoints[i - 1].Contains(Convert.ToString(x)))
            {
                int a, b;
                a = Convert.ToInt32(arrJoints[i - 1].Substring(0, arrJoints[i - 1].IndexOf(",")));
                b = Convert.ToInt32(arrJoints[i - 1].Substring(arrJoints[i - 1].IndexOf(",") + 1));
                lst.Add(a != x ? a : b);
            }

        }
        int[] arr = lst.ToArray();
        return arr;
    }

    public static GameObject[] GetSpots()
    {
        GameObject[] arr = spots.ToArray();
        return arr;
    }

    public static GameObject[] GetChips()
    {
        GameObject[] arr = chips.ToArray();
        return arr;
    }

    public void Delete()
    {
        Destr(ref chips);
        Destr(ref dots);
        Destr(ref spots);
        Destr(ref solChips);
        spotsCoords.RemoveRange(0, spotsCoords.Count);
        joints.RemoveRange(0, joints.Count);
        gameObject.GetComponent<Gameplay>().ClearChips();
    }

    private void Destr(ref List<GameObject> list)
    {
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(list[i]);
        }
        list.RemoveRange(0, count);
    }
}