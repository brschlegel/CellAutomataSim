using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleHeirarchy : MonoBehaviour
{
    Rule top;
    public Rule bottom;
    int count = 0;

    public RuleEditor editor;
    public GameObject editorObj;
    int[,] prevTypes;
    void Start()
    {
        top = null;
        bottom = null;
        editor = editorObj.GetComponent<RuleEditor>();

    }



    // Update is called once per frame
    void Update()
    {

    }

    public bool CreateRule(int[] condition, int effect, string name)
    {
        if (ValidateRule(condition))
        {
            if (top == null)
            {
                top = new Rule(condition, effect, name);
                bottom = top;
            }
            else
            {
                Rule newRule = new Rule(condition, effect, name);
                newRule.previous = bottom;
                bottom.next = newRule;
                bottom = newRule;
            }
            count++;
            editor.AddButton(bottom);
            PrintHeirarchy();
            return true;
        }
        return false;
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
        for (int m = -1; m < 2; m++)
        {
            for (int n = -1; n < 2; n++)
            {

                int matrixValue = (int)(GlobalVars.colorToType[matrix[i + m, j + n].GetComponent<SpriteRenderer>().color]);
                int ruleValue = rule.getValue(m + 1, n + 1);
                if (ruleValue != -1)
                {
                    if (matrixValue != ruleValue)
                        return false;
                }

            }
        }
        return true;
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

    public void EditRule(Rule rule, int[] conditions, int effect, string name)
    {
        if(ValidateRule(conditions))
        {
            rule.data.condition = conditions;
            rule.data.effect = effect;
            rule.data.name = name;
        }
    }


}
