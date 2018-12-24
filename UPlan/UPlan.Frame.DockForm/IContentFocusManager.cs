namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections.Generic;

    public interface IContentFocusManager
    {
        void Activate(IDockContent content);
        void AddToList(IDockContent content);
        void GiveUpFocus(IDockContent content);
        void RemoveFromList(IDockContent content);

        List<IDockContent> ListContent { get; }
    }
}

