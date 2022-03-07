using System.Collections.Generic;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Cameras
{
    /* Fades out any objects between the player and this transform.
       The renderer's shader is first changed to be an Alpha/Diffuse,
       then alpha is faded out to fadedOutAlpha.

       In order to catch all occluders, 5 rays are casted.
       'occlusionRadius' is the distance between them.
    */

    [AddComponentMenu("Scripts/GameBrains/Cameras/Fade Out Line Of Sight")]
    public sealed class FadeOutLineOfSight : ExtendedMonoBehaviour
    {
        [SerializeField] LayerMask layerMask = -1;
        [SerializeField] float fadeSpeed = 1.0f;
        [SerializeField] float occlusionRadius = 0.3f;
        [SerializeField] float fadedOutAlpha = 0.3f;
        [SerializeField] Transform target;
        [SerializeField] bool warnIfNoTarget;

        Camera attachedCamera;

        readonly List<FadeOutLOSInfo> fadedOutObjects = new List<FadeOutLOSInfo>();

        public override void Awake()
        {
            base.Awake();

            if (warnIfNoTarget && !target)
            {
                Log.Debug("Please assign a target to FadeOutLineOfSight.");
            }

            attachedCamera = GetComponent<Camera>();

            if (warnIfNoTarget && !attachedCamera) { Log.Debug("No attached camera."); }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            SelectableCamera.OnUpdated += HandleCameraUpdate;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            SelectableCamera.OnUpdated -= HandleCameraUpdate;
        }

        public void HandleCameraUpdate(SelectableCamera selectableCamera)
        {
            if (selectableCamera.gameObject != gameObject) { return; }

            if (selectableCamera is TargetedCamera targetedCamera)
            {
                target = targetedCamera.Target;
                if (!target) return;
            }
            else { return; }

            Vector3 from = transform.position;
            Vector3 to = target.position;
            var castDistance = Vector3.Distance(to, from);

            // Mark all objects as not needing fade out.
            foreach (FadeOutLOSInfo fade in fadedOutObjects) { fade.needFadeOut = false; }

            Vector3[] offsets =
                {
                    new Vector3(0f, 0f, 0f),
                    new Vector3(0f, occlusionRadius, 0f),
                    new Vector3(0f, -occlusionRadius, 0f),
                    new Vector3(occlusionRadius, 0f, 0f),
                    new Vector3(-occlusionRadius, 0f, 0f)
                };

            // We cast 5 rays to really make sure even occluders that are partly occluding
            // the player are faded out.
            foreach (Vector3 offset in offsets)
            {
                Vector3 relativeOffset = transform.TransformDirection(offset);

                // Find all blocking objects which we want to hide.
                var hits
                    = Physics.RaycastAll(
                        from + relativeOffset,
                        to - from, castDistance,
                        layerMask.value);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject == target.gameObject) continue;

                    // Make sure we have a renderer.
                    var hitRenderer = hit.collider.GetComponentInChildren<Renderer>();
                    if (!hitRenderer || !hitRenderer.enabled) continue;

                    FadeOutLOSInfo info = FindLosInfo(hitRenderer);

                    // We are not fading this renderer already, so insert into faded objects map.
                    if (info == null)
                    {
                        info = new FadeOutLOSInfo();
                        info.originalMaterials = hitRenderer.sharedMaterials;
                        info.alphaMaterials = new Material[info.originalMaterials.Length];
                        info.renderer = hitRenderer;
                        for (var i = 0; i < info.originalMaterials.Length; i++)
                        {
                            //TODO-6: Investigate whether material pooling can reduce garbage.
                            var newMaterial = new Material(Shader.Find("Alpha/Diffuse"));
                            newMaterial.mainTexture = info.originalMaterials[i].mainTexture;
                            Color color = info.originalMaterials[i].color;
                            color.a = 1.0f;
                            newMaterial.color = color;
                            info.alphaMaterials[i] = newMaterial;
                        }

                        hitRenderer.sharedMaterials = info.alphaMaterials;
                        fadedOutObjects.Add(info);
                    }
                    // Just mark the renderer as needing fade out.
                    else
                    {
                        info.needFadeOut = true;
                    }
                }
            }

            // Now go over all renderers and do the actual fading!
            var fadeDelta = fadeSpeed * Time.deltaTime;
            for (var i = 0; i < fadedOutObjects.Count; i++)
            {
                FadeOutLOSInfo fade = fadedOutObjects[i];
                // Fade out up to minimum alpha value.
                if (fade.needFadeOut)
                {
                    foreach (Material alphaMaterial in fade.alphaMaterials)
                    {
                        var alpha = alphaMaterial.color.a;
                        alpha -= fadeDelta;
                        alpha = Mathf.Max(alpha, fadedOutAlpha);
                        Color color = alphaMaterial.color;
                        color.a = alpha;
                        alphaMaterial.color = color;
                    }
                }
                // Fade back in.
                else
                {
                    var totallyFadedIn = 0;
                    foreach (Material alphaMaterial in fade.alphaMaterials)
                    {
                        var alpha = alphaMaterial.color.a;
                        alpha += fadeDelta;
                        alpha = Mathf.Min(alpha, 1.0f);
                        Color color = alphaMaterial.color;
                        color.a = alpha;
                        alphaMaterial.color = color;
                        if (alpha >= 0.99f) { totallyFadedIn++; }
                    }

                    // All alpha materials are faded back to 100%
                    // Thus we can switch back to the original materials.
                    if (totallyFadedIn == fade.alphaMaterials.Length)
                    {
                        if (fade.renderer)
                        {
                            fade.renderer.sharedMaterials = fade.originalMaterials;
                        }

                        foreach (Material newMaterial in fade.alphaMaterials)
                        {
                            Destroy(newMaterial);
                        }

                        fadedOutObjects.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        FadeOutLOSInfo FindLosInfo(Renderer r)
        {
            foreach (FadeOutLOSInfo fade in fadedOutObjects)
            {
                if (r == fade.renderer) { return fade; }
            }

            return null;
        }

        public class FadeOutLOSInfo
        {
            public Material[] alphaMaterials;
            public bool needFadeOut = true;
            public Material[] originalMaterials;
            public Renderer renderer;
        }
    }
}