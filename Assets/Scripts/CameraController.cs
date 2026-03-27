using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject center;
    [SerializeField]
    float zDistance;
    [SerializeField]
    float minCenterDistance;
    [SerializeField]
    float maxCameraSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zDistance);
        center.transform.position = player.transform.position;
    }
    float currentSpeed = 0f;
    [SerializeField]
    float minDistance = 0.2f;
    [SerializeField]
    float minSpeed = 1f;
    // Update is called once per frame
    void LateUpdate()
    {
        // if (Vector3.Distance(player.transform.position, center.transform.position) > minCenterDistance)
        // {
        //     Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, zDistance);
        //     transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        // }
        // else if (Vector3.Distance(player.transform.position, center.transform.position) > 0f)
        // {
        //     Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, zDistance);
        //     transform.position = Vector3.Lerp(transform.position, targetPosition, closeCameraSpeed * Time.deltaTime);
        // }
        // transform.rotation = player.transform.rotation;
        float distance = Vector3.Distance(player.transform.position, center.transform.position);
        currentSpeed = Mathf.Lerp(minSpeed, maxCameraSpeed, (distance-minDistance)/minCenterDistance);
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, zDistance);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        transform.rotation = player.transform.rotation;
    }
}
