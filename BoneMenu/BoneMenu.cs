using LabFusion.Network;

namespace WeatherElectric.LabCam.Menu;

internal static class BoneMenu
{
    public static void Setup()
    {
        var mainCat = Page.Root.CreatePage("<color=#6FBDFF>Weather Electric</color>", Color.white);
        var subCat = mainCat.CreatePage("<color=#ffad2d>LabCam</color>", Color.white);
        subCat.CreateEnum("Quality", Color.white, Preferences.Quality.Value, v =>
        {
            Preferences.Quality.Value = (ImageQuality)v;
            Preferences.OwnCategory.SaveToFile(false);
            if (Main.FusionInstalled)
            {
                if (NetworkInfo.HasServer && NetworkInfo.IsClient) return;
            }
            if (LabCamera.Instance != null) LabCamera.Instance.SetQuality();
            if (Quagmire.Instance != null) Quagmire.Instance.SetQuality();
        });
    }
}