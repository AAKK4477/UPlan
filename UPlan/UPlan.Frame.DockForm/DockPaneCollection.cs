using System.Collections.Generic;

namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections.ObjectModel;

    public class DockPaneCollection : ReadOnlyCollection<DockPane>
    {
        internal DockPaneCollection() : base(new List<DockPane>())
        {
        }

        internal int Add(DockPane pane)
        {
            if (base.Items.Contains(pane))
            {
                return base.Items.IndexOf(pane);
            }
            base.Items.Add(pane);
            return (base.Count - 1);
        }

        internal void AddAt(DockPane pane, int index)
        {
            if (((index >= 0) && (index <= (base.Items.Count - 1))) && !base.Contains(pane))
            {
                base.Items.Insert(index, pane);
            }
        }

        internal void Dispose()
        {
            for (int i = base.Count - 1; i >= 0; i--)
            {
                base[i].Close();
            }
        }

        internal void Remove(DockPane pane)
        {
            base.Items.Remove(pane);
        }
    }
}

