using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;


public class PuzzleAgent : Agent
{
 
    public BehaviorParameters parameters;
    const int TOTALALLOWEDRULES = 8;
    public Game game;
    public RuleEditor list;
    public List<int> wins;

    // Start is called before the first frame update
    void Awake()
    {
        parameters = GetComponent<BehaviorParameters>();
        parameters.BrainParameters.VectorActionSize = new int[TOTALALLOWEDRULES * 12];
        for(int i = 0; i < TOTALALLOWEDRULES; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                parameters.BrainParameters.VectorActionSize[i * 12 + j] = 5;
            }
            parameters.BrainParameters.VectorActionSize[i * 12 + 9] = 2;
            parameters.BrainParameters.VectorActionSize[i * 12 + 10] = 2;
            parameters.BrainParameters.VectorActionSize[i * 12 + 11] = 2;
        }
        
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 0 1 2
    // 3 4 5
    // 6 7 8 
    //Effect = 9
    //Horizontal Symmetry = 10
    //Vertical Symmetry = 11
    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log("Action Requested");
        list.Clear();
        game.rules.Clear();
        for(int i = 0; i < TOTALALLOWEDRULES; i++)
        {
            int[] conditions = new int[9];
            for(int j = 0; j < 9; j++)
            {
                conditions[j] = actions.DiscreteActions[(12*i) + j] - 1; 
            }
            int effect = actions.DiscreteActions[12*i + 9];
            int hor = actions.DiscreteActions[12 * i + 10];
            int vert = actions.DiscreteActions[12 * i + 11];
            //Create a rule with these things
            Rule r = game.rules.CreateRule(conditions, effect, "AIRULE",hor != 0, vert != 0 ) ;
            if(r!= null)
            {
                list.AddButton(r);
                for(int k = 0; k < 9;k++)
                {
                    if(conditions[k] == -1)
                    {
                        AddReward(5);
                    }
                }
            }
            
        }
        if(parameters.BehaviorType != BehaviorType.InferenceOnly)
        {
            game.StartButton();
        }
    }

    public override void OnEpisodeBegin()
    {
        if(parameters.BehaviorType != BehaviorType.InferenceOnly)
        {
        game.currentLevel = GetWeightedRandomIndex(wins);
        }
        else
        {
            game.currentLevel = 0;
        }
        list.Clear();
        Debug.Log("Episode Begun");
        game.Reset();
        game.rules.Clear();
        RequestDecision();
    }

    const int NUMTYPES = 4;
    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting Observation");
        float sum = 0;
        for (int i = 0; i < game.matrix.GetLength(0); i++)
        {
            for(int j = 0; j < game.matrix.GetLength(1); j++)
            {
                sensor.AddOneHotObservation((int)GlobalVars.colorToType[game.matrix[i,j].GetComponent<SpriteRenderer>().color], NUMTYPES);
                if(GlobalVars.colorToType[game.matrix[i,j].GetComponent<SpriteRenderer>().color] == CellType.Goal)
                {
                    sum++;
                }
            }
        }
        Debug.Log("Sum is" + sum);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

       //for(int i = 0; i < TOTALALLOWEDRULES; i++)
       //{
       //    for(int j = 0; j < 9; j++)
       //    {
       //        actions[i * 12 + j] = Random.Range(0,5);
       //    }
       //    actions[i * 12 + 9] = Random.Range(0,2);
       //    actions[i * 12 + 10] = Random.Range(0,2);
       //    actions[i * 12 + 11] = Random.Range(0,2);
       //}
       
        Debug.Log("HEURSTIC");
    }

    public void StartPlaying()
    {
        game.Reset();
        game.rules.Clear();
                
        RequestDecision();
    }

    public int GetWeightedRandomIndex(List<int> arr)
    {
        List<float> normalized = new List<float>() ;
        float sum = 0;
        for(int i = 0; i < arr.Count; i++)
        {
            if(arr[i] != 0)
            {
            normalized.Add(1/arr[i]);

            }
            else
            {
                normalized.Add(100);
            }
            sum += normalized[i];
        }

        if(sum < 100)
        {
            return Random.Range(0,arr.Count);
        }
        for(int i =0; i < normalized.Count; i++)
        {
            normalized[i] /= sum;
        }


        float rand = Random.Range(0.0f,1.0f);
        for(int i = 0; i < normalized.Count; i++)
        {
            if(rand < normalized[i])
            {
                return i;
            }
            rand -= normalized[i];
        }
        return arr.Count -1;
    }

    public void NextLevel()
    {
        if(game.currentLevel < game.levelNames.Count -1)
        {
            game.currentLevel++;
            StartPlaying();
        }
    }

}
