namespace LabCam.Scripts;

// giggity
[RegisterTypeInIl2Cpp]
public class Quagmire : MonoBehaviour
{
    public static Quagmire Instance;

    public GameObject giggity;
    public GameObject giggityPreview;
    public MeshRenderer giggityRenderer;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        giggity = transform.Find("Scale").gameObject;
        giggityPreview = transform.Find("Preview").gameObject;
        giggityRenderer = giggityPreview.GetComponent<MeshRenderer>();
    }

    public void SetQuality()
    {
        switch (Preferences.Quality.Value)
        {
            case ImageQuality.Low:
                giggityRenderer.material = Assets.LowQualityMat;
                break;
            case ImageQuality.Medium:
                giggityRenderer.material = Assets.MediumQualityMat;
                break;
            case ImageQuality.High:
                giggityRenderer.material = Assets.HighQualityMat;
                break;
            default:
                ModConsole.Error("Invalid quality setting!");
                break;
        }
    }
    
    public void SendCapture()
    {
        if (LabCamera.Instance == null) return;
        LabCamera.Instance.Capture();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public Quagmire(IntPtr ptr) : base(ptr) { }
}