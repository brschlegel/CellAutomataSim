using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RuleEditor : MonoBehaviour
{
    public GameObject world;
    RuleHeirarchy heirarchy;
    public Transform buttonPrefab;
    public GameObject creatorObj;
    RuleCreation creator;
    Dictionary<GameObject, Rule> buttons;
    Dictionary<Rule, GameObject> inverseButtons;
    GameObject selected;
    bool swapping;
    GameObject swappingTo;
    Text selectedText;
    GameObject swapButton;
    GameObject deleteButton;

    Transform ruleParent;
    // Start is called before the first frame update
    void Start()
    {
        heirarchy = world.GetComponent<RuleHeirarchy>();
        buttons = new Dictionary<GameObject, Rule>();
        inverseButtons = new Dictionary<Rule, GameObject>();
        selectedText = transform.GetChild(0).GetComponent<Text>();
        swapButton = GameObject.Find("Swap");
        deleteButton = GameObject.Find("Delete");
        deleteButton.SetActive(false);
        swapButton.SetActive(false);
        creator = creatorObj.GetComponent<RuleCreation>();
        ruleParent = GameObject.Find("RuleParent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddButton(Rule rule)
    {
        int offset = 35;
        GameObject b = Instantiate(buttonPrefab,new Vector3(0,0,0),Quaternion.identity, ruleParent ).gameObject;
        float y = 0;
        if(rule.previous != null)
             y = inverseButtons[rule.previous].transform.localPosition.y - offset;

        b.transform.localPosition = new Vector3(0,y,0);
        b.transform.GetChild(0).GetComponent<Text>().text = rule.data.name;

        buttons[b] = rule;
        inverseButtons[rule] = b;
        b.GetComponent<Button>().onClick.AddListener(ChangeSelected);
        b.GetComponent<Button>().onClick.AddListener(SetUpViewSelected);
    }

    public void ChangeSelected()
    {
        if(!swapping)
        {
        selected = EventSystem.current.currentSelectedGameObject;
        selectedText.text = "Selected Rule: " + selected.transform.GetChild(0).GetComponent<Text>().text;
        deleteButton.SetActive(true);
        swapButton.SetActive(true);
        }
        else
        {
            swappingTo = EventSystem.current.currentSelectedGameObject;
            SwapButtons();
        }
    }

    public void Deselect()
    {
        selected = null;
        selectedText.text = "Selected Rule: ";
        deleteButton.SetActive(false);
        swapButton.SetActive(false);
        swapping = false;

    }

    public void DeleteSelected()
    {
        MoveRulesUp(buttons[selected]);
        heirarchy.DeleteRule(buttons[selected]);
        inverseButtons.Remove(buttons[selected]);
        buttons.Remove(selected);
        DestroyImmediate(selected);
        selected = null;
    }

    public void MoveRulesUp(Rule rule)
    {
        if(rule.next != null)
        {
        Rule currentRule = heirarchy.bottom;
        while(currentRule != rule)
        {
            GameObject g = inverseButtons[currentRule];
            GameObject above = inverseButtons[currentRule.previous];
            g.transform.localPosition = above.transform.localPosition;
            currentRule = currentRule.previous;
        }
        }

    }

    public void SwapOnClick()
    {
        swapping = true;
    }

    public void SwapButtons()
    {   
        string selectedName = selected.transform.GetChild(0).GetComponent<Text>().text;
        selected.transform.GetChild(0).GetComponent<Text>().text = swappingTo.transform.GetChild(0).GetComponent<Text>().text;
        swappingTo.transform.GetChild(0).GetComponent<Text>().text = selectedName;
        heirarchy.Swap(buttons[selected], buttons[swappingTo]);
        swapping = false;
    }

    public void EditSelected()
    {
        creator.SetUpEdit(buttons[selected]);
    }

    public void SetUpViewSelected()
    {
        creator.SetUpView(buttons[selected]);
    }

    public void Clear()
    {
          foreach(Transform child in ruleParent)
        {
            Destroy(child.gameObject);
        
        }
        buttons.Clear();
        inverseButtons.Clear();
    }
}
