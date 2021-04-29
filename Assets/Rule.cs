using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    public int[] condition;
    public int effect;
    public string name;


    public Data(int[] condition, int effect, string name)
    {
        this.condition = condition;
        this.effect = effect;
        this.name = name;
    }
}
public class Rule 
{
    // Start is called before the first frame update
    public Rule next;
    public Rule previous;
    public Data data;
    public Rule(int[] condition, int effect, string name)
    {
       data = new Data(condition, effect, name);
    }

    public int getValue(int i, int j)
    {
        return data.condition[i + (j * 3)];
    }

    
}


