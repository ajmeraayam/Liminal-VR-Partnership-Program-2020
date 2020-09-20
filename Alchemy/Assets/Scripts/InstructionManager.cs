using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public Queue<string> instructions;

    public Text instructionText;
    void Start()
    {
        instructions = new Queue<string>();
    }

    public void startInstruction(Instruction instruction)
    {
        Debug.Log("Instruction:" + instruction.title);

        instructions.Clear();

        foreach (string text in instruction.instructions)
        {
            instructions.Enqueue(text);
        }
    }

    public void nextInstruction()
    {
        // ends instruction when all queue is finished
        if (instructions.Count == 0)
        {
            endInstruction();
            return;
        }

        string instruction = instructions.Dequeue();
        instructionText.text = instruction;
        Debug.Log(instruction);
    }

    void endInstruction()
    {

    }

}
