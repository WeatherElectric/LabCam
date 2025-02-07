namespace WeatherElectric.LabCam.Melon;

internal static class UserData
{
    private static readonly string WeatherElectricPath = Path.Combine(MelonEnvironment.UserDataDirectory, "Weather Electric");

    public static readonly string ModPath = Path.Combine(MelonEnvironment.UserDataDirectory, "Weather Electric/LabCam");

    public static void Setup()
    {
        if (!Directory.Exists(WeatherElectricPath))
        {
            Directory.CreateDirectory(WeatherElectricPath);
        }

        if (!Directory.Exists(ModPath))
        {
            Directory.CreateDirectory(ModPath);
        }
    }
}