using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CellManager : MonoBehaviour
{

    public Transform cellPrefab;
    public RuleHeirarchy rules;
    public GameObject[,] matrix;

    public WorldCreation world;

    public bool paused;

    


    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        rules = GetComponent<RuleHeirarchy>();
        world = GetComponent<WorldCreation>();
        matrix = world.matrix;
      
        rules.Init(matrix);
        InvokeRepeating("Step", .25f,.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Step()
    {
        if(paused)
            return;
        int[,] newBoard = rules.Step(matrix);
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j].GetComponent<SpriteRenderer>().color != GlobalVars.typeToColor[CellType.Wall])
                    matrix[i,j].GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[(CellType)newBoard[i,j]];
            }
        }
    }

    public void Pause()
    {
        paused = !paused;
    }

    
}
