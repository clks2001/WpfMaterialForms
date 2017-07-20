﻿using System.Collections.Generic;
using MaterialForms.Wpf.Resources;

namespace MaterialForms.Wpf.Fields.Defaults
{
    public class BooleanField : DataFormField
    {
        public BooleanField(string key) : base(key)
        {
        }

        public bool IsSwitch { get; set; }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context, IDictionary<string, IValueProvider> formResources)
        {
            if (IsSwitch)
            {
                return new SwitchBindingProvider(context, Resources, formResources);
            }

            return  new CheckBoxBindingProvider(context, Resources, formResources);
        }
    }

    public class CheckBoxBindingProvider : BindingProvider
    {
        public CheckBoxBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }

    public class SwitchBindingProvider : BindingProvider
    {
        public SwitchBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
