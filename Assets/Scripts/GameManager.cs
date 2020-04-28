﻿using System.Collections;
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

    [Header("Enemies Pool")]
    public ObjectPool EnemyPool;

    [Header("Player Audio Pools")]
    public ObjectPool AudioLightAttackPool;

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
