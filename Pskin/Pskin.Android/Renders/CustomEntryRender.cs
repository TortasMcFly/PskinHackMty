using System;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Pskin.Droid.Renders;
using Pskin.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRender))]
namespace Pskin.Droid.Renders
{
    [Obsolete]
    public class CustomEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);

                Control.SetBackgroundDrawable(gd);
                Control.SetRawInputType(Android.Text.InputTypes.TextFlagNoSuggestions);
            }
        }
    }
}
