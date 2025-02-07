[assembly: AssemblyTitle(WeatherElectric.LabCam.Main.Description)]
[assembly: AssemblyDescription(WeatherElectric.LabCam.Main.Description)]
[assembly: AssemblyCompany(WeatherElectric.LabCam.Main.Company)]
[assembly: AssemblyProduct(WeatherElectric.LabCam.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + WeatherElectric.LabCam.Main.Author)]
[assembly: AssemblyTrademark(WeatherElectric.LabCam.Main.Company)]
[assembly: AssemblyVersion(WeatherElectric.LabCam.Main.Version)]
[assembly: AssemblyFileVersion(WeatherElectric.LabCam.Main.Version)]
[assembly:
    MelonInfo(typeof(WeatherElectric.LabCam.Main), WeatherElectric.LabCam.Main.Name, WeatherElectric.LabCam.Main.Version,
        WeatherElectric.LabCam.Main.Author, WeatherElectric.LabCam.Main.DownloadLink)]
[assembly: MelonColor(255, 255, 173, 45)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
[assembly: MelonOptionalDependencies("LabFusion")]