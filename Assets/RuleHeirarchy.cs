using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleHeirarchy : MonoBehaviour
{
    Rule top;
    public Rule bottom;
    public int count = 0;

    int[,] prevTypes;
    void Start()
    {
        top = null;
        bottom = null;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public Rule CreateRule(int[] condition, int effect, string name, bool hor, bool vert)
    {
        if (ValidateRule(condition))
        {
            Rule newRule =  new Rule(condition, effect, name, hor, vert);
            if (top == null)
            {
                
                top = newRule;
                bottom = top;
            }
            else
            {
                newRule.previous = bottom;
                bottom.next = newRule;
                bottom = newRule;
            }
            count++;
         
            PrintHeirarchy();
            return newRule;
        }
        return null;
    }
    public void Init(GameObject[,] matrix)
    {
        prevTypes = new int[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                prevTypes[i, j] = (int)(GlobalVars.colorToType[matrix[i, j].GetComponent<SpriteRenderer>().color]);
            }
        }
    }

    public int[,] Step(GameObject[,] matrix)
    {
        Rule currentRule = bottom;
        int[,] newTypes = prevTypes;
        while (currentRule != null)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    CellType center = GlobalVars.colorToType[matrix[i, j].GetComponent<SpriteRenderer>().color];
                    if (center != CellType.Wall)
                    {
                        if (checkCell(matrix, currentRule, i, j))
                        {
                            newTypes[i, j] = currentRule.data.effect;
                        }

                    }
                }
            }
            currentRule = currentRule.previous;
        }
        prevTypes = newTypes;
        return newTypes;
    }

    bool checkCell(GameObject[,] matrix, Rule rule, int i, int j)
    {
        return CheckSym(matrix, rule, i, j, "none") || CheckSym(matrix, rule, i , j, "horizontal") ||CheckSym(matrix, rule, i , j, "vertical") || CheckSym(matrix, rule, i , j, "diagonal");
    }

    //Yeah, this code is pretty repeated but I'm over it
    bool CheckSym(GameObject[,] matrix, Rule rule, int i, int j, string type)
    {
        switch(type)
        {
            case "horizontal":
            if(!rule.data.horizontal)
                return false;
                break;
            case "vertical":
            if(!rule.data.vertical)
                return false;
                break;
            case "diagonal":
            if(!(rule.data.horizontal && rule.data.vertical))
                return false;
                break;
        }

        for (int m = -1; m < 2; m++)
        {
            for (int n = -1; n < 2; n++)
            {
                int matrixValue = (int)(GlobalVars.colorToType[matrix[i + m, j + n].GetComponent<SpriteRenderer>().color]);
                int ruleValue = GetRuleValue(type, rule, m,n);
                if(!CheckRuleValue(matrixValue, ruleValue))
                {
                    return false;
                }
            }
        }
        return true;
    }

    int GetRuleValue(string type, Rule rule, int m, int n)
    {
        switch(type)
        {
            case "none":
                return rule.getValue(m + 1, n + 1);
                break;
            case "horizontal":
                return rule.getValue(1 - m, n+1);
                break;
            case "vertical":
                return rule.getValue(m+1, 1-n);
                break;
            case "diagonal":
                return rule.getValue(1-m, 1-n);
                break;
        }
        return -2;
    }

    public void DeleteRule(Rule rule)
    {
        if (bottom == rule)
        {
            bottom = rule.previous;
            bottom.next = null;
            return;
        }
        if (top == rule)
        {
            top = rule.next;
            top.previous = null;
            return;
        }
        rule.next.previous = rule.previous;
        rule.previous.next = rule.next;

    }

    bool CheckRuleValue(int matrixValue, int ruleValue)
    {
        if (ruleValue != -1)
        {
            if (matrixValue != ruleValue)
            {
                return false;
            }

        }
        return true;
    }

    public void PrintHeirarchy()
    {
        Rule currentRule = top;
        Debug.Log("From the Top rope------------------------");
        string log = " ";
        while (currentRule != null)
        {
            log += currentRule.data.name + " -> ";
            currentRule = currentRule.next;
        }
        Debug.Log(log);

        currentRule = bottom;
        Debug.Log("From the Bottom rope------------------------");
        log = " ";
        while (currentRule != null)
        {
            log += " <- " + currentRule.data.name;
            currentRule = currentRule.previous;
        }
        Debug.Log(log);

    }

    public void Swap(Rule a, Rule b)
    {
        Data temp = a.data;
        a.data = b.data;
        b.data = temp;
        PrintHeirarchy();

    }

    public bool ValidateRule(int[] conditions)
    {
        //Make sure a cell is alive
        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public void EditRule(Rule rule, int[] conditions, int effect, string name, bool horizontal, bool vertical)
    {
        if(ValidateRule(conditions))
        {
            rule.data.condition = conditions;
            rule.data.effect = effect;
            rule.data.name = name;
            rule.data.horizontal = horizontal;
            rule.data.vertical = vertical;
        }
    }


}
