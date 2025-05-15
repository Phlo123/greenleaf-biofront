using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 20f, -6f);

    private Transform player;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("CameraFollow: No GameObject found with tag 'Player'");
            return;
        }

        player = playerObj.transform;
    }

    void LateUpdate()
    {
        if (player == null) return;

        cam.position = player.position + offset;
        cam.LookAt(player.position); // Optional: keeps the camera aimed at the player
    }
}
