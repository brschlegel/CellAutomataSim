using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleViewer : MonoBehaviour
{
    public List<Text> conditions;
    Text effect;
    Text horizontal;
    Text vertical;
    // Start is called before the first frame update
    void Start()
    {
        conditions = new List<Text>();
        Transform parent = transform.GetChild(0);
        for(int i = 0; i < 9; i++)
        {
            conditions.Add(parent.GetChild(i).GetComponent<Text>());
        }
        effect = transform.GetChild(1).GetComponent<Text>();
        horizontal = transform.GetChild(2).GetComponent<Text>();
        vertical = transform.GetChild(3).GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRule(Rule rule)
    {
        for(int i = 0; i < conditions.Count; i++)
        {
            conditions[i].text = (rule.data.condition[i]).ToString();
        }
        effect.text = rule.data.effect.ToString();
        horizontal.text = rule.data.horizontal.ToString();
        vertical.text = rule.data.vertical.ToString();
    }
}
