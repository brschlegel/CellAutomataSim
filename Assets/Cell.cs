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
            if(GetComponent<SpriteRenderer>().color == GlobalVars.typeToColor[CellType.Alive])
            {
                GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Dead];
                GlobalVars.drawing = 0;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Alive];
                GlobalVars.drawing = 1;
            }
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Wall];
            GlobalVars.drawing = 2;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Goal];
            GlobalVars.drawing = 3;
        }

        switch(GlobalVars.drawing)
        {
            case 0:
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Dead];
            if(Input.GetMouseButtonUp(0))
                GlobalVars.drawing = -1;
            break;
            case 1:
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Alive];
            if(Input.GetMouseButtonUp(0))
                GlobalVars.drawing = -1;
            break;
            case 2:
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Wall];
            if(Input.GetMouseButtonUp(1))
                GlobalVars.drawing = -1;
            break;
            case 3:
            GetComponent<SpriteRenderer>().color = GlobalVars.typeToColor[CellType.Goal];
            if(Input.GetMouseButtonUp(2))
                GlobalVars.drawing = -1;
            break;
        }
    }




}
