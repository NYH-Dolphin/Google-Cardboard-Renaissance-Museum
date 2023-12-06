using UnityEngine;

namespace DefaultNamespace.Museum
{
    [RequireComponent(typeof(MeshRenderer))]
    public class InteractablePaintingBehaviour : InteractableBehaviour
    {
        private Material painting;
        [SerializeField] private AudioClip congratsClip;
        

        protected void Start()
        {
            base.Start();
            painting = GetComponent<MeshRenderer>().materials[1];
        }

        public void OnClickPainting()
        {
            PickImage();
        }

        private void PickImage()
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                // Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path);
                    if (texture == null)
                    {
                        // Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    // Assign texture to the painting
                    painting.mainTexture = texture;
                }
            });

            Debug.Log("upload!");
            MyInputManager.Instance.speech.clip = congratsClip;
            MyInputManager.Instance.speech.Play();
        }
    }
}