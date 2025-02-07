namespace WeatherElectric.LabCam.Resources;

internal static class Assets
{
    private static AssetBundle _assetBundle;
    
    public static void Load()
    {
        _assetBundle = HelperMethods.LoadEmbeddedAssetBundle(Main.CurrAsm, HelperMethods.IsAndroid() ? "WeatherElectric.LabCam.Resources.LabCamAndroid.bundle" : "WeatherElectric.LabCam.Resources.LabCamWindows.bundle");
        ModConsole.Msg($"AssetBundle: {_assetBundle.name}", 1);
        if (_assetBundle == null) return;
        RenderTextures.LoadRenderTextures();
        Materials.LoadMaterials();
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
            
            ModConsole.Msg($"RenderTextures: {HighQuality.name}, {MediumQuality.name}, {LowQuality.name}", 1);
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
            if (HighQuality == null) HighQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/HighQualityPreview.mat");
            if (MediumQuality == null) MediumQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/MediumQualityPreview.mat");
            if (LowQuality == null) LowQuality = _assetBundle.LoadPersistentAsset<Material>("Assets/LabCam/LowQualityPreview.mat");
            
            ModConsole.Msg($"Materials: {HighQuality.name}, {MediumQuality.name}, {LowQuality.name}", 1);
        }
    }
}