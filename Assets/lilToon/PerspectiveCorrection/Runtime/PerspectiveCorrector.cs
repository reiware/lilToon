using UnityEngine;

namespace Reiware.LilToonPerspectiveCorrection
{
    [ExecuteAlways]
    public sealed class PerspectiveCorrector : MonoBehaviour
    {
        [Header("References")] 
        
        [Tooltip("Head bone used as the center of the perspective correction")]
        [SerializeField] private Transform headBone;

        [Tooltip("Optional camera used to calculate the correction direction")] [SerializeField]
        private Camera targetCamera;

        [Tooltip("Renderers that receive the perspective correction properties.")]
        [SerializeField] private Renderer[] targetRenderers;
        [Header("Effect")] [Range(0f, 1f)] [SerializeField]
        private float amount = 1f;

        [Tooltip("Maximum distance from the head bone affected by the correction.")]
        [Min(0.0001f)] [SerializeField] private float radius = 0.25f;
        
        [Tooltip("Distance below the head bone where the correction region begins.")]
        [Min(0f)] [SerializeField] private float startHeight = 0.20f;
        
        [Tooltip("Distance above the head bone included in the correction region.")]
        [Min(0f)] [SerializeField] private float endHeight = 0.10f;

        private static readonly int HeadPositionId = Shader.PropertyToID("_HeadBonePositionWS");
        private static readonly int CameraPositionId = Shader.PropertyToID("_MainCameraPositionWS");
        private static readonly int UseCustomCameraId = Shader.PropertyToID("_UseCustomPerspectiveCamera");
        private static readonly int AmountId = Shader.PropertyToID("_PerspectiveRemovalAmount");
        private static readonly int RadiusId = Shader.PropertyToID("_PerspectiveRemovalRadius");
        private static readonly int StartHeightId = Shader.PropertyToID("_PerspectiveRemovalStartHeight");
        private static readonly int EndHeightId = Shader.PropertyToID("_PerspectiveRemovalEndHeight");

        private MaterialPropertyBlock propertyBlock;

        private void Reset()
        {
            targetRenderers = GetComponentsInChildren<Renderer>(true);
        }

        private void OnEnable()
        {
            EnsureInitialized();
            Apply();
        }
        
        private void LateUpdate()
        {
            Apply();
        }

        private void OnValidate()
        {
            EnsureInitialized();
            Apply();
        }

        private void OnDisable()
        {
            SetAmount(0f);
        }

        private void EnsureInitialized()
        {
            propertyBlock ??= new MaterialPropertyBlock();

            if (targetRenderers == null || targetRenderers.Length == 0)
                targetRenderers = GetComponentsInChildren<Renderer>(true);
        }

        private void Apply()
        {
            if (headBone == null || targetRenderers == null || targetRenderers.Length == 0)
                return;

            propertyBlock ??= new MaterialPropertyBlock();

            Vector3 headPosition = headBone.position;
            Vector4 headPosV4 = new Vector4(headPosition.x, headPosition.y, headPosition.z, 1f);

            bool useCustomCamera = targetCamera != null;
            Vector3 cameraPosition = useCustomCamera ? targetCamera.transform.position : Vector3.zero;
            Vector4 camPosV4 = new Vector4(cameraPosition.x, cameraPosition.y, cameraPosition.z, 1f);
    
            float customCamFloat = useCustomCamera ? 1f : 0f;
            float clampedAmount = Mathf.Clamp(amount, 0f, 1f);
            float clampedRadius = Mathf.Max(radius, 0.0001f);

            foreach (Renderer targetRenderer in targetRenderers)
            {
                if (targetRenderer == null)
                    continue;

                targetRenderer.GetPropertyBlock(propertyBlock);

                propertyBlock.SetVector(HeadPositionId, headPosV4);
                propertyBlock.SetVector(CameraPositionId, camPosV4);
                propertyBlock.SetFloat(UseCustomCameraId, customCamFloat);
                propertyBlock.SetFloat(AmountId, clampedAmount);
                propertyBlock.SetFloat(RadiusId, clampedRadius);
                propertyBlock.SetFloat(StartHeightId, startHeight);
                propertyBlock.SetFloat(EndHeightId, endHeight);

                targetRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void SetAmount(float value)
        {
            if (targetRenderers == null)
                return;

            propertyBlock ??= new MaterialPropertyBlock();

            foreach (Renderer targetRenderer in targetRenderers)
            {
                if (targetRenderer == null)
                    continue;

                targetRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat(AmountId, value);
                targetRenderer.SetPropertyBlock(propertyBlock);
            }
        }
    }
}