using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    public int[] condition;
    public int effect;
    public string name;

    public bool horizontal;
    public bool vertical;


    public Data(int[] condition, int effect, string name, bool horizontal, bool vertical)
    {
        this.condition = condition;
        this.effect = effect;
        this.name = name;
        this.horizontal = horizontal;
        this.vertical = vertical;
    }
}
public class Rule 
{
    // Start is called before the first frame update
    public Rule next;
    public Rule previous;
    public Data data;
    public Rule(int[] condition, int effect, string name, bool hor, bool vert)
    {
       data = new Data(condition, effect, name, hor, vert);
    }

    public int getValue(int i, int j)
    {
        return data.condition[i + (j * 3)];
    }

    
}


