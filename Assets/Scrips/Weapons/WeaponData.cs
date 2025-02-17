
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
  
    public int from;
    public int to;
    public int max;
    public float spawnCooldown;
    public int amount;
}

[System.Serializable]
public class NPCSpawnInfo
{
    public Object npc;
}

[System.Serializable]
public class WaveSetting
{

    public int amount;
}

[System.Serializable]
public class TowerSpawnInfo
{
    public List<WaveSetting> wave;
}

[CreateAssetMenu(menuName = "New Game Settings")]
public class WeaponData : ScriptableObject
{
    [Header("Enemy Settings")]
    [Tooltip("All enemy Settings")]
    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
    [Space(10)]
    [Header("NPC Settings")]
    [Tooltip("Include all NPCs that are ready for the game")]
    public List<Object> npcs = new List<Object>();

    [Header("Tower Settings")]
    [Tooltip("Include all Tower-Tiers that are ready for the game")]
    public List<TowerSpawnInfo> towers = new List<TowerSpawnInfo>();
    [Space(10)]
    [Header("Map Settings")]
    [Tooltip("The width of the Map")]
    public int mapSizeX = 20;
    [Tooltip("The height of the Map")]
    public int mapSizeY = 20;
    [Tooltip("Radius in which Fogtiles get cleared and nearby Tower spawn")]
    public int towerClearRadius = 3;
    [Tooltip("The spawn-cap of loot Tiles per tower fog clearing\nNote: Extra conditions are not included in this number")]
    public int lootCap = 2;
    [Tooltip("The spawn-cap of npc Tiles per tower fog clearing\nNote: Extra conditions are not included in this number")]
    public int npcCap;

    [Header("General Settings")]
    public string startingSpell = "Soularrow";
    [Tooltip("Base-Velocity inwhich the player moves")]
    public float basePlayerSpeed = 10;
    public float basePlayerHealth = 50;
    public float basePlayerDamage = 0;
    public int abilityCap = 2;
    public float baseHPRegen = 0.5f;
    public int baseArtifactCount = 3;
    public int GameOverOnSecond = 900;
    public float baseEXPMultiplier = 5;
    public float baseDivisor = 2;
    public Color lowExpColor = new Color(0, 1, 0.9528818f, 1);
    public Color highExpColor = new Color(1, 0, 0, 1);
    public float highExp = 140;
    public Color lowDamageColor = new Color(1, 1, 1, 1);
    public float highDamage = 50;
    public Color highDamageColor = new Color(1, 0, 0.213181f, 0);
    public Object damagePrefabSettings;


    [Header("Veil Settings")]
    public Color veilColor = new Color(0.65f, 0.65f, 0.22f, 1);
    public float VeilMultiplier = 2;

}