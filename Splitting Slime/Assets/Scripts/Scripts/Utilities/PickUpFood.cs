using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class PickUpFood : MonoBehaviour
{
    public float healthPercent = .1f;
    public PlayerController player;
    public float time = 20;
    public float compareTime = 0;
    public returnableObject me;
    public string poolTag;
    [SerializeField]
    private bool doAnim = true;
    private GameObject shadow;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float distanceAboveGround = .45f;

    private void Awake()
    {
        shadow = transform.Find("Shadow").gameObject;
    }
    public void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (compareTime > time)
        {
            if (me == null)
            {
                me = new returnableObject(gameObject, null, null);
            }
            ObjectPooling.instance.addToPool(poolTag, me);
        }
        else
        {
            compareTime += Time.deltaTime;
        }
    }

    public void OnEnable()
    {
        if (doAnim)
        {
            shadow.SetActive(false);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5, mask))
            {
                transform.DOJump(new Vector3(hit.point.x, hit.point.y + distanceAboveGround, hit.point.z), 1.5f, 2, 2).OnComplete(ActivateShadow);
            }
        }
    }

    private void ActivateShadow()
    {
        shadow.SetActive(true);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(me == null)
            {
                me = new returnableObject(gameObject, null, null);
            }
            player.AddHealth(Mathf.Ceil(player.myStats.maxHealth * healthPercent));
            ObjectPooling.instance.addToPool(poolTag, me);
        }
    }
}
