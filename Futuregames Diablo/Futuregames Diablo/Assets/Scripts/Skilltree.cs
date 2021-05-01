using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skilltree : MonoBehaviour
{

    public bool[] skillArray1 = new bool[3];
    public bool[] skillArray2 = new bool[3];
   
    //tickdamage AOE ability
    public void Ability1()
    {
        skillArray1 = new bool[3];
        skillArray1[0] = true;
    }
    // slow AOE ability
    public void Ability2()
    {
        skillArray1 = new bool[3];
        skillArray1[1] = true;
    }
    // movementspeed buff AOE ability
    public void Ability3()
    {
        skillArray1 = new bool[3];
        skillArray1[2] = true;
    }
    // fear enemies
    public void Ability4()
    {
        skillArray2 = new bool[3];
        skillArray2[0] = true;
    }
   // player sacrifice HP for MANA
    public void Ability5()
    {
        skillArray2 = new bool[3];
        skillArray2[1] = true;
    }
    // channel heal
    public void Ability6()
    {
        skillArray2 = new bool[3];
        skillArray2[2] = true;
    }
}
