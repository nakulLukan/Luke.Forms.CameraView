using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luke.Forms.CameraView
{
    public class SurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        readonly CameraViewRenderer viewRenderer = null;

        public SurfaceTextureListener(CameraViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;

        }
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            viewRenderer.OpenCamera();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

    }
}
