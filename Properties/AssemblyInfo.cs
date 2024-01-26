[assembly: AssemblyTitle(LabCam.Main.Description)]
[assembly: AssemblyDescription(LabCam.Main.Description)]
[assembly: AssemblyCompany(LabCam.Main.Company)]
[assembly: AssemblyProduct(LabCam.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + LabCam.Main.Author)]
[assembly: AssemblyTrademark(LabCam.Main.Company)]
[assembly: AssemblyVersion(LabCam.Main.Version)]
[assembly: AssemblyFileVersion(LabCam.Main.Version)]
[assembly:
    MelonInfo(typeof(LabCam.Main), LabCam.Main.Name, LabCam.Main.Version,
        LabCam.Main.Author, LabCam.Main.DownloadLink)]
[assembly: MelonColor(ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]