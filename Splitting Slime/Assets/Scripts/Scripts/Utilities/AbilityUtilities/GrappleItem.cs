using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrappleItem : MonoBehaviour
{
    [SerializeField]
    private Vector3 endPos, camMinPos, camMaxPos;

    private PlayerController player;
    private Rigidbody playerRigid;
    private CameraBehavior cam;
    private Transform camTrans;

    [SerializeField]
    private float jumpPower = 2, duration = 5;

    [SerializeField]
    FallDamage fallObj;

    private void Start()
    {
        GameObject tempPlayer = PlayerManager.instance.Player;
        player = tempPlayer.GetComponent<PlayerController>();
        playerRigid = tempPlayer.GetComponent<Rigidbody>();
        cam = CameraManager.instance.mainCam.GetComponent<CameraBehavior>();
        camTrans = CameraManager.instance.mainCam.transform;
        camMinPos.x -= Helpers.instance.SetAspect();
        camMaxPos.x += Helpers.instance.SetAspect();
    }
    //Works well
    public void launchPlayer()
    {
        player.canPlay = false;
        cam.UnlockY = true;
        playerRigid.isKinematic = true;
        player.transform.DOJump(endPos, jumpPower, 1, duration).OnComplete(lockCamY);
    }

    public void lockCamY()
    {
        cam.minPosition = camMinPos;
        cam.maxPosition = camMaxPos;
        camTrans.DOMove(camMinPos, 2);
        cam.UnlockY = false;
        playerRigid.isKinematic = false;
        player.canPlay = true;
        fallObj.enabled = true;
    }
}
