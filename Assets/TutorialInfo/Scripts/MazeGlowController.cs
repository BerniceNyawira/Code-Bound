using UnityEngine;

public class MazeGlowController : MonoBehaviour
{
    public Material normalGlowMaterial; // Drag Mazeglowing here
    public Material redGlowMaterial;    // Drag Mazeglowred here

    private Renderer[] mazeWallRenderers;

    void Start()
    {
        // Find all maze walls tagged as "MazeWall"
        GameObject[] mazeWalls = GameObject.FindGameObjectsWithTag("MazeWall");
        mazeWallRenderers = new Renderer[mazeWalls.Length];

        for (int i = 0; i < mazeWalls.Length; i++)
        {
            mazeWallRenderers[i] = mazeWalls[i].GetComponent<Renderer>();
        }
    }

    public void SetNormalGlow()
    {
        foreach (Renderer rend in mazeWallRenderers)
        {
            if (rend != null)
                rend.material = normalGlowMaterial;
        }
    }

    public void SetRedGlow()
    {
        foreach (Renderer rend in mazeWallRenderers)
        {
            if (rend != null)
                rend.material = redGlowMaterial;
        }
    }
}
