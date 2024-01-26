namespace LabCam.Menu;

internal static class BoneMenu
{
    public static void Setup()
    {
        MenuCategory mainCat = MenuManager.CreateCategory("Weather Electric", "#6FBDFF");
        MenuCategory subCat = mainCat.CreateCategory("LabCam", "#DF71DE");
        subCat.CreateEnumElement("Quality", Color.white, Preferences.Quality.Value, v =>
        {
            Preferences.Quality.Value = v;
            Preferences.OwnCategory.SaveToFile(false);
            LabCamera.Instance.SetQuality();
        });
        subCat.CreateFunctionElement("Spawn Camera", Color.green, SpawnCam);
        subCat.CreateFunctionElement("Despawn Camera", Color.red, DespawnCam);
        
        subCat.CreateFunctionElement("Spawn Trigger", Color.green, SpawnTrigger);
        subCat.CreateFunctionElement("Despawn Trigger", Color.red, DespawnTrigger);
    }

    private static void SpawnCam()
    {
        if (LabCamera.Instance != null) return;
        Object.Instantiate(Assets.CameraPrefab);
    }

    private static void DespawnCam()
    {
        if (LabCamera.Instance == null) return;
        Object.Destroy(LabCamera.Instance.gameObject);
    }
    
    private static void SpawnTrigger()
    {
        if (LabCamera.Instance != null) return;
        Object.Instantiate(Assets.TriggerPrefab);
    }
    
    private static void DespawnTrigger()
    {
        if (LabCamera.Instance == null) return;
        Object.Destroy(Quagmire.Instance.gameObject);
    }
}