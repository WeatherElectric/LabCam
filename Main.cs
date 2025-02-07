using Il2CppHorizonBasedAmbientOcclusion.Universal;
using Il2CppSLZ.Bonelab.SaveData;
using WeatherElectric.LabCam.Melon;
using WeatherElectric.LabCam.Resources;

namespace WeatherElectric.LabCam;

public class Main : MelonMod
{
    internal const string Name = "LabCam";
    internal const string Description = "Take pictures with a physical camera that save to a file!";
    internal const string Author = "Mabel Amber";
    internal const string Company = "Weather Electric";
    internal const string Version = "1.1.0";
    internal const string DownloadLink = "https://bonelab.thunderstore.io/package/SoulWithMae/LabCam/";
    
    internal static Assembly CurrAsm => Assembly.GetExecutingAssembly();

    internal static string CurrentMap;
    internal static bool FusionInstalled;
    internal static readonly List<HBAO> Hbao = [];

    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        Menu.BoneMenu.Setup();
        UserData.Setup();
        Assets.Load();
        Hooking.OnLevelLoaded += OnLevelLoad;
#if DEBUG
        ModConsole.Msg("This is a debug build! Possibly unstable!");
#endif
    }

    public override void OnLateInitializeMelon()
    {
        FusionInstalled = HelperMethods.CheckIfAssemblyLoaded("LabFusion");
    }

    private static void OnLevelLoad(LevelInfo levelInfo)
    {
        CurrentMap = levelInfo.title.Replace(" ", "");
        Hbao.AddRange(Object.FindObjectsOfType<HBAO>());
        foreach (var hbao in Hbao)
        {
            ModConsole.Msg($"HBAO: {hbao.name}", 1);
        }
    }
}
