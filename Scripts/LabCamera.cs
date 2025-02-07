// ReSharper disable InvokeAsExtensionMethod, unhollowed extension methods are cursed.

using Il2CppHorizonBasedAmbientOcclusion.Universal;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Pool;
using LabFusion.Network;
using UnityEngine.Rendering;

namespace WeatherElectric.LabCam.Scripts;

[RegisterTypeInIl2Cpp]
public class LabCamera : MonoBehaviour
{
    internal static LabCamera Instance;

    private Camera _camera;
    private AudioSource _captureSound;
    private MeshRenderer _previewRenderer;
    private GameObject _flashRenderer;
    private bool _flash;
    private GameObject _light;

    private PlayerAvatarArt _avatarArt;
    
    private float _cooldownTimer;
    private const float Cooldown = 5f;
    private bool _cooldownUp;
    
    private bool _markedForDestruction;
    
    // Has to be OnEnable, BL's pooling works by just disabling objects, and I want this code to run again every time it's spawned.
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            _markedForDestruction = true;
            var pool = GetComponent<Poolee>();
            pool.Despawn();
            return;
        }
        
        Instance = this;
        _markedForDestruction = false;
        
        AssignFields();
        SetQuality();
        SetMixers();
    }

    private void SetMixers()
    {
        if (_captureSound != null) _captureSound.outputAudioMixerGroup = Audio.Gunshot;
    }

    // Temporary until we move on to ML 1.0, since we don't have fieldinjection for IL2CPP without adding another dependency, and I don't want to do that for a mod like this.
    // so ML 0.6 added fieldinjection, so I could avoid all this, right?
    // wrong! it's interop, made by bepinex, and it's SHIT! this works still i guess
    private void AssignFields()
    {
        _camera = transform.Find("Systems/Camera").GetComponent<Camera>();
        _captureSound = transform.Find("Audio/CaptureSound").GetComponent<AudioSource>();
        _previewRenderer = transform.Find("UI/CameraScreen").GetComponent<MeshRenderer>();
        _flashRenderer = transform.Find("UI/FlashWarning").gameObject;
        _light = transform.Find("Systems/Flash").gameObject;
    }
    
    // TODO: Maybe just modify the material instead of swapping it out? I'd need Scanline's shadercode though since the property to change isn't MainTex.
    public void SetQuality()
    {
        switch (Preferences.Quality.Value)
        {
            case ImageQuality.Low:
                _camera.targetTexture = Assets.RenderTextures.LowQuality;
                _previewRenderer.material = Assets.Materials.LowQuality;
                break;
            case ImageQuality.Medium:
                _camera.targetTexture = Assets.RenderTextures.MediumQuality;
                _previewRenderer.material = Assets.Materials.MediumQuality;
                break;
            case ImageQuality.High:
                _camera.targetTexture = Assets.RenderTextures.HighQuality;
                _previewRenderer.material = Assets.Materials.HighQuality;
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
    
    // i think spamming capture is why my GPU drivers crashed, so this should prevent that
    public void Update()
    {
        if (_cooldownUp) return;
        if (_cooldownTimer < Cooldown)
        {
            _cooldownUp = true;
            _cooldownTimer = 0f;
            return;
        }
        _cooldownTimer += Time.deltaTime;
    }

    public void Capture()
    {
        if (!_cooldownUp) return;

        if (Main.FusionInstalled)
        {
            if (NetworkInfo.HasServer && NetworkInfo.IsClient)
            {
                CaptureFusionClient();
                return;
            }
        }

        // HBAO fucks up the image, temporarily disable it then restore it to what it was before
        var dictionary = new Dictionary<HBAO, ClampedFloatParameter>();
        var zeroIntensity = new ClampedFloatParameter(0f, 0f, 1f, true);
        foreach (var hbao in Main.Hbao)
        {
            dictionary.Add(hbao, hbao.intensity);
            SetHbao(hbao, zeroIntensity);
        }
        
        if (_avatarArt == null) _avatarArt = Player.ControllerRig.GetComponent<PlayerAvatarArt>();
        if (_camera == null || _camera.targetTexture == null)
        {
            ModConsole.Error("Camera, render texture, or both are/is null!");
            return;
        }
        
        _captureSound.Play();
        SetHairMeshes(true);
        SetQuagmire(false);
        if (_flash) _light.SetActive(true);
        
        SaveRenderedImage(Render());
        
        SetHairMeshes(false);
        SetQuagmire(true);
        if (_flash) Invoke(nameof(LightDisable), 0.3f);
        
        foreach (var hbao in dictionary)
        {
            SetHbao(hbao.Key, hbao.Value);
        }
        
        _cooldownUp = false;
    }

    private void CaptureFusionClient()
    {
        _captureSound.Play();
        if (_flash) _light.SetActive(true);
        if (_flash) Invoke(nameof(LightDisable), 0.3f);
    }

    private static void SetHbao(HBAO hbao, ClampedFloatParameter intensity)
    {
        hbao.intensity = intensity;
    }

    private static void SaveRenderedImage(Texture2D image)
    {
        string path;
        byte[] bytes;
        
        if (Preferences.Quality.Value == ImageQuality.Low)
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{Main.CurrentMap}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.jpg");
            bytes = ImageConversion.EncodeToJPG(image);
        }
        else
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{Main.CurrentMap}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
            bytes = ImageConversion.EncodeToPNG(image);
        }

        File.WriteAllBytes(path, bytes);
        Destroy(image);
    }

    private Texture2D Render()
    {
        var rt = _camera.targetTexture;
        var image = new Texture2D(rt.width, rt.height, rt.graphicsFormat, rt.mipmapCount, TextureCreationFlags.None);
        _camera.Render();
        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        return image;
    }

    // Avoids needing a coroutine if I can just call it delayed with an Invoke, I'd like to avoid needing Jevilib's coroutine fix for this mod.
    private void LightDisable()
    {
        _light.SetActive(false);
    }
    
    private void SetHairMeshes(bool state)
    {
        if (_avatarArt == null) return;
        switch (state)
        {
            case true:
                _avatarArt.EnableHair();
                break;
            case false:
                _avatarArt.DisableHair();
                break;
        }
    }
    
    // Do not question my naming schemes, they are perfect.
    private static void SetQuagmire(bool state)
    {
        if (Quagmire.Instance == null) return;
        Quagmire.Instance.giggity.SetActive(state);
        Quagmire.Instance.giggityPreview.SetActive(state);
    }

    public void OnDisable()
    {
        if (_markedForDestruction) return;
        Instance = null;
    }
    
    // ReSharper disable once ConvertToPrimaryConstructor, no;
    public LabCamera(IntPtr ptr) : base(ptr) { }
}