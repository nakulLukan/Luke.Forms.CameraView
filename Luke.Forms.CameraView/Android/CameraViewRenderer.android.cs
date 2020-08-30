using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Support.V7.Widget;
using Android.Views;
using Luke.Forms.CameraView;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Luke.Forms.CameraView
{
    public class CameraViewRenderer : ViewRenderer<CameraView, ViewGroup>
    {
        TextureView cameraPreview = null;

        public CameraDevice CameraDevice { get; set; }
        public CaptureRequest.Builder CaptureRequestBuilder { get; set; }

        readonly CameraViewStateCallbacks stateCallback = null;
        readonly SurfaceTextureListener surfaceTextureListener = null;
        readonly CameraCaptureSessionStateCallback cameraCaptureSessionStateCallback = null;

        public CameraViewRenderer(Context context) : base(context)
        {
            stateCallback = new CameraViewStateCallbacks(this);
            cameraCaptureSessionStateCallback = new CameraCaptureSessionStateCallback(this);
            surfaceTextureListener = new SurfaceTextureListener(this);
        }

        /// <summary>
        /// Get corresponding system camera id for the Camera Options
        /// </summary>
        /// <param name="cameraIds"></param>
        /// <param name="cameraOptions"></param>
        /// <returns></returns>
        private string GetCameraId(string[] cameraIds, CameraOptions cameraOptions)
        {
            if (cameraOptions == CameraOptions.Back && cameraIds.Length > 0)
            {
                return cameraIds.First();
            }
            else if (cameraOptions == CameraOptions.Front && cameraIds.Length > 1)
            {
                // camera id for secondary camera is 2
                return cameraIds.Skip(1).First();
            }

            throw new CameraViewException("Requested camera not available in the system");
        }

        private Vector<int, int> GetBufferSize(Vector<int, int> viewDimension)
        {
            var aspectRatio = Element.AspectRatio;
            var rect = new Vector<int, int>(viewDimension.X, viewDimension.Y);

            if (aspectRatio != AspectRatioOptions.Default)
            {
                var coeff = Common.GetAspectRatioCoeff(aspectRatio);
                if (viewDimension.X < viewDimension.Y)
                {
                    rect.X = viewDimension.X;
                    rect.Y = viewDimension.X * coeff.Y / coeff.X;
                }
                else
                {
                    rect.Y = viewDimension.Y;
                    rect.X = viewDimension.Y * coeff.X / coeff.Y;
                }
                SetPreviewSize(rect);
            }

            return rect;
        }

        private void SetPreviewSize(Vector<int, int> viewDimension)
        {
            (Control as CardView).SetContentPadding((int)Math.Abs(Element.Width - viewDimension.X), (int)Math.Abs(Element.Height - viewDimension.Y), (int)Math.Abs(Element.Width - viewDimension.X), (int)Math.Abs(Element.Height - viewDimension.Y));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var cardView = new CardView(Context);
                    cardView.SetCardBackgroundColor(e.NewElement.BackgroundColor.ToAndroid());
                    cameraPreview = new TextureView(Context);
                    cardView.AddView(cameraPreview);
                    SetNativeControl(cardView);
                }
                cameraPreview.SurfaceTextureListener = surfaceTextureListener;
            }
        }

        public void OpenCamera()
        {
            CameraManager manager = (CameraManager)Context.GetSystemService(Context.CameraService);
            try
            {
                string cameraId = GetCameraId(manager.GetCameraIdList(), Element.Camera);
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                if (map == null) throw new CameraViewException("map is null");
                manager.OpenCamera(cameraId, stateCallback, null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        public void CreateCameraPreview()
        {
            SurfaceTexture texture = cameraPreview.SurfaceTexture;
            if (texture == null) throw new CameraViewException("Failed to get the surface texture");
            var bufferSize = GetBufferSize(new Vector<int, int>((int)Element.Width, (int)Element.Height));
            //texture.SetDefaultBufferSize(bufferSize.X, bufferSize.Y);
            Surface surface = new Surface(texture);
            CaptureRequestBuilder = CameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
            CaptureRequestBuilder.AddTarget(surface);
            CameraDevice.CreateCaptureSession(new System.Collections.Generic.List<Surface>() { surface }, cameraCaptureSessionStateCallback, null);
        }

    }
}
