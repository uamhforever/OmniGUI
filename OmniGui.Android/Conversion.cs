﻿using System;
using System.IO;
using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using OmniGui.Xaml;
using OmniXaml;
using OmniXaml.Attributes;
using AndroidBitmap = Android.Graphics.Bitmap;

namespace OmniGui.Android
{
    public class Conversion
    {
        [TypeConverterMember(typeof(Bitmap))]
        public static Func<string, ConvertContext, (bool, object)> ThicknessConverter = (str, v) => (true, GetBitmap(str));

        private ITypeLocator locator;

        private static Bitmap GetBitmap(string str)
        {
            AndroidBitmap bmp;
            using (var stream = AndroidPlatform.Current.Assets.Open(str))
            {
                
                bmp = BitmapFactory.DecodeStream(stream);                               
            }

            var pixels = new int[bmp.Width * bmp.Height];
            bmp.GetPixels(pixels, 0, bmp.Width, 0, 0, bmp.Width, bmp.Height);
            
            return new Bitmap
            {
                Width = bmp.Width,
                Height = bmp.Height,
                Bytes = pixels.Select(i => (byte)i).ToArray(),
            };
        }
    }
}