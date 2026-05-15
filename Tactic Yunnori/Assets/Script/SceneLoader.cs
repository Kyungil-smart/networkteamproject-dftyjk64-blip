using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _loadSceneIndex;

    private void Start()
    {
        SceneManager.LoadScene(_loadSceneIndex);
    }
}
