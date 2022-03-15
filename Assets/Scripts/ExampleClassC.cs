using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class ExampleClassB
{
    [ModelHelp(true, "Uuid", "string")]
    public string uuid;
    [ModelHelp(true, "Phone", "string", true, false)]
    public string phone;
    [ModelHelp(true, "Hair", "string")]
    public string hair;
    [ModelHelp(true, "Cloth", "string")]
    public string cloth;
    [ModelHelp(true, "Pants", "string")]
    public string pants;
    [ModelHelp(true, "Shoes", "string")]
    public string shoes;
    [ModelHelp(true, "Skin", "string")]
    public string skin;

}
