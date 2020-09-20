using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    public static Instruction instruction;

    public static void triggerInstruction()
    {
        FindObjectOfType<InstructionManager>().startInstruction(instruction);
    }

}
