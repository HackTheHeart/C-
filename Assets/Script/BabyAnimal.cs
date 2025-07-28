using UnityEngine;

public class BabyAnimal : Animal
{
    private float growthProgress = 0f;
    private float growthTime;
    private string animalName;
    protected override void Start()
    {
        base.Start();
        animalName = gameObject.name.Replace("(Clone)", "").Trim();
        growthTime = AnimalManager.Instance.GetGrowthTime(animalName);
        //GameTimeManager.Instance.OnNewDayStarted += Grow;
    }
    public new AnimalData GetAnimalData()
    {
        var baseData = base.GetAnimalData();
        baseData.isBaby = true;
        baseData.growthProgress = growthProgress;
        return baseData;
    }
    private void Grow()
    {
        growthProgress++;
        if (growthProgress >= growthTime)
        {
            TransformToAdult();
        }
    }
    protected override void CheckForDailyProduction()
    {
        return;
    }
    public void TransformToAdult()
    {
        GameObject adultPrefab = AnimalManager.Instance.GetAdultVersion(animalName);
        if (adultPrefab != null)
        {
            Instantiate(adultPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameTimeManager.Instance.OnNewDayStarted -= Grow;
    }
}
