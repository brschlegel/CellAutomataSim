using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleCreation : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Dropdown> conditions;
    public Dropdown effect;
    public GameObject world;
    public InputField nameInput;
    RuleHeirarchy heirarchy;
    public RuleEditor editor;
    public GameObject editorObj;
    public Text status;
    GameObject createButton;
    Rule editing;
    void Start()
    {
        Transform parent = transform.GetChild(0);
        for(int i = 0; i < 9; i++)
        {
            conditions.Add(parent.GetChild(i).GetComponent<Dropdown>());
        }
        effect = transform.GetChild(1).GetComponent<Dropdown>();    
        heirarchy = world.GetComponent<RuleHeirarchy>();
        editor = editorObj.GetComponent<RuleEditor>();
        status = GameObject.Find("Status").GetComponent<Text>();
        editing = null;
        createButton = GameObject.Find("CreateButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRule()
    {
        
        int[] cond = new int[9];
        for(int i = 0; i < conditions.Count; i++)
        {
           cond[i] = conditions[i].value - 1;
        }
        int eff;
        eff = effect.value;
        if(editing == null)
        {
            Rule created = heirarchy.CreateRule(cond, eff, nameInput.text);
            if (created != null)
            {
                status.text = "Success!";
                editor.AddButton(created);
            }
            else
            {
                status.text = "1 cell must be alive";
            }
        }
        else
        {
            if(heirarchy.ValidateRule(cond))
            {
                status.text = "Edited!";
                heirarchy.EditRule(editing, cond, eff, nameInput.text);
                editing = null;
                createButton.transform.GetChild(0).GetComponent<Text>().text = "Create";
            }
            else
            {
                status.text = "1 cell must be alive";
            }
        }
    }

    public void SetUpEdit(Rule rule)
    {
        for(int i = 0; i < conditions.Count; i++)
        {
            conditions[i].value = rule.data.condition[i] + 1;
        }
        effect.value = rule.data.effect;
        nameInput.text = rule.data.name;
        createButton.transform.GetChild(0).GetComponent<Text>().text = "Edit";
        editing = rule;

    }

    
}
