using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndGame : MonoBehaviour
{
    PlayerController player;
    CameraBehavior behavior;
    Transform camTransform;
    PlayableDirector dir;

    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        behavior = CameraManager.instance.mainCam.GetComponent<CameraBehavior>();
        camTransform = CameraManager.instance.mainCam.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.HideStatusBar();
            player.canPlay = false;
            behavior.cinematic = true;
            camTransform.position = new Vector3(9.23f, 75, -11.2f);
            StartCoroutine(QuitGame());
        }
    }

    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManagerScript.instance.QuitToTitle();
        yield break;
    }
}
