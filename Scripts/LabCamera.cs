namespace LabCam.Scripts;

[RegisterTypeInIl2Cpp]
public class LabCamera : MonoBehaviour
{
    public static LabCamera Instance;

    private Camera _camera;
    private AudioSource _captureSound;
    private MeshRenderer _previewRenderer;
    private MeshRenderer _previewRendererUI;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        var impactSFX = GetComponent<ImpactSFX>();
        if (impactSFX != null) impactSFX.outputMixer = Audio.SFXMixer;
        _captureSound = transform.Find("CaptureSound").GetComponent<AudioSource>();
        // WOOOO FUCK YOU SDK MODDERS I CAN DO THIS!
        _captureSound.outputAudioMixerGroup = Audio.SFXMixer;
        _previewRenderer = transform.Find("LensPreview").GetComponent<MeshRenderer>();
        _previewRendererUI = transform.Find("WindowPreview").GetComponent<MeshRenderer>();
        SetQuality();
    }

    public void SetQuality()
    {
        switch (Preferences.Quality.Value)
        {
            case ImageQuality.Low:
                _camera.targetTexture = Assets.LowQualityRt;
                _previewRendererUI.material = Assets.LowQualityMatUI;
                _previewRenderer.material = Assets.LowQualityMat;
                break;
            case ImageQuality.Medium:
                _camera.targetTexture = Assets.MediumQualityRt;
                _previewRendererUI.material = Assets.MediumQualityMatUI;
                _previewRenderer.material = Assets.MediumQualityMat;
                break;
            case ImageQuality.High:
                _camera.targetTexture = Assets.HighQualityRt;
                _previewRendererUI.material = Assets.HighQualityMatUI;
                _previewRenderer.material = Assets.HighQualityMat;
                break;
            default:
                ModConsole.Error("Invalid quality setting!");
                break;
        }
    }

    public void Capture()
    {
        if (_camera == null || _camera.targetTexture == null)
        {
            ModConsole.Error("Camera, render texture, or both are/is null!");
            return;
        }
        
        _captureSound.Play();
        ShowHead();
        RenderTexture rt = _camera.targetTexture;
        Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        _camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        
        var path = Path.Combine(UserData.ModPath, $"BONELAB_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
        // ReSharper disable once InvokeAsExtensionMethod, unhollowed extension methods are cursed, they really need an excorcism
        byte[] bytes = ImageConversion.EncodeToPNG(screenShot);
        File.WriteAllBytes(path, bytes);
        HideHead();
    }

    private static void HideHead()
    {
        var hairMeshes = Player.rigManager.avatar.hairMeshes;
        if (hairMeshes == null) return;
        foreach (var hairMesh in hairMeshes)
        {
            hairMesh.gameObject.SetActive(false);
        }
    }
    
    private static void ShowHead()
    {
        var hairMeshes = Player.rigManager.avatar.hairMeshes;
        if (hairMeshes == null) return;
        foreach (var hairMesh in hairMeshes)
        {
            hairMesh.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public LabCamera(IntPtr ptr) : base(ptr) { }
}