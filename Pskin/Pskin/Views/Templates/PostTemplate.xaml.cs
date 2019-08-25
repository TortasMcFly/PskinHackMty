using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pskin.Views.Templates
{
    public partial class PostTemplate : ContentView
    {
        public Image PostImage;
        public PostTemplate()
        {
            InitializeComponent();

            PostImage = Image;
        }


    }
}
