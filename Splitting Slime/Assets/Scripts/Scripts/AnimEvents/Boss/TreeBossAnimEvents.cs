using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class TreeBossAnimEvents : MonoBehaviour
{
    public Animator lazerAnim;
    public TreeBossMoveController moveController;
    public Vector3 rotation;
    public Transform projectileSpawn;
    [SerializeField]
    private GameObject sword, portal, baseObj;

    private List<GameObject> projectiles = new List<GameObject>();
    void Start()
    {
        moveController = transform.root.GetComponent<TreeBossMoveController>();
        baseObj = transform.parent.gameObject;
    }

    public void startLazer()
    {
        lazerAnim.SetTrigger("LazerActive");
    }

    public void LazerReset()
    {
        moveController.LazerActive = false;
    }

    public void SwingReset()
    {
        moveController.resetSwing();
    }

    public void ProjectileShot()
    {
        GameObject g = ObjectPooling.instance.SpawnFromPool("ProjectileShot", projectileSpawn.position, Quaternion.identity, false).gameObject;
        if (projectiles.Count < 20)
            projectiles.Add(g);
    }

    public void endProjectileShot()
    {
        moveController.endProjectileShot();
    }

    public void DisableSwordAndProjectiles()
    {
        sword.SetActive(false);
        for(int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].SetActive(false);
        }
    }

    public void EnablePortal()
    {
        baseObj.SetActive(false);
        portal.SetActive(true);
        moveController.EndFight();
    }

    public void AddProjectile(GameObject g)
    {
        projectiles.Add(g);
    }
}
