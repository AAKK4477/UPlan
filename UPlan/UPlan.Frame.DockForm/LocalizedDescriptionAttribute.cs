namespace UPlan.Frame.DockForm
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private bool m_initialized;

        public LocalizedDescriptionAttribute(string key) : base(key)
        {
            this.m_initialized = false;
        }

        public override string Description
        {
            get
            {
                if (!this.m_initialized)
                {
                    string description = base.Description;
                    base.DescriptionValue = UPlan.Frame.DockForm.ResourceHelper.GetString(description);
                    if (base.DescriptionValue == null)
                    {
                        base.DescriptionValue = string.Empty;
                    }
                    this.m_initialized = true;
                }
                return base.DescriptionValue;
            }
        }
    }
}

