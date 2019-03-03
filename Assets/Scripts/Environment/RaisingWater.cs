using UnityEngine;

public class RaisingWater : MonoBehaviour
{
    public enum WaterState
    {
        None,
        Raising,
        Draining,
    }

    public static RaisingWater Instance;

    public float WaterRaiseSpeed;
    public float WaterDrainSpeed;

    private float startYPos;
    
    [SerializeField] private WaterState _waterState = WaterState.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        startYPos = transform.position.y;
    }

    private void Update()
    {
        switch (_waterState)
        {
            case WaterState.Raising:
                RaiseWater();
                break;
            case WaterState.Draining:
                DrainWater();
                break;
        }
    }
    
    public void InitRaisingWater()
    {
        _waterState = WaterState.Raising;
    }

    public void InitDrainWater()
    {
        _waterState = WaterState.Draining;
    }

    private void RaiseWater()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y += WaterRaiseSpeed;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }

    private void DrainWater()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y += WaterDrainSpeed;

        if (targetPosition.y < startYPos)
        {
            targetPosition.y = startYPos;
            _waterState = WaterState.None;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}