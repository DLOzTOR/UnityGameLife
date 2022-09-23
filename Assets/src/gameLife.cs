using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLife : MonoBehaviour
{
    private const int width = 64, height = 36;
    public static bool gameStart = false;
    public static cell[,] field = new cell[width,height];
    public GameObject cellPref;
    public float iterationTimeIsSeconds = 0.5f;
    private static bool iterationEnd=true;
    void createField()
    {
        for(int i = 0; i< height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                field[j, i] = new cell(j, i, false, cellPref);
            }
        }
    }
    int loop(int i, int max)
    {
        return i < 0 ? max - 1 : i > max - 1 ? 0 : i; 
    }
    bool updateCell(int x, int y)
    {
        int lifeCellsCount = 0;

        if (field[loop(x - 1, width),loop(y + 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x, width),loop(y + 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x + 1, width),loop(y + 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x + 1, width),loop(y, height)].isAlife) lifeCellsCount++;
        if (field[loop(x + 1, width),loop(y - 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x, width),loop(y - 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x - 1, width),loop(y - 1, height)].isAlife) lifeCellsCount++;
        if (field[loop(x - 1, width),loop(y, height)].isAlife) lifeCellsCount++;
        if ((field[x,y].isAlife == false && lifeCellsCount == 3) || (field[x,y].isAlife && (lifeCellsCount == 2 || lifeCellsCount == 3))) 
            return true;
        return false;
    }
    IEnumerator updateField()
    {
        yield return new WaitForSeconds(iterationTimeIsSeconds);
        bool[,] t = new bool[width,height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                t[j, i] = updateCell(j, i);
            }
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                 field[j,i].changeState(t[j, i]);
            }
        }
        iterationEnd = true;
    }
    void Start()
    {
        createField();
    }

    void Update()
    {
        if (Input.GetKeyDown("s"))gameStart = !gameStart;
        if (gameStart == true && iterationEnd) { iterationEnd = false; StartCoroutine(updateField()); }
    }
}