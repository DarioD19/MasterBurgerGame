using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter PlatesCounter;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;

    private List<GameObject> _plateViusalGameObjectList;
    private void Awake()
    {
        _plateViusalGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        PlatesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        PlatesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = _plateViusalGameObjectList[_plateViusalGameObjectList.Count - 1];
        _plateViusalGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
       Transform plateViusalTransform = Instantiate(_plateVisualPrefab, _counterTopPoint);

        float plateOffsetY = 0.1f;
        plateViusalTransform.localPosition = new Vector3(0, plateOffsetY * _plateViusalGameObjectList.Count, 0);

        _plateViusalGameObjectList.Add(plateViusalTransform.gameObject);
    }
}
