using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Survivor
{
    public class WorldSpaceUIcontroller : MonoBehaviour
    {
        [SerializeField] private GameObject _damageInconPrefab;
        private Transform _mainCameraTransform;

        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            // DealDamgaeSystem을 가져오는 코드
            var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DealDamageSystem>();
            Debug.Log("dealDamageSystem : " + dealDamageSystem.ToString());
            dealDamageSystem.OnDealDamage += DisplayDamageIcon;
            dealDamageSystem.OnGrantExperience += DisplayExperienceIcon;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DealDamageSystem>();
            dealDamageSystem.OnDealDamage -= DisplayDamageIcon;
            dealDamageSystem.OnGrantExperience -= DisplayExperienceIcon;
        }

        private void DisplayDamageIcon(int damgeAmount, float3 startPositon)
        {
            Debug.Log("DisplayDamageIcon damgeAmount : " + damgeAmount);
            var directionToCamera = (Vector3)startPositon - _mainCameraTransform.position;
            var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
            var newIcon = Instantiate(_damageInconPrefab, startPositon, rotationToCamera, transform);
            var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
            newIconText.text = $"<color=red>-{damgeAmount.ToString()}</color>";
        }

        private void DisplayExperienceIcon(int experienceAmount, float3 startPositon)
        {
            Debug.Log("DisplayDamageIcon experienceAmount : " + experienceAmount);
            var directionToCamera = (Vector3)startPositon - _mainCameraTransform.position;
            var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
            var newIcon = Instantiate(_damageInconPrefab, startPositon, rotationToCamera, transform);
            var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
            newIconText.text = $"<color=yellow>+{experienceAmount.ToString()} EXP</color>";
        }
    }
}

