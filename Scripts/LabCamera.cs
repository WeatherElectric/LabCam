namespace LabCam.Scripts;

[RegisterTypeInIl2Cpp]
public class LabCamera : MonoBehaviour
{
    public static LabCamera Instance;

    private Camera _camera;
    private AudioSource _captureSound;
    private MeshRenderer _previewRenderer;
    private GameObject _flashRenderer;
    private bool _flash;
    private GameObject _light;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        var impactSfx = GetComponent<ImpactSFX>();
        if (impactSfx != null) impactSfx.outputMixer = Audio.SFXMixer;
        _captureSound = transform.Find("CaptureSound").GetComponent<AudioSource>();
        // WOOOO FUCK YOU SDK MODDERS I CAN DO THIS!
        _captureSound.outputAudioMixerGroup = Audio.SFXMixer;
        _previewRenderer = transform.Find("LensPreview").GetComponent<MeshRenderer>();
        _flashRenderer = transform.Find("FlashWarning").gameObject;
        _light = transform.Find("Flash").gameObject;
        SetQuality();
    }

    public void SetQuality()
    {
        switch (Preferences.Quality.Value)
        {
            case ImageQuality.Low:
                _camera.targetTexture = Assets.LowQualityRt;
                _previewRenderer.material = Assets.LowQualityMat;
                break;
            case ImageQuality.Medium:
                _camera.targetTexture = Assets.MediumQualityRt;
                _previewRenderer.material = Assets.MediumQualityMat;
                break;
            case ImageQuality.High:
                _camera.targetTexture = Assets.HighQualityRt;
                _previewRenderer.material = Assets.HighQualityMat;
                break;
            default:
                ModConsole.Error("Invalid quality setting!");
                break;
        }
    }
    
    public void ToggleFlash()
    {
        _flash = !_flash;
        _flashRenderer.SetActive(_flash);
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
        HideQuagmire();
        if (_flash) _light.SetActive(true);
        RenderTexture rt = _camera.targetTexture;
        Texture2D screenShot = new Texture2D(rt.width, rt.height);
        _camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        string path;
        // ReSharper disable once InvokeAsExtensionMethod, unhollowed extension methods are cursed, they really need an excorcism
        byte[] bytes;
        if (Preferences.Quality.Value == ImageQuality.Low)
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.jpg");
            bytes = ImageConversion.EncodeToJPG(screenShot);
        }
        else
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
            bytes = ImageConversion.EncodeToPNG(screenShot);
        }
        Destroy(screenShot);
        File.WriteAllBytes(path, bytes);
        HideHead();
        ShowQuagmire();
        if (_flash) Invoke(nameof(LightDisable), 0.3f);
    }

    private void LightDisable()
    {
        _light.SetActive(false);
    }
    
    private static void HideHead()
    {
        var hairMeshes = Player.rigManager.avatar.hairMeshes;
        if (hairMeshes == null) return;
        foreach (var hairMesh in hairMeshes)
        {
            hairMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
    
    private static void ShowHead()
    {
        var hairMeshes = Player.rigManager.avatar.hairMeshes;
        if (hairMeshes == null) return;
        foreach (var hairMesh in hairMeshes)
        {
            hairMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        }
    }
    
    private static void HideQuagmire()
    {
        if (Quagmire.Instance == null) return;
        Quagmire.Instance.giggity.SetActive(false);
        Quagmire.Instance.giggityPreview.SetActive(false);
    }
    
    private static void ShowQuagmire()
    {
        if (Quagmire.Instance == null) return;
        Quagmire.Instance.giggity.SetActive(true);
        Quagmire.Instance.giggityPreview.SetActive(true);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public LabCamera(IntPtr ptr) : base(ptr) { }
}