namespace LabCam;

public class Main : MelonMod
{
    internal const string Name = "LabCam";
    internal const string Description = "Take pictures with a physical camera that save to a file!";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "0.0.1";
    internal const string DownloadLink = null;
    
    internal static Assembly CurrAsm => Assembly.GetExecutingAssembly();

    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        BoneMenu.Setup();
        UserData.Setup();
        Assets.Load();
#if DEBUG
        ModConsole.Msg("This is a debug build! Possibly unstable!");
#endif
    }
}