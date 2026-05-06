using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class YutPiece : MonoBehaviour
{
    public YutMapData mapData;
    public int currentNodeIndex = 0;
    public bool isFinished = false;

    private void Start()
    {
        if (mapData != null && mapData.nodes.Count > 0)
        {
            transform.position = mapData.nodes[currentNodeIndex].position;
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) 
        {
            MoveOneStep();
        }
    }

    void MoveOneStep()
    {
        int nextIndex = (currentNodeIndex + 1) % mapData.nodes.Count;
        currentNodeIndex = nextIndex;

        StartCoroutine(MoveToNode(mapData.nodes[currentNodeIndex].position));
    }

    public IEnumerator MoveToNode(Vector3 targetPos)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }
        transform.position = targetPos;
    }
}

