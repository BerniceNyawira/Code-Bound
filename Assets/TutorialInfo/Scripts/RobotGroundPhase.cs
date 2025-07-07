using UnityEngine;

public class RobotGroundPhase : MonoBehaviour
{
    public Renderer groundRenderer; // Assign the ground’s MeshRenderer
    public Material normalMaterial; // Default material
    public Material redMaterial;    // Red alert material

    public Transform player; // Drag the player
    public float triggerDistance = 5f;

    public RobotChase[] robots; // Optional: assign if you want chasing

    private bool activated = false;

    void Update()
    {
        if (player == null || groundRenderer == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= triggerDistance && !activated)
        {
            activated = true;
            groundRenderer.material = redMaterial;

            foreach (RobotChase robot in robots)
            {
                if (robot != null) robot.StartChasing();
            }
        }
        else if (dist > triggerDistance && activated)
        {
            activated = false;
            groundRenderer.material = normalMaterial;

            foreach (RobotChase robot in robots)
            {
                if (robot != null) robot.StopChasing();
            }
        }
    }
}
