using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public GameObject[] mazePrefabs;    // List of maze prefabs
    public Transform mazeParent;        // Optional: Where the maze spawns
    public float shiftInterval = 4f;    // Seconds between each maze change

    private GameObject currentMaze;
    private int currentIndex = 0;

    void Start()
    {
        SpawnMaze(currentIndex);
        InvokeRepeating(nameof(ShiftMaze), shiftInterval, shiftInterval);
    }

    void ShiftMaze()
    {
        if (currentMaze != null)
            Destroy(currentMaze);  // remove old maze

        currentIndex = (currentIndex + 1) % mazePrefabs.Length;
        SpawnMaze(currentIndex);
    }

    void SpawnMaze(int index)
    {
        currentMaze = Instantiate(mazePrefabs[index], Vector3.zero, Quaternion.identity, mazeParent);
    }
}

