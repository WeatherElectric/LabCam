namespace LabCam.Scripts;

public class LabCamera : MonoBehaviour
{
    public static LabCamera Instance;

    private Camera _camera;
    private AudioSource _captureSound;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _captureSound = transform.Find("CaptureSound").GetComponent<AudioSource>();
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
}