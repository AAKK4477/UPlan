namespace UPlan.Frame.DockForm
{
    using System;
    using System.Resources;

    internal static class ResourceHelper
    {
        private static System.Resources.ResourceManager _resourceManager = null;

        public static string GetString(string name)
        {
            return ResourceManager.GetString(name);
        }

        private static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = new System.Resources.ResourceManager("UPlan.Frame.DockForm.Strings", typeof(UPlan.Frame.DockForm.ResourceHelper).Assembly);
                }
                return _resourceManager;
            }
        }
    }
}

