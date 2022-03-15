using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class ExampleClassC
{
    [ModelHelp(true, "Name", "string")]
    public string name;

    [ModelHelp(true, "Uuid", "string")]
    public string uuid;

    [ModelHelp(true, "Phone", "string", true, false)]
    public string phone;
}
