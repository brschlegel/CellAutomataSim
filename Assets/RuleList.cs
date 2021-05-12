using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RuleList : MonoBehaviour
{
    public Transform buttonPrefab;
    Dictionary<GameObject, Rule> buttons;
    Dictionary<Rule, GameObject> inverseButtons;
    public RuleViewer viewer;

    void Start()
    {
        buttons = new Dictionary<GameObject, Rule>();
        inverseButtons = new Dictionary<Rule, GameObject>();
    }

    public void AddButton(Rule rule)
    {
        int offset = 35;
        GameObject b = Instantiate(buttonPrefab,new Vector3(0,0,0),Quaternion.identity, transform ).gameObject;
        float y = 0;
        if(rule.previous != null)
             y = inverseButtons[rule.previous].transform.localPosition.y - offset;

        b.transform.localPosition = new Vector3(0,y,0);
        b.transform.GetChild(0).GetComponent<Text>().text = rule.data.name;

        buttons[b] = rule;
        inverseButtons[rule] = b;
        b.GetComponent<Button>().onClick.AddListener(ChangeSelected);
    }

    public void ChangeSelected()
    {
        viewer.ChangeRule(buttons[EventSystem.current.currentSelectedGameObject]);
    }
    public void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
            buttons.Clear();
            inverseButtons.Clear();
        }
    }
}
