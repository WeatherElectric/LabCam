namespace LabCam.Resources;

internal static class Assets
{
    private static AssetBundle _assetBundle;
    
    public static void Load()
    {
        _assetBundle = HelperMethods.LoadEmbeddedAssetBundle(Main.CurrAsm, HelperMethods.IsAndroid() ? "LabCam.Resources.Android.bundle" : "LabCam.Resources.Windows.bundle");
        if (_assetBundle == null) return;
        Prefabs.LoadPrefabs();
        RenderTextures.LoadRenderTextures();
        Materials.LoadMaterials();
    }
    
    internal static class Prefabs
    {
        public static GameObject CameraPrefab;
        public static GameObject TriggerPrefab;
        
        internal static void LoadPrefabs()
        {
            if (_assetBundle == null) return;
            if (CameraPrefab == null) CameraPrefab = _assetBundle.LoadPersistentAsset<GameObject>("Assets/LabCam/LabCam.prefab");
            if (TriggerPrefab == null) TriggerPrefab = _assetBundle.LoadPersistentAsset<GameObject>("Assets/LabCam/RemoteTrigger.prefab");
        }
    }
    
    internal static class RenderTextures
    {
        public static RenderTexture HighQuality;
        public static RenderTexture MediumQuality;
        public static RenderTexture LowQuality;
        
        internal static void LoadRenderTextures()
        {
            if (_assetBundle == null) return;
            if (HighQuality == null) HighQuality = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/HighQuality.renderTexture");
            if (MediumQuality == null) MediumQuality = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/MediumQuality.renderTexture");
            if (LowQuality == null) LowQuality = _assetBundle.LoadPersistentAsset<RenderTexture>("Assets/LabCam/LowQuality.renderTexture");
        }
    }
    
    internal static class Materials
    {
        public static Material HighQuality;
        public static Material MediumQuality;
        public static Material LowQuality;
        
        internal static void LoadMaterials()
        {
            if (_assetBundle == null) return;
            if (HighQuality == null) HighQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/HighQualityPreview.mat");
            if (MediumQuality == null) MediumQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/MediumQualityPreview.mat");
            if (LowQuality == null) LowQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/Materials/LowQualityPreview.mat");
        }
    }
}