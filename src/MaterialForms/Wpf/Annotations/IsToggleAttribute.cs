﻿using System;

namespace MaterialForms.Wpf.Annotations
{
    public static class Display
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class ToggleAttribute : Attribute
        {
        }
    }
}
