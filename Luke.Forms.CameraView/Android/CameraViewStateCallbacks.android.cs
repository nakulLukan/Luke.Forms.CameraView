using Android.Hardware.Camera2;
using Android.Runtime;
using System;

namespace Luke.Forms.CameraView
{
    public class CameraViewStateCallbacks : CameraDevice.StateCallback
    {
        readonly CameraViewRenderer viewRenderer = null;
        public CameraViewStateCallbacks(CameraViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;
        }
        public override void OnDisconnected(CameraDevice camera)
        {
            if (viewRenderer.CameraDevice != null)
            {
                viewRenderer.CameraDevice.Close();
                viewRenderer.CameraDevice = null;
            }
        }

        public override void OnError(CameraDevice camera, [GeneratedEnum] global::Android.Hardware.Camera2.CameraError error)
        {
            Console.WriteLine("Error on callback");
        }

        public override void OnOpened(CameraDevice camera)
        {
            viewRenderer.CameraDevice = camera;
            viewRenderer.CreateCameraPreview();
        }
    }
}
