using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Alive];
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Wall];
        }
        else if (Input.GetMouseButtonDown(2))
        {
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Goal];
        }
    }


}
