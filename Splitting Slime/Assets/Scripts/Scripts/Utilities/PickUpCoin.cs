using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using UnityEngine.UI;

public class PickUpCoin : MonoBehaviour
{
    private PlayerController player;
    public string poolTag;
    public returnableObject me;
    public float time = 10;
    private float CompareTime = 0;
    private bool keepCounting = true;
    public int coinAmount = 1;
    private VisualEffect effect;
    private GameObject shadow;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    private bool doAnim = true;
    [SerializeField]
    private float distanceAboveGround = .45f;
    [SerializeField]
    private AudioClip pickupSound;
    private void Awake()
    {
        poolTag = gameObject.tag;
        effect = GetComponentInChildren<VisualEffect>();
        shadow = transform.Find("Shadow").gameObject;
    }

    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }

    private void OnEnable()
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

    private void Update()
    {
        if(CompareTime > time && keepCounting)
        {
            ReturnToPool();
        }
        else
        {
            CompareTime += Time.deltaTime;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && keepCounting)
        {
            keepCounting = false;
            if (gameObject.CompareTag("Coin"))
            {
                player.addCoin(coinAmount);
            }
            else
            {
                player.addSCoin(coinAmount);
            }
            AudioManager.instance.PlaySFX(pickupSound);
            CollectAnim();
        }
    }

    public void CollectAnim()
    {
        shadow.SetActive(false);
        effect.Play();
        Sequence collectSeq = DOTween.Sequence();
        collectSeq.Append(transform.DOMove(transform.position + new Vector3(0, 1, 0), 1));
        collectSeq.Insert(0, transform.DOScale(new Vector3(.05f, .05f, .05f), 1));
        collectSeq.OnComplete(ReturnToPool);
    }

    private void ReturnToPool()
    {
        if (me == null)
        {
            me = new returnableObject(gameObject, null, null);
        }
        ObjectPooling.instance.addToPool(poolTag, me);
    }
}
