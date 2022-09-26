using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gameLife : MonoBehaviour
{
    public enum GameState
    {
        draw, 
        watch, 
        play
    }
    public static bool darkTheme = false;
    public GameObject canvas;
    public GameObject UIiterationHistory;
    public const int width = 100, height = 100;
    public static GameState state = GameState.draw;
    public static cell[,] field = new cell[width,height];
    public GameObject cellPref;
    public float iterationTimeIsSeconds = 0.5f;
    private static bool iterationEnd=true;
    public TMP_Text UIspeed;
    public TMP_Text UIiteration;
    public TMP_Text UIcurIteration;
    static public int iteration = 0, curIteration;
    private List<bool[,]> iterations = new List<bool[,]>();
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
        
        
        bool[,] t = new bool[width,height], iterationState = new bool[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                t[j, i] = updateCell(j, i);
                iterationState[j, i] = t[j, i];
            }
        }

        iterations.Add(iterationState);
        Debug.Log(iterations.Count);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                 field[j,i].changeState(t[j, i]);
            }
        }
        iteration++;
        yield return new WaitForSeconds(iterationTimeIsSeconds);
        iterationEnd = true;
    }

    void UIupdate()
    {
        UIspeed.text = string.Format(" Iteration delay: {0}", iterationTimeIsSeconds);
        UIiteration.text = string.Format(" Last iteration is : {0}", iteration);
        if(state == GameState.watch)UIcurIteration.text = string.Format(" cur it: {0}", curIteration);
    }
    void pause()
    {
        if (state == GameState.draw) { state = GameState.play; if (iteration == 0) {
                bool[,] iterationState = new bool[width, height];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        iterationState[j, i] = field[j, i].isAlife;
                    }
                }
                iterations.Add(iterationState);
            }
        }
        else if (state == GameState.play) state = GameState.draw; 
    }
    void randomField()
    {
        if (iteration == 0) {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[j, i].changeState(false);
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[j, i].changeState(Random.RandomRange(0, 8) == 1 ? true : false);
                }
            }
        }
    }
    void stateViewer()
    {
        if (state != GameState.watch)
        {
            state = GameState.watch;
            curIteration = iteration;
            UIiterationHistory.SetActive(true);
        }
        else
        {
            UIcurIteration.text = ""; 
            UIiterationHistory.SetActive(false);
            state = GameState.draw;
            curIteration = iteration;
            if(iteration > 0)loadIteration();
        }
    }
    void loadIteration()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                field[j, i].changeState(iterations[curIteration][j,i]);
            }
        }
    }
    void stateNext()
    {
        if (curIteration != iteration)
        {
            curIteration++;
            loadIteration();
        }
    }
    void stateBack()
    {
        if(curIteration != 0)
        {
            curIteration--;
            loadIteration();
        }
    }
    void darkThemeSet()
    {
        darkTheme = !darkTheme;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                field[j, i].changeState(field[j, i].isAlife);
            }
        }
    }
    void kayboardInput()
    {
        if (Input.GetKeyDown("p")) pause();
        if (Input.GetKeyDown("u")) { canvas.SetActive(!canvas.activeSelf); Debug.Log(canvas.activeSelf); }
        if (Input.GetKeyDown("t")) iterationTimeIsSeconds /= 2;
        if (Input.GetKeyDown("y")) iterationTimeIsSeconds *= 2;
        if (Input.GetKeyDown("v")) stateViewer();
        if (Input.GetKeyDown("h")) darkThemeSet();
        if (Input.GetKeyDown("z")) randomField();
        if (Input.GetKeyDown("m")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (state == GameState.watch && iteration > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) stateBack();
            if (Input.GetKeyDown(KeyCode.RightArrow)) stateNext();
        }
    }

    void Start()
    {
        createField();
        UIupdate();
    }
    void Update()
    {
        kayboardInput();
        if (state== GameState.play && iterationEnd) { iterationEnd = false; StartCoroutine(updateField()); }
        Debug.Log(darkTheme);
        UIupdate();
    }
}
