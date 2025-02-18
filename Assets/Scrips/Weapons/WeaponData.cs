using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

[System.Serializable]
public class PlayerWeaponData
{
    public string name;
    public Sprite weaponIcon;
    public float firerate;
    public float damage;
    public float relodeTime;
    public int maxBullets;
    public float SprayPattern;
    public bool isScopable;
    public GameObject weaponObject;
}

[CreateAssetMenu(menuName = "New Game Settings")]
public class WeaponData : ScriptableObject
{
    [Header("Enemy Settings")]
    [Tooltip("All enemy Settings")]
    public List<PlayerWeaponData> Weapons = new List<PlayerWeaponData>();
}