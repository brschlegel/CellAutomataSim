using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType{Dead = 0, Alive = 1, Wall = 2, Goal = 3};
public static class GlobalVars 
{
    public static Dictionary<CellType, Color> typeToColor = new Dictionary<CellType, Color>{
        {CellType.Alive,new Color(255,0,0)},{CellType.Dead,new Color(255,255,255)},{CellType.Wall, new Color(0,0,0)},{CellType.Goal,new Color(0,0,255)}
    };
    public static Dictionary<Color, CellType> colorToType = new Dictionary<Color, CellType>{
        {new Color(255,0,0),CellType.Alive},{new Color(255,255,255), CellType.Dead},{ new Color(0,0,0),CellType.Wall},{new Color(0,0,255),CellType.Goal}
    };

    public static int drawing = -1;
}
