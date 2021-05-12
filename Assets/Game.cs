using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;
public class Game : MonoBehaviour
{
    // Start is called before the first frame update    
    public GameObject[,] matrix;

    public float ruleScoreMult = 5;
    WorldCreation world;
    public List<string> levelNames;
    public RuleHeirarchy rules;
    public int currentLevel;
    public int stepLimit = 50;
    int currentStep;
    bool running;

    public PuzzleAgent agent;
    public bool won;
    [SerializeField]
    public List<int[]> winConds;
    
    void Start()
    {
        world = GetComponent<WorldCreation>();
        rules = GetComponent<RuleHeirarchy>();
        matrix = world.matrix;
        levelNames = new List<string>() {"LineUp", "Funnel",  "Walls",  "Escape", "WallCheat", "Distance", "FourCorners", "InvertLineUp", "Optimization", "Overload", "Room", "Temptation", "WallHackEasy", "WallHackHarder", "Bucket"};
        if(agent != null)
        {
         agent.wins = new List<int>();
        for(int i = 0; i < levelNames.Count; i++)
        {
            agent.wins.Add(0);
        }
        }
        currentLevel = 0;
        currentStep = 0;
        winConds = new List<int[]>();
        world.Load(levelNames[currentLevel]);
        PopulateWinConds();
        rules.Init(matrix);
        running = false;
        won = false;
       

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
        if(agent!= null)
        {
            agent.AddReward(-1);
        }
        if(CheckForWin())
        {
            
            if(agent != null)
            {
                agent.AddReward(CalculateScore());
                Debug.Log("Beat Level " + currentLevel);
                agent.wins[currentLevel] += 1;
                if(agent.parameters.BehaviorType != BehaviorType.InferenceOnly)
                {
                    agent.EndEpisode();
                }
                else
                {
                    CancelInvoke("Step");
                }

               
            }
            won = true;
        }

        if(currentStep == stepLimit && agent != null )
        {
         
            CancelInvoke("Step");
            GameObject[,] saved = world.matrix;
            world.Load(levelNames[currentLevel]);
            //Give points for changing things
            for(int i = 0; i < saved.GetLength(0); i++)
            {
                for(int j = 0; j < saved.GetLength(1); j++)
                {
                    if(saved[i,j].GetComponent<SpriteRenderer>().color != world.matrix[i,j].GetComponent<SpriteRenderer>().color &&saved[i,j].GetComponent<SpriteRenderer>().color == GlobalVars.typeToColor[CellType.Alive] )
                    {
                        agent.AddReward(1);
                    }
                }
            }
            if(agent.parameters.BehaviorType != BehaviorType.InferenceOnly)
            {
            agent.AddReward(-2000 );
            agent.EndEpisode();
            }
        }
        currentStep++;
        
    }

    void PopulateWinConds()
    {
        winConds.Clear();
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
          Reset();
        InvokeRepeating("Step", .25f,.05f);
      
        // if(!running)
        //     InvokeRepeating("Step", .25f,.25f);
        // running = true;
    }

    public void Reset()
    {
        running = false;
        world.Load(levelNames[currentLevel]);
        currentStep = 0;
        CancelInvoke("Step");
        rules.Init(matrix);
        PopulateWinConds();
    }

    public void NextLevel()
    {
        currentLevel++;
        Reset();
    }

    public float CalculateScore()
    {
        float score = 0;
        score -= rules.count * ruleScoreMult;
         for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j].GetComponent<SpriteRenderer>().color == GlobalVars.typeToColor[CellType.Alive])
                {
                   score -= 1.5f;

                }
            }
        }
        return score;
    }
}
