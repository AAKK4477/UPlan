namespace UPlan.Frame.DockForm
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string key) : base(key)
        {
        }

        protected override string GetLocalizedString(string key)
        {
            return UPlan.Frame.DockForm.ResourceHelper.GetString(key);
        }
    }
}

