namespace LabCam;

public class Main : MelonMod
{
    internal const string Name = "LabCam";
    internal const string Description = "Take pictures with a physical camera that save to a file!";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "1.0.0";
    internal const string DownloadLink = "https://bonelab.thunderstore.io/package/SoulWithMae/LabCam/";
    
    internal static Assembly CurrAsm => Assembly.GetExecutingAssembly();

    internal static string CurrentMap;

    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        BoneMenu.Setup();
        UserData.Setup();
        Assets.Load();
        Hooking.OnLevelInitialized += OnLevelLoad;
#if DEBUG
        ModConsole.Msg("This is a debug build! Possibly unstable!");
#endif
    }

    private static void OnLevelLoad(LevelInfo levelInfo)
    {
        CurrentMap = levelInfo.title.Replace(" ", "");
    }
}
