using System;
using Xamarin.Forms;

namespace Pskin.Renders
{
    public class GradientColorStack:StackLayout
    {
        public Color StartColor
        {
            get;
            set;
        }
        public Color EndColor
        {
            get;
            set;
        }
    }
}
