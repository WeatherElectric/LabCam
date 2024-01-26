// ReSharper disable InvokeAsExtensionMethod, unhollowed extension methods are cursed.

using System.Threading.Tasks;

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
        AssignFields();
        SetQuality();
        SetMixers();
    }

    private void SetMixers()
    {
        var impactSfx = GetComponent<ImpactSFX>();
        if (impactSfx != null) impactSfx.outputMixer = Audio.SFXMixer;
        if (_captureSound != null) _captureSound.outputAudioMixerGroup = Audio.SFXMixer;
    }

    // Temporary until we move on to ML 1.0, since we don't have fieldinjection for IL2CPP without adding another dependency, and I don't want to do that for a mod like this.
    private void AssignFields()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _captureSound = transform.Find("CaptureSound").GetComponent<AudioSource>();
        _previewRenderer = transform.Find("LensPreview").GetComponent<MeshRenderer>();
        _flashRenderer = transform.Find("FlashWarning").gameObject;
        _light = transform.Find("Flash").gameObject;
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

    public void Capture()
    {
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
    }

    private static void SaveRenderedImage(Texture2D image)
    {
        string path;
        byte[] bytes;
        
        if (Preferences.Quality.Value == ImageQuality.Low)
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{Main.CurrentMap}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.jpg");
            // Don't need PNG for a 480p image.
            bytes = ImageConversion.EncodeToJPG(image);
        }
        else
        {
            path = Path.Combine(UserData.ModPath, $"BONELAB_{Main.CurrentMap}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
            bytes = ImageConversion.EncodeToPNG(image);
        }

        Task.Run(() =>
        {
            File.WriteAllBytes(path, bytes);
        }).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                ModConsole.Error($"Failed to save picture to {path}!");
                ModConsole.Error(task.Exception?.ToString());
            }
            else
            {
                ModConsole.Msg($"Saved picture to {path}");
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
        // Destroy the Texture2D since it's not needed, the PNG/JPG is saved, save some memory by deleting the Texture2D.
        Destroy(image);
    }

    private Texture2D Render()
    {
        RenderTexture rt = _camera.targetTexture;
        Texture2D image = new Texture2D(rt.width, rt.height);
        _camera.Render();
        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        image.Apply();
        return image;
    }

    // Avoids needing a coroutine if I can just call it delayed with an Invoke, I'd like to avoid needing Jevilib's coroutine fix for this mod.
    private void LightDisable()
    {
        _light.SetActive(false);
    }
    
    private static void SetHairMeshes(bool state)
    {
        var hairMeshes = Player.rigManager.avatar.hairMeshes;
        foreach (var hairMesh in hairMeshes)
        {
            if (hairMesh == null) return;
            hairMesh.shadowCastingMode = state switch
            {
                true => UnityEngine.Rendering.ShadowCastingMode.TwoSided,
                false => UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly
            };
        }
    }
    
    // Do not question my naming schemes, they are perfect.
    private static void SetQuagmire(bool state)
    {
        if (Quagmire.Instance == null) return;
        Quagmire.Instance.giggity.SetActive(state);
        Quagmire.Instance.giggityPreview.SetActive(state);
    }
    
    private void OnDestroy()
    {
        Instance = null;
    }
    
    public LabCamera(IntPtr ptr) : base(ptr) { }
}