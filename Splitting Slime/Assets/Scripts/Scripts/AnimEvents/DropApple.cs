using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.ComponentModel.Design.Serialization;

public class DropApple : MonoBehaviour
{
    private GameObject apple;
    public returnableObject me;
    public returnableObject bottomArrow;
    public Transform player;
    private PlayerController playerCont;
    public LayerMask mask;
    public Vector3 appleLocalPos;
    public Vector3 explosionMove = Vector3.zero;

    private void Awake()
    {
        playerCont = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }
    private void Start()
    {
        apple = transform.GetChild(0).gameObject;
        me = new returnableObject(transform.parent.gameObject, null, null);
    }

    private void OnEnable()
    {
        bottomArrow = ObjectPooling.instance.SpawnFromPool("BottomArrow", new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity, false);
    }

    public void spawnAppleAndDrop()
    {
        apple.SetActive(true);
        apple.transform.DOMoveY(0.4f, .6f).OnComplete(returnItemToStack);
    }

    private void returnItemToStack()
    {
        int length = Physics.OverlapBox(apple.transform.position, (apple.transform.localScale / 2) * 1.4f, Quaternion.identity, mask).Length;
        if(length > 0)
        {
            playerCont.TakeDamage(5);
        }
        ObjectPooling.instance.SpawnFromPool("Explosion", apple.transform.position + explosionMove, Quaternion.identity, false);
        apple.transform.localPosition = appleLocalPos;
        apple.SetActive(false);
        ObjectPooling.instance.addToPool("BottomArrow", bottomArrow);
        ObjectPooling.instance.addToPool("AppleDrop", me);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    if (m_started)
    //        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //        Gizmos.DrawWireCube(apple.transform.position, apple.transform.localScale * 1.4f);
    //}
}
