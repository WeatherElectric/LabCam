namespace LabCam.Resources;

internal static class Assets
{
    private static AssetBundle _assetBundle;
    public static GameObject CameraPrefab;
    
    public static void Load()
    {
        _assetBundle = HelperMethods.LoadEmbeddedAssetBundle(Main.CurrAsm, HelperMethods.IsAndroid() ? "LabCam.Resources.Android.bundle" : "LabCam.Resources.Windows.bundle");
        CameraPrefab = _assetBundle.LoadPersistentAsset<GameObject>("Assets/LabCam/Camera.prefab");
    }
}