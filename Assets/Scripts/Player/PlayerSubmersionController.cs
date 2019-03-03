using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerSubmersionController : MonoBehaviour
{
    public PostProcessingProfile UnderwaterProfile;
    public PostProcessingProfile UnderwaterWeaponProfile;
    public PostProcessingBehaviour WeaponPostProcessingBehaviour;
    public float damageRate;

    private PlayerModel _playerModel;
    private PostProcessingBehaviour fpsPostProcessingBehaviour;

    private bool isInUnderwater = false;
    private PostProcessingProfile _initialProfile;
    private PostProcessingProfile _initialWeaponProfile;

    private float nextDamagableTime = float.MinValue;

    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        fpsPostProcessingBehaviour = _playerModel.fpsCamera.GetComponent<PostProcessingBehaviour>();
        _initialProfile = fpsPostProcessingBehaviour.profile;
        _initialWeaponProfile = WeaponPostProcessingBehaviour.profile;
    }

    private void Update()
    {
        isInUnderwater = RaisingWater.Instance.transform.position.y > _playerModel.fpsCamera.transform.position.y;

        PostProcessingProfile selectedProfile = _initialProfile;
        PostProcessingProfile selectedWeaponProfile = _initialWeaponProfile;
        if (isInUnderwater)
        {
            PlayerStats.Instance.UpdateHealth(-damageRate * Time.deltaTime);
            selectedProfile = UnderwaterProfile;
            selectedWeaponProfile = UnderwaterWeaponProfile;
        }

        fpsPostProcessingBehaviour.profile = selectedProfile;
        WeaponPostProcessingBehaviour.profile = selectedWeaponProfile;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Touching Water");
        }
    }
}