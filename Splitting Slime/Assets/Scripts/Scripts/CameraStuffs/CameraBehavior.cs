using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    public Transform player;
    Vector3 camMove;
    public bool UnlockZ = false;
    public bool UnlockY = false;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public bool cinematic = false;

    private void Awake()
    {
        SetSortMode();
        if (CameraManager.instance.mainCam == null)
        {
            CameraManager.instance.mainCam = GetComponent<Camera>();
        }
    }


    void Start()
    {
        player = PlayerManager.instance.Player.transform;
        transform.position = new Vector3(player.transform.position.x, minPosition.y, minPosition.z);
        minPosition.x -= Helpers.instance.SetAspect();
        maxPosition.x += Helpers.instance.SetAspect();
    }

    private void SetSortMode()
    {
        Camera cam = transform.GetComponent<Camera>();
        transform.GetComponent<Camera>().transparencySortMode = TransparencySortMode.CustomAxis;
        transform.GetComponent<Camera>().transparencySortAxis = new Vector3(0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cinematic)
        {
            camMove = transform.position;
            camMove.x = player.position.x;
            camMove.x = Mathf.Clamp(camMove.x, minPosition.x, maxPosition.x);
            if (UnlockZ)
            {
                camMove.z = player.position.z - Mathf.Abs(minPosition.z);
                camMove.z = Mathf.Clamp(camMove.z, minPosition.z, maxPosition.z);
            }
            if (UnlockY)
            {
                camMove.y = player.position.y + minPosition.y;
                camMove.y = Mathf.Clamp(camMove.y, minPosition.y, maxPosition.y);
            }
            transform.position = Vector3.MoveTowards(transform.position, camMove, 100 * Time.deltaTime);
        }
    }

    public void ControlledMove(Vector3 movePos, float timeToFinish)
    {
        cinematic = true;
        transform.DOMove(movePos, timeToFinish).OnComplete(ControlledMoveEnd);
    }

    public void TeleportToPlayerNoZChange()
    {
        Vector3 move = new Vector3(player.position.x, player.position.y + minPosition.y, minPosition.z);
        transform.position = move;
    }

    public void ControlledMoveEnd()
    {
        cinematic = false;
    }
}
