using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Donut Settings")]
    [SerializeField] private GameObject donutPrefab;
    [SerializeField] private Transform donutSpawnPoint;
    [SerializeField] private TMP_Text donutCountText;
    [SerializeField] private float donutLifetime = 3f;

    [Header("Ice Cream Settings")]
    [SerializeField] private TMP_Text iceCreamCountText;
    [SerializeField] private float iceCreamBaseRate = 5f;
    private float iceCreamCount;

    [Header("Oompa Loompa Settings")]
    [SerializeField] private TMP_Text oompaCountText;
    [SerializeField] private TMP_Text oompaCostText;
    [SerializeField] private int oompaLoompaCount = 1;
    [SerializeField] private int oompaLoompaPrice = 3;
    [SerializeField] private float oompaLoompaPriceMultiplier = 1.1f;
    [SerializeField] private float baseProductionRate = 1f;

    [Header("Power-ups")]
    [SerializeField] private float rateMultiplier = 1f;

    private float donutCount;

    public int DonutCountInt => Mathf.FloorToInt(donutCount);
    public int IceCreamCountInt => Mathf.FloorToInt(iceCreamCount);
    public int OompaLoompaCount => oompaLoompaCount;
    public int OompaLoompaPrice => oompaLoompaPrice;
    public float RateMultiplier => rateMultiplier;

    private void Awake()
    {
        if (oompaLoompaCount <= 0)
        {
            oompaLoompaCount = 1;
        }
    }

    private void Update()
    {
        ProduceDonutsFromOompaLoompas();
        ProduceIceCream();
        UpdateUI();
    }

    private void ProduceDonutsFromOompaLoompas()
    {
        if (oompaLoompaCount <= 0) return;

        // Euler integration: value += rate * deltaTime
        float rate = oompaLoompaCount * baseProductionRate * rateMultiplier;
        donutCount += rate * Time.deltaTime;
    }

    private void ProduceIceCream()
    {
        // Euler integration: value += rate * deltaTime
        iceCreamCount += iceCreamBaseRate * Time.deltaTime;
    }

    private void UpdateUI()
    {
        if (donutCountText != null)
        {
            donutCountText.text = Mathf.FloorToInt(donutCount).ToString();
        }

        if (iceCreamCountText != null)
        {
            iceCreamCountText.text = Mathf.FloorToInt(iceCreamCount).ToString();
        }

        if (oompaCountText != null)
        {
            oompaCountText.text = oompaLoompaCount.ToString();
        }

        if (oompaCostText != null)
        {
            oompaCostText.text = oompaLoompaPrice.ToString();
        }
    }

    public void SpawnDonut()
    {
        if (donutPrefab == null)
        {
            Debug.LogWarning("Donut prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = donutSpawnPoint != null ? donutSpawnPoint.position : transform.position;
        Quaternion spawnRotation = donutSpawnPoint != null ? donutSpawnPoint.rotation : Quaternion.identity;

        GameObject donut = Instantiate(donutPrefab, spawnPosition, spawnRotation);
        Destroy(donut, donutLifetime);

        donutCount += 1;
    }

    public void BuyOompaLoompa()
    {
        if (Mathf.FloorToInt(donutCount) < oompaLoompaPrice) return;

        donutCount -= oompaLoompaPrice;
        oompaLoompaCount += 1;
        oompaLoompaPrice = Mathf.CeilToInt(oompaLoompaPrice * oompaLoompaPriceMultiplier);
    }

    public void ApplyRateMultiplier(float multiplier)
    {
        rateMultiplier *= multiplier;
    }

    public bool TrySpend(int amount)
    {
        if (Mathf.FloorToInt(donutCount) < amount) return false;
        donutCount -= amount;
        return true;
    }
}