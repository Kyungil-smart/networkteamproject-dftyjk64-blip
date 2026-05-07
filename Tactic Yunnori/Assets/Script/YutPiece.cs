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
            StartCoroutine(MoveRoutine(1));
        }
    }

    private int GetClickedNodeIndex()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                YutNodeProperty nodeProp = hit.collider.GetComponentInParent<YutNodeProperty>();

                if (nodeProp != null)
                {
                    Debug.Log($"클릭한 노드 번호: {nodeProp.nodeIndex}");
                    return nodeProp.nodeIndex;
                }
            }
        }
        return -1;
    }
    public IEnumerator MoveRoutine(int moveCount)
    {
        for (int i = 0; i < moveCount; i++)
        {
            YutNode currentNode = mapData.nodes[currentNodeIndex];
            int nextIndex = -1;

            if (i == 0 && currentNode.IsBranchNode)
            {
                Debug.Log("이동할 방향의 노드를 클릭하세요!");

                while (nextIndex == -1)
                {
                    int clickedIndex = GetClickedNodeIndex();

                    if (currentNode.nextNodes.Contains(clickedIndex))
                    {
                        nextIndex = clickedIndex;
                    }
                    yield return null;
                }
            }
            else
            {
                nextIndex = currentNode.nextNodes[0];
            }

            currentNodeIndex = nextIndex;
            yield return StartCoroutine(MoveToNode(mapData.nodes[currentNodeIndex].position));
        }
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

