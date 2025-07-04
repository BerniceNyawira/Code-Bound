using UnityEngine;

public class RobotPhase : MonoBehaviour
{
    public float startDelay = 6f;
    public MazeGlowController glow;
    public RobotChase[] robotsToControl; // Assign robots here manually in Inspector
    public Transform player; // Drag the player here
    public float glowTriggerDistance = 5f; // Distance to glow red

    private bool glowing = false;
    private bool phaseActive = false;

    void Start()
    {
        Invoke(nameof(StartRobotPhase), startDelay);
    }

    void Update()
    {
        if (!phaseActive || glow == null || player == null)
            return;

        foreach (RobotChase robot in robotsToControl)
        {
            if (robot == null) continue;

            float dist = Vector3.Distance(robot.transform.position, player.position);

            if (dist <= glowTriggerDistance)
            {
                if (!glowing)
                {
                    glow.SetRedGlow();
                    glowing = true;
                }
                return; // No need to check other robots if one is close enough
            }
        }

        // If no robot is close enough
        if (glowing)
        {
            glow.SetNormalGlow();
            glowing = false;
        }
    }

    void StartRobotPhase()
    {
        phaseActive = true;

        foreach (RobotChase robot in robotsToControl)
        {
            if (robot != null)
                robot.StartChasing();
        }
    }

    void EndRobotPhase()
    {
        phaseActive = false;

        if (glow != null)
            glow.SetNormalGlow();

        foreach (RobotChase robot in robotsToControl)
        {
            if (robot != null)
                robot.StopChasing();
        }

        glowing = false;
    }
}
