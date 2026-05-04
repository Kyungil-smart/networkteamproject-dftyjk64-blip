using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YutMapData", menuName = "YutGame/MapData")]
public class YutMapData : ScriptableObject
{
    public List<YutNode> nodes = new List<YutNode>();
}
