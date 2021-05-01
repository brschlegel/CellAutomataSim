using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update    
    GameObject[,] matrix;
    WorldCreation world;
    List<string> levelNames;
    RuleHeirarchy rules;
    int currentLevel;
    public int stepLimit = 50;
    int currentStep;
    bool running;
    [SerializeField]
    public List<int[]> winConds;
    
    void Start()
    {
        world = GetComponent<WorldCreation>();
        rules = GetComponent<RuleHeirarchy>();
        matrix = world.matrix;
        levelNames = new List<string>() {"LineUp"};
        currentLevel = 0;
        currentStep = 0;
        winConds = new List<int[]>();
        world.Load(levelNames[currentLevel]);
        PopulateWinConds();
        rules.Init(matrix);
        running = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Step()
    {
        int[,] newBoard = rules.Step(matrix);
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j].GetComponent<SpriteRenderer>().color != GlobalVars.typeToColor[CellType.Wall])
                    matrix[i,j].GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[(CellType)newBoard[i,j]];
            }
        }
        if(CheckForWin())
        {
            Debug.Log("You Won!");
        }
        currentStep++;
    }

    void PopulateWinConds()
    {
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j].GetComponent<SpriteRenderer>().color == GlobalVars.typeToColor[CellType.Goal])
                {
                    winConds.Add(new int[2] {i,j});

                }
            }
        }
    }
    bool CheckForWin()
    {
        foreach(int[] cond in winConds)
        {
            if(matrix[cond[0], cond[1]].GetComponent<SpriteRenderer>().color == GlobalVars.typeToColor[CellType.Alive])
            {
                return true;
            }
        }
        return false;
    }

    public void StartButton()
    {
        InvokeRepeating("Step", .25f,.25f);
        // if(!running)
        //     InvokeRepeating("Step", .25f,.25f);
        // running = true;
    }

    public void Reset()
    {
        running = false;
        world.Load(levelNames[currentLevel]);
        CancelInvoke("Step");
        rules.Init(matrix);
    }
}
