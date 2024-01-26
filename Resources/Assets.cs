namespace LabCam.Resources;

internal static class Assets
{
    private static AssetBundle _assetBundle;
    
    public static GameObject CameraPrefab;
    public static GameObject TriggerPrefab;
    
    public static RenderTexture HighQualityRt;
    public static RenderTexture MediumQualityRt;
    public static RenderTexture LowQualityRt;

    public static Material HighQualityMat;
    public static Material MediumQualityMat;
    public static Material LowQualityMat;
    
    public static Material HighQualityMatUI;
    public static Material MediumQualityMatUI;
    public static Material LowQualityMatUI;
    
    public static void Load()
    {
        // can the quest kill itself
        _assetBundle = HelperMethods.LoadEmbeddedAssetBundle(Main.CurrAsm, HelperMethods.IsAndroid() ? "LabCam.Resources.Android.bundle" : "LabCam.Resources.Windows.bundle");
        CameraPrefab = _assetBundle.LoadPersistentAsset<GameObject>("Assets/LabCam/LabCam.prefab");
        TriggerPrefab = _assetBundle.LoadPersistentAsset<GameObject>("Assets/LabCam/RemoteTrigger.prefab");
        
        HighQualityRt = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/HighQuality.renderTexture");
        MediumQualityRt = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/MediumQuality.renderTexture");
        LowQualityRt = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/LowQuality.renderTexture");
        
        HighQualityMat = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/HighQualityPreview.mat");
        MediumQualityMat = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/MediumQualityPreview.mat");
        LowQualityMat = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/LowQualityPreview.mat");
        
        HighQualityMatUI = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/HighQualityPreview2.mat");
        MediumQualityMatUI = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/MediumQualityPreview2.mat");
        LowQualityMatUI = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/LowQualityPreview2.mat");
        
    }
}