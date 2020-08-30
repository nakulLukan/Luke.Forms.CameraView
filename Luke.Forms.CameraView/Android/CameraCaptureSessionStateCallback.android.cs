using Android.Hardware.Camera2;
using Android.Widget;

namespace Luke.Forms.CameraView
{
    public class CameraCaptureSessionStateCallback : CameraCaptureSession.StateCallback
    {
        readonly CameraViewRenderer viewRenderer = null;
        CameraCaptureSession cameraCaptureSessions = null;
        public CameraCaptureSessionStateCallback(CameraViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;
        }
        public override void OnConfigured(CameraCaptureSession session)
        {
            if (viewRenderer.CameraDevice == null)
            {
                return;
            }
            // When the session is ready, we start displaying the preview.
            cameraCaptureSessions = session;
            updatePreview();

        }

        protected void updatePreview()
        {
            if (viewRenderer.CameraDevice == null)
            {
                throw new CameraViewException("Camera Device is null");
            }

            Java.Lang.Object controlMode = 1;
            viewRenderer.CaptureRequestBuilder.Set(CaptureRequest.ControlMode, controlMode);
            try
            {
                cameraCaptureSessions.SetRepeatingRequest(viewRenderer.CaptureRequestBuilder.Build(), null, null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            Toast.MakeText(viewRenderer.Context, "Configuration change", ToastLength.Short).Show();
        }
    }
}
