using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerWeaponData
{
    public string name;
    public Sprite weaponIcon;
    public bool fullAuto;
    public float firerate;
    public float damage;
    public float relodeTime;
    public int maxBullets;
    public Vector3 bulletSpread;
    public bool isScopable;
    public GameObject weaponObject;
    
}

[CreateAssetMenu(menuName = "New Game Settings")]
public class WeaponData : ScriptableObject
{
    public List<PlayerWeaponData> Weapons = new List<PlayerWeaponData>();
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTrail;
    public LayerMask mask;
}