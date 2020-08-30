using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Luke.Forms.CameraView
{
    public class CameraView : Frame
    {
        public static readonly BindableProperty NameProperty = BindableProperty.Create (
            propertyName: "Name",
            returnType: typeof(string),
            declaringType: typeof(CameraView),
            defaultValue: "CameraView");

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: nameof(Camera),
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraView),
            defaultValue: CameraOptions.Back);
        
        public static readonly BindableProperty AspectRatioProperty = BindableProperty.Create(
            propertyName: nameof(AspectRatio),
            returnType: typeof(AspectRatioOptions),
            declaringType: typeof(CameraView),
            defaultValue: AspectRatioOptions.Default);

        public CameraOptions Camera { get => (CameraOptions)GetValue(CameraProperty); set=> SetValue(CameraProperty, value); }
        public AspectRatioOptions AspectRatio { get => (AspectRatioOptions)GetValue(AspectRatioProperty); set=> SetValue(AspectRatioProperty, value); }
    }
}
