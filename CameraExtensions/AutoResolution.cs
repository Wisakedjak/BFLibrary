using UnityEngine;

namespace BFL.CameraExtensions
{
    public class AutoResolution
    {
        public int setWidth = 1440;
        public int setHeight = 2560;
        
        private void Start()
        {
            Camera camera = Camera.main;
            Rect rect = camera.rect;

            float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
            float scaleWidth = 1f / scaleHeight;

            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                rect.width = scaleWidth;
                rect.x = (1f - scaleWidth) / 2f;
            }
            
            camera.rect = rect;

            SetResolution();
        }

        public void SetResolution()
        {
            int deviceWidth = Screen.width;
            int deviceHeight = Screen.height;
            
            Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
            
            if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
            {
                float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
                Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
            }
            else
            {
                float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
                Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
            }
        }
    }
}