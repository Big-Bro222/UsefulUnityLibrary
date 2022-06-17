using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBase :IData
{
    [ModelHelp(true, "Name", "string")]
    public string name;
}
