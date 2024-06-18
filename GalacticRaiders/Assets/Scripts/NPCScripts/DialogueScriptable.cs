using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Template", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string[] voicelines;
}