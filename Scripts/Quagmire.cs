namespace LabCam.Scripts;

// giggity
[RegisterTypeInIl2Cpp]
public class Quagmire : MonoBehaviour
{
    public static Quagmire Instance;
    
    private void Awake()
    {
        Instance = this;
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