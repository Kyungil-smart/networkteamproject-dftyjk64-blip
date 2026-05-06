using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class YutNode
{
    public string nodeName;
    public Vector3 position;

    public List<int> nextNodes = new List<int>();

    public bool IsBranchNode => nextNodes.Count > 1;
}
