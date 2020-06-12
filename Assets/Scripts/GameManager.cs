using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Combo Particle pools")]
    public ObjectPool FireComboPool;
    public ObjectPool WaterComboPool;
    public ObjectPool EarthComboPool;
    public ObjectPool MetalComboPool;
    public ObjectPool WoodComboPool;

    [Header("Spell Particle pools")]
    public ObjectPool FireSpellPool;
    public ObjectPool WaterSpellPool;
    public ObjectPool EarthSpellPool;
    public ObjectPool MetalSpellPool;
    public ObjectPool WoodSpellPool;

    [Header("Coin Pool")]
    public ObjectPool CoinPool;

    [Header("Particle Pools")]
    public ObjectPool hitMarkerPool;

    [Header("Enemies Pool")]
    public ObjectPool EnemyPool;

    [Header("Player Audio Pools")]
    public ObjectPool AudioLightAttackPool;
    public ObjectPool AudioLightAttack2Pool;
    public ObjectPool AudioLightAttack3Pool;
    public ObjectPool AudioHeavyAttack1Pool;
    public ObjectPool AudioHeavyAttack2Pool;
    public ObjectPool AudioHeavyAttack3Pool;
    public ObjectPool AudioStepPool;
    public ObjectPool AudioStep2Pool;
    public ObjectPool AudioDashPool;
    public ObjectPool AudioHurtPool;
    public ObjectPool AudioHurt2Pool;
    public ObjectPool AudioDeathPool;
    public ObjectPool AudioSlashPool;
    public ObjectPool AudioSpecialPool;

    [Header("Enemy Audio Pools")]
    public ObjectPool AudioEnemyAttackPool;
    public ObjectPool AudioEnemyHurtPool;
    public ObjectPool AudioEnemyDeathPool;
    public ObjectPool AudioEnemyStep1Pool;
    public ObjectPool AudioEnemyStep2Pool;


    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
