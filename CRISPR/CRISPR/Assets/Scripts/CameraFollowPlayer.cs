using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;

    void FixedUpdate()
    {
        if (player == null) return;
        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.fixedDeltaTime);
    }
}
