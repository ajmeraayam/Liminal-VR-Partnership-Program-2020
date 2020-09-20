using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Instruction
{
    public string title; // unused

    [TextArea(3, 20)]
    public string[] instructions;

}
