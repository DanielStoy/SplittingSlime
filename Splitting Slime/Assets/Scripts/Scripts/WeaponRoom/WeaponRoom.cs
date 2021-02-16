using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoom : MonoBehaviour
{

    public List<GameObject> weaponHolders = new List<GameObject>();
    private GameObject player;
    private PlayerController playerCon;
    public List<Weapon> unlockedWeapons = new List<Weapon>();
    public List<GameObject> weaponSlots = new List<GameObject>();
    public bool playerIn = false;
    public int playerSlot = -1;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.GetComponent<CameraBehavior>().UnlockZ = true;
        player = PlayerManager.instance.Player;
        unlockedWeapons = GM._instance.savedPlayerData.unlockedWeapons;
        playerCon = player.GetComponent<PlayerController>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Walls"))
            {

            }
        }
        getChildren();
        itterateWeapons();
    }

    void getChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Walls"))
            {
               for(int j = 0; j< transform.GetChild(i).childCount; j++)
               {
                    GameObject weaponSlot = transform.GetChild(i).GetChild(j).GetChild(0).gameObject;
                    weaponSlot.GetComponent<WeaponSlot>().slotNum = j;
                    weaponSlots.Add(transform.GetChild(i).GetChild(j).GetChild(0).gameObject);
               }
            }
        }
    }

    void itterateWeapons()
    {
        
        for(int i = 0; i < unlockedWeapons.Count; i++)
        {
            GameObject weaponSlot = weaponSlots[unlockedWeapons[i].spawnArea];
            weaponSlot.GetComponent<WeaponSlot>().isUnlocked = true;
            Instantiate(unlockedWeapons[i].model, weaponSlots[unlockedWeapons[i].spawnArea].transform.position, Quaternion.identity);
        }
    }

    public void Update()
    {
        if (playerIn)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                playerCon.EquipWeapon(unlockedWeapons[playerSlot]);
            }
        }
    }

    public void activateFLoor(int floorNum)
    {
        transform.GetChild(1).GetChild(floorNum).gameObject.SetActive(true);
    }

}
