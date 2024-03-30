using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueLine
{
    public bool playerMessage;
    [TextArea]
    public string message;
}