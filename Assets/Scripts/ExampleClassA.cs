using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class ExampleClassA: ExampleBase
{

    [ModelHelp(true, "Class", "string")]
    public string[] className;

    [ModelHelp(true, "CoreClass", "string")]
    public string coreClass;

    [ModelHelp(true, "X", "double")]
    public double x;

    [ModelHelp(true, "Y", "double")]
    public double y;

    [ModelHelp(true, "Z", "double")]
    public double z;

}
