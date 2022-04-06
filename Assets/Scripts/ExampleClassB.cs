using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class ExampleClassC: ExampleBase
{

    [ModelHelp(true, "Uuid", "string")]
    public string uuid;

    [ModelHelp(true, "Phone", "string", true, false)]
    public string phone;
}
