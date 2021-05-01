using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WorldCreation : MonoBehaviour
{
    public GameObject[,] matrix;
    public int width = 40;
    public int height = 40;
    public InputField nameInput;

    public float cellWidth = 1;
    public float cellHeight = 1;
    public Transform cellPrefab;
    void Awake()
    {
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

        nameInput = GameObject.Find("SaveName").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        string filename = "Levels/"+  nameInput.text + ".txt";
        string file = "";
        using (StreamWriter writer = new StreamWriter(filename))
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    file += (int)GlobalVars.colorToType[matrix[i, j].GetComponent<SpriteRenderer>().color] + ",";
                }
                file += "\n";
            }
            writer.Write(file);
        }
    }

    public void Load(string LevelName)
    {
        int [,] entries = new int[width, height];

        using(StreamReader reader = new StreamReader("Levels/" + LevelName + ".txt"))
        {
            for(int i = 0; i < width; i++)
            {
                string[] line = reader.ReadLine().Split(',');
                for(int j = 0; j < height; j++)
                {
                   int entry = int.Parse(line[j]);
                   matrix[i,j].GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[(CellType)entry];
                }
            }
        }
    }

    public void LoadWithInput()
    {
        Load(nameInput.text);
    }
}
