namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    internal static class DockHelper
    {
        private static Dictionary<DockState, DockState> AutoHideMap = new Dictionary<DockState, DockState>();

        static DockHelper()
        {
            InitAutoHideMap();
        }

        private static bool EstimateDockBottom(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.DockBottom) == 0) && ((dockState == DockState.DockBottom) || (dockState == DockState.DockBottomAutoHide)))
            {
                return false;
            }
            return true;
        }

        private static bool EstimateDockDocument(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.Document) == 0) && (dockState == DockState.Document))
            {
                return false;
            }
            return EstimateDockLeft(dockState, dockableAreas);
        }

        private static bool EstimateDockFloat(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.Float) == 0) && (dockState == DockState.Float))
            {
                return false;
            }
            return EstimateDockDocument(dockState, dockableAreas);
        }

        private static bool EstimateDockLeft(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.DockLeft) == 0) && ((dockState == DockState.DockLeft) || (dockState == DockState.DockLeftAutoHide)))
            {
                return false;
            }
            return EstimateDockRight(dockState, dockableAreas);
        }

        private static bool EstimateDockRight(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.DockRight) == 0) && ((dockState == DockState.DockRight) || (dockState == DockState.DockRightAutoHide)))
            {
                return false;
            }
            return EstimateDockTop(dockState, dockableAreas);
        }

        private static bool EstimateDockState(DockState dockState, DockAreas dockableAreas)
        {
            return EstimateDockFloat(dockState, dockableAreas);
        }

        private static bool EstimateDockTop(DockState dockState, DockAreas dockableAreas)
        {
            if (((dockableAreas & DockAreas.DockTop) == 0) && ((dockState == DockState.DockTop) || (dockState == DockState.DockTopAutoHide)))
            {
                return false;
            }
            return EstimateDockBottom(dockState, dockableAreas);
        }

        public static FloatWindow FloatWindowAtPoint(Point pt, DockPanel dockPanel)
        {
            for (Control control = Win32Helper.ControlAtPoint(pt); control != null; control = control.Parent)
            {
                FloatWindow window = control as FloatWindow;
                if ((window != null) && (window.DockPanel == dockPanel))
                {
                    return window;
                }
            }
            return null;
        }

        private static void InitAutoHideMap()
        {
            AutoHideMap.Add(DockState.DockLeft, DockState.DockLeftAutoHide);
            AutoHideMap.Add(DockState.DockRight, DockState.DockRightAutoHide);
            AutoHideMap.Add(DockState.DockTop, DockState.DockTopAutoHide);
            AutoHideMap.Add(DockState.DockBottom, DockState.DockBottomAutoHide);
            AutoHideMap.Add(DockState.DockLeftAutoHide, DockState.DockLeft);
            AutoHideMap.Add(DockState.DockRightAutoHide, DockState.DockRight);
            AutoHideMap.Add(DockState.DockTopAutoHide, DockState.DockTop);
            AutoHideMap.Add(DockState.DockBottomAutoHide, DockState.DockBottom);
        }

        public static bool IsDockStateAutoHide(DockState dockState)
        {
            return ((((dockState == DockState.DockLeftAutoHide) || (dockState == DockState.DockRightAutoHide)) || (dockState == DockState.DockTopAutoHide)) || (dockState == DockState.DockBottomAutoHide));
        }

        public static bool IsDockStateValid(DockState dockState, DockAreas dockableAreas)
        {
            return EstimateDockFloat(dockState, dockableAreas);
        }

        public static bool IsDockWindowState(DockState state)
        {
            return ((((state == DockState.DockTop) || (state == DockState.DockBottom)) || ((state == DockState.DockLeft) || (state == DockState.DockRight))) || (state == DockState.Document));
        }

        public static DockPane PaneAtPoint(Point pt, DockPanel dockPanel)
        {
            for (Control control = Win32Helper.ControlAtPoint(pt); control != null; control = control.Parent)
            {
                IDockContent content = control as IDockContent;
                if ((content != null) && (content.DockHandler.DockPanel == dockPanel))
                {
                    return content.DockHandler.Pane;
                }
                DockPane pane = control as DockPane;
                if ((pane != null) && (pane.DockPanel == dockPanel))
                {
                    return pane;
                }
            }
            return null;
        }

        public static DockState ToggleAutoHideState(DockState state)
        {
            if (AutoHideMap.ContainsKey(state))
            {
                return AutoHideMap[state];
            }
            return state;
        }
    }
}

