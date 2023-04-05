using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectScriptableObj PlateKitchenObjectScriptableObj;

    private float _spawnPlateTimer;
    private float _spawnTimerMax = 4f;
    private int _platesSpawnAmount;
    private int _platesSpawnAmountMax = 4;


    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnTimerMax)
        {
            _spawnPlateTimer = 0f;
          
            if (GameManager.Instance.IsGamePlaying() && _platesSpawnAmount < _platesSpawnAmountMax)
            {
                _platesSpawnAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {//Player is empty handed
            if (_platesSpawnAmount > 0)
            {//There is at least one plate here
                _platesSpawnAmount--;
                KitchenObject.SpawnKitchenObject(PlateKitchenObjectScriptableObj, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

            }
        }
    }
}
