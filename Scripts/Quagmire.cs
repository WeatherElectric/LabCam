using Il2CppSLZ.Marrow.Pool;

namespace WeatherElectric.LabCam.Scripts;

// giggity
// i'm not renaming this class, i'm leaving it like this fuck you
[RegisterTypeInIl2Cpp]
public class Quagmire : MonoBehaviour
{
    public static Quagmire Instance;

    public GameObject giggity;
    public GameObject giggityPreview;
    public MeshRenderer giggityRenderer;
    public AudioSource giggityAudio;

    private bool _markedForDestruction;
    
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            _markedForDestruction = true;
            var poolee = GetComponent<Poolee>();
            poolee.Despawn();
            return;
        }

        _markedForDestruction = false;
        Instance = this;
        
        SetFields();
        SetQuality();
    }

    private void SetFields()
    {
        giggity = transform.Find("Art").gameObject;
        giggityPreview = transform.Find("Systems/Preview").gameObject;
        giggityRenderer = giggityPreview.GetComponent<MeshRenderer>();
        giggityAudio = transform.Find("Audio/TriggerSound").GetComponent<AudioSource>();
    }

    public void SetQuality()
    {
        switch (Preferences.Quality.Value)
        {
            case ImageQuality.Low:
                giggityRenderer.material = Assets.Materials.LowQuality;
                break;
            case ImageQuality.Medium:
                giggityRenderer.material = Assets.Materials.MediumQuality;
                break;
            case ImageQuality.High:
                giggityRenderer.material = Assets.Materials.HighQuality;
                break;
            default:
                ModConsole.Error("Invalid quality setting!");
                break;
        }
    }
	
    public void SendCapture()
    {
        if (LabCamera.Instance == null) return;
        giggityAudio.Play();
        LabCamera.Instance.Capture();
    }

    public void OnDisable()
    {
        if (_markedForDestruction) return;
        Instance = null;
    }

    public Quagmire(IntPtr ptr) : base(ptr) { }
}