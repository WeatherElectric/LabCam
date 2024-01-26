namespace LabCam.Menu;

internal static class BoneMenu
{
    public static void Setup()
    {
        MenuCategory mainCat = MenuManager.CreateCategory("Weather Electric", "#6FBDFF");
        MenuCategory subCat = mainCat.CreateCategory("LabCam", "#DF71DE");
        subCat.CreateFunctionElement("Spawn", Color.green, Spawn);
        subCat.CreateFunctionElement("Despawn", Color.red, Despawn);
    }

    private static void Spawn()
    {
        if (LabCamera.Instance != null) return;
        Object.Instantiate(Assets.CameraPrefab);
    }

    private static void Despawn()
    {
        if (LabCamera.Instance == null) return;
        Object.Destroy(LabCamera.Instance.gameObject);
    }
}