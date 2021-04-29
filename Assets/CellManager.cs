using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CellManager : MonoBehaviour
{

    public Transform cellPrefab;
    public RuleHeirarchy rules;
    public GameObject[,] matrix;
    public float cellWidth = 1;
    public float cellHeight = 1;
    public bool paused;
    
    public int width = 600;
    public int height = 600;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        rules = GetComponent<RuleHeirarchy>();
        matrix = new GameObject[width, height];
        float offset = 5.0f/width;
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Transform g = Instantiate(cellPrefab, new Vector3(((cellWidth  + offset)* i) - ((cellWidth  + offset) * (width)/2.0f) , ((cellHeight + offset) * j) - ((cellHeight  + offset) * (height)/2.0f), 0), Quaternion.identity);
                matrix[i,j] = g.gameObject;
                if(i % (width-1) == 0 || j % (height -1)  == 0)
                {
                    g.GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Wall];
                }
                else
                {
                    g.GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Dead];
                }
            }
        }
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
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
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
