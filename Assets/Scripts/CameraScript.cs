using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Public params
    public Vector2 ratioVector = new Vector2(16f, 9f);
    public Player player;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
            FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector2 playerPos = player.transform.position;
        Vector3 cameraPos = transform.position;
        cameraPos.x = playerPos.x;
        cameraPos.y = playerPos.y;
        transform.position = cameraPos;
    }

    float GetSourceAspectRatio()
    {
        return ratioVector.x / ratioVector.y;
    }

    float GetScreenAspectRatio()
    {
        return (float)Screen.width / (float)Screen.height;
    }

    void UpdateBlackBars()
    {
        camera = GetComponent<Camera>();

        var aspectRatio = GetScreenAspectRatio();
        var targetRatio = GetSourceAspectRatio();

        var greater = Mathf.Max(aspectRatio, targetRatio);
        var smaller = Mathf.Min(aspectRatio, targetRatio);

        var width = 1f;
        var height = 1f;
        if (aspectRatio < targetRatio)
        {
            height = smaller / greater;
        }
        else
        {
            width = smaller / greater;
        }

        float x = 0.5f - width / 2;
        float y = 0.5f - height / 2;

        camera.rect = new Rect(x, y, width, height);
    }
}
