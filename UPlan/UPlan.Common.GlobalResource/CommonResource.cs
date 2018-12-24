namespace UPlan.Common.GlobalResource
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Threading;

    [Obfuscation(Exclude = true, ApplyToMembers = true), DebuggerNonUserCode, GeneratedCode("DMKSoftware.CodeGenerators.Tools.StronglyTypedResourceBuilderEx", "2.6.0.0")]
    public class CommonResource
    {
        private static object _internalSyncObject;
        private static CultureInfo _resourceCulture;
        private static System.Resources.ResourceManager _resourceManager;

        public static string COMMON_BIGGERTHAN_ZEROFormat(object arg0, object arg1, object arg2)
        {
            return string.Format(_resourceCulture, COMMON_BIGGERTHAN_ZERO, new object[] { arg0, arg1, arg2 });
        }

        public static string COMMON_LESS_THANFormat(object arg0, object arg1)
        {
            return string.Format(_resourceCulture, COMMON_LESS_THAN, new object[] { arg0, arg1 });
        }

        public static string EXCEL_EXPORT_FAILFormat(object arg0)
        {
            return string.Format(_resourceCulture, EXCEL_EXPORT_FAIL, new object[] { arg0 });
        }

        public static string EXCEL_LOAD_FAILFormat(object arg0)
        {
            return string.Format(_resourceCulture, EXCEL_LOAD_FAIL, new object[] { arg0 });
        }

        public static string COMMENT_ERROR
        {
            get
            {
                return ResourceManager.GetString("COMMENT_ERROR", _resourceCulture);
            }
        }

        public static string COMMON_BIGGERTHAN_ZERO
        {
            get
            {
                return ResourceManager.GetString("COMMON_BIGGERTHAN_ZERO", _resourceCulture);
            }
        }

        public static string COMMON_IN_RANGE
        {
            get
            {
                return ResourceManager.GetString("COMMON_IN_RANGE", _resourceCulture);
            }
        }

        public static string COMMON_INVALID_FIELD
        {
            get
            {
                return ResourceManager.GetString("COMMON_INVALID_FIELD", _resourceCulture);
            }
        }

        public static string COMMON_LESS_THAN
        {
            get
            {
                return ResourceManager.GetString("COMMON_LESS_THAN", _resourceCulture);
            }
        }

        public static string COMMON_NOT_NEGATIVE
        {
            get
            {
                return ResourceManager.GetString("COMMON_NOT_NEGATIVE", _resourceCulture);
            }
        }

        public static string COMMON_OVER_RANGE
        {
            get
            {
                return ResourceManager.GetString("COMMON_OVER_RANGE", _resourceCulture);
            }
        }

        public static string CONTROLS_1PT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_1PT", _resourceCulture);
            }
        }

        public static string CONTROLS_1ST_DATA_ROW
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_1ST_DATA_ROW", _resourceCulture);
            }
        }

        public static string CONTROLS_2PT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_2PT", _resourceCulture);
            }
        }

        public static string CONTROLS_3PT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_3PT", _resourceCulture);
            }
        }

        public static string CONTROLS_6PT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_6PT", _resourceCulture);
            }
        }

        public static string CONTROLS_AVAILABLE_FIELD
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_AVAILABLE_FIELD", _resourceCulture);
            }
        }

        public static string CONTROLS_BOLD
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_BOLD", _resourceCulture);
            }
        }

        public static string CONTROLS_CANCEL
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_CANCEL", _resourceCulture);
            }
        }

        public static string CONTROLS_CLOSE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_CLOSE", _resourceCulture);
            }
        }

        public static string CONTROLS_COLUMNS_TO_BE_DISPLAYED
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_COLUMNS_TO_BE_DISPLAYED", _resourceCulture);
            }
        }

        public static string CONTROLS_CONFIGURATION_FILE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_CONFIGURATION_FILE", _resourceCulture);
            }
        }

        public static string CONTROLS_CUT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_CUT", _resourceCulture);
            }
        }

        public static string CONTROLS_DATAEXPORT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_DATAEXPORT", _resourceCulture);
            }
        }

        public static string CONTROLS_DATAIMPORT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_DATAIMPORT", _resourceCulture);
            }
        }

        public static string CONTROLS_DELETE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_DELETE", _resourceCulture);
            }
        }

        public static string CONTROLS_DISPLAY_COLUMNS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_DISPLAY_COLUMNS", _resourceCulture);
            }
        }

        public static string CONTROLS_EXPORT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_EXPORT", _resourceCulture);
            }
        }

        public static string CONTROLS_EXPORTED_FIELD
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_EXPORTED_FIELD", _resourceCulture);
            }
        }

        public static string CONTROLS_FIELD_MAPPING
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FIELD_MAPPING", _resourceCulture);
            }
        }

        public static string CONTROLS_FIELD_SEPARATOR
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FIELD_SEPARATOR", _resourceCulture);
            }
        }

        public static string CONTROLS_FONT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FONT", _resourceCulture);
            }
        }

        public static string CONTROLS_FONTSTYLE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FONTSTYLE", _resourceCulture);
            }
        }

        public static string CONTROLS_FORE_COLOUR
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FORE_COLOUR", _resourceCulture);
            }
        }

        public static string CONTROLS_FREEZE_COLUMNS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_FREEZE_COLUMNS", _resourceCulture);
            }
        }

        public static string CONTROLS_HEADER
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_HEADER", _resourceCulture);
            }
        }

        public static string CONTROLS_HIDE_COLUMNS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_HIDE_COLUMNS", _resourceCulture);
            }
        }

        public static string CONTROLS_IMPORT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_IMPORT", _resourceCulture);
            }
        }

        public static string CONTROLS_ITALIC
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_ITALIC", _resourceCulture);
            }
        }

        public static string CONTROLS_LINE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_LINE", _resourceCulture);
            }
        }

        public static string CONTROLS_LOAD
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_LOAD", _resourceCulture);
            }
        }

        public static string CONTROLS_M
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_M", _resourceCulture);
            }
        }

        public static string CONTROLS_MAIN_COLOR
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_MAIN_COLOR", _resourceCulture);
            }
        }

        public static string CONTROLS_OK
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_OK", _resourceCulture);
            }
        }

        public static string CONTROLS_OPEN_LICENSE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_OPEN_LICENSE", _resourceCulture);
            }
        }

        public static string CONTROLS_OTHER
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_OTHER", _resourceCulture);
            }
        }

        public static string CONTROLS_PASTE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_PASTE", _resourceCulture);
            }
        }

        public static string CONTROLS_PREVIEW
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_PREVIEW", _resourceCulture);
            }
        }

        public static string CONTROLS_SAMPLE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SAMPLE", _resourceCulture);
            }
        }

        public static string CONTROLS_SAVE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SAVE", _resourceCulture);
            }
        }

        public static string CONTROLS_SAVE_FILE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SAVE_FILE", _resourceCulture);
            }
        }

        public static string CONTROLS_SELECT_ALL
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SELECT_ALL", _resourceCulture);
            }
        }

        public static string CONTROLS_SELECT_NONE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SELECT_NONE", _resourceCulture);
            }
        }

        public static string CONTROLS_SIZE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SIZE", _resourceCulture);
            }
        }

        public static string CONTROLS_SORT_ASCENDING
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SORT_ASCENDING", _resourceCulture);
            }
        }

        public static string CONTROLS_SORT_DESCENDING
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SORT_DESCENDING", _resourceCulture);
            }
        }

        public static string CONTROLS_STRIKEOUT
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_STRIKEOUT", _resourceCulture);
            }
        }

        public static string CONTROLS_SYMBOL
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_SYMBOL", _resourceCulture);
            }
        }

        public static string CONTROLS_TABLE_FIELDS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_TABLE_FIELDS", _resourceCulture);
            }
        }

        public static string CONTROLS_UNDERLINE
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_UNDERLINE", _resourceCulture);
            }
        }

        public static string CONTROLS_UNFREEZE_ALL_COLUMNS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_UNFREEZE_ALL_COLUMNS", _resourceCulture);
            }
        }

        public static string CONTROLS_UPDATE_RECORDS
        {
            get
            {
                return ResourceManager.GetString("CONTROLS_UPDATE_RECORDS", _resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get
            {
                return _resourceCulture;
            }
            set
            {
                _resourceCulture = value;
            }
        }

        public static string DATA_LOAD_FAIL
        {
            get
            {
                return ResourceManager.GetString("DATA_LOAD_FAIL", _resourceCulture);
            }
        }

        public static string EXCEL_EXPORT_FAIL
        {
            get
            {
                return ResourceManager.GetString("EXCEL_EXPORT_FAIL", _resourceCulture);
            }
        }

        public static string EXCEL_LOAD_FAIL
        {
            get
            {
                return ResourceManager.GetString("EXCEL_LOAD_FAIL", _resourceCulture);
            }
        }

        public static string EXPORT_LEGEND
        {
            get
            {
                return ResourceManager.GetString("EXPORT_LEGEND", _resourceCulture);
            }
        }

        public static string IMPORT_LEGEND
        {
            get
            {
                return ResourceManager.GetString("IMPORT_LEGEND", _resourceCulture);
            }
        }

        public static object InternalSyncObject
        {
            get
            {
                if (object.ReferenceEquals(_internalSyncObject, null))
                {
                    Interlocked.CompareExchange(ref _internalSyncObject, new object(), null);
                }
                return _internalSyncObject;
            }
        }

        public static string LABEL_ACTIVITY
        {
            get
            {
                return ResourceManager.GetString("LABEL_ACTIVITY", _resourceCulture);
            }
        }

        public static string LABEL_CHANNEL_INDEX
        {
            get
            {
                return ResourceManager.GetString("LABEL_CHANNEL_INDEX", _resourceCulture);
            }
        }

        public static string LABEL_END_BREAK
        {
            get
            {
                return ResourceManager.GetString("LABEL_END_BREAK", _resourceCulture);
            }
        }

        public static string LABEL_END_COLOR
        {
            get
            {
                return ResourceManager.GetString("LABEL_END_COLOR", _resourceCulture);
            }
        }

        public static string LABEL_FIELD
        {
            get
            {
                return ResourceManager.GetString("LABEL_FIELD", _resourceCulture);
            }
        }

        public static string LABEL_FREQUENCY_BAND
        {
            get
            {
                return ResourceManager.GetString("LABEL_FREQUENCY_BAND", _resourceCulture);
            }
        }

        public static string LABEL_FREQUENCY_SWITCH
        {
            get
            {
                return ResourceManager.GetString("LABEL_FREQUENCY_SWITCH", _resourceCulture);
            }
        }

        public static string LABEL_INTERVAL
        {
            get
            {
                return ResourceManager.GetString("LABEL_INTERVAL", _resourceCulture);
            }
        }

        public static string LABEL_LENGTH
        {
            get
            {
                return ResourceManager.GetString("LABEL_LENGTH", _resourceCulture);
            }
        }

        public static string LABEL_START_BREAK
        {
            get
            {
                return ResourceManager.GetString("LABEL_START_BREAK", _resourceCulture);
            }
        }

        public static string LABEL_START_COLOR
        {
            get
            {
                return ResourceManager.GetString("LABEL_START_COLOR", _resourceCulture);
            }
        }

        public static string LONGITUDE_OR_LATITUDE
        {
            get
            {
                return ResourceManager.GetString("LONGITUDE_OR_LATITUDE", _resourceCulture);
            }
        }

        public static string MAX_VALUE
        {
            get
            {
                return ResourceManager.GetString("MAX_VALUE", _resourceCulture);
            }
        }

        public static string MIN_VALUE
        {
            get
            {
                return ResourceManager.GetString("MIN_VALUE", _resourceCulture);
            }
        }

        public static string NAME_EMPTY
        {
            get
            {
                return ResourceManager.GetString("NAME_EMPTY", _resourceCulture);
            }
        }

        public static string NAME_END_ERROR
        {
            get
            {
                return ResourceManager.GetString("NAME_END_ERROR", _resourceCulture);
            }
        }

        public static string NAME_LENGTH_ERROR
        {
            get
            {
                return ResourceManager.GetString("NAME_LENGTH_ERROR", _resourceCulture);
            }
        }

        public static string NAME_REGEX_ERROR
        {
            get
            {
                return ResourceManager.GetString("NAME_REGEX_ERROR", _resourceCulture);
            }
        }

        public static string NEIGHBOR_CELL
        {
            get
            {
                return ResourceManager.GetString("NEIGHBOR_CELL", _resourceCulture);
            }
        }

        public static string POINT_ANALYSIS
        {
            get
            {
                return ResourceManager.GetString("POINT_ANALYSIS", _resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(_resourceManager, null))
                {
                    lock (InternalSyncObject)
                    {
                        if (object.ReferenceEquals(_resourceManager, null))
                        {
                            Interlocked.Exchange<System.Resources.ResourceManager>(ref _resourceManager, new System.Resources.ResourceManager("ENet.Common.GlobalResource.CommonResource", typeof(CommonResource).Assembly));
                        }
                    }
                }
                return _resourceManager;
            }
        }

        public static string TASK_EXCUTING
        {
            get
            {
                return ResourceManager.GetString("TASK_EXCUTING", _resourceCulture);
            }
        }

        public class ResourceNames
        {
            public const string COMMENT_ERROR = "COMMENT_ERROR";
            public const string COMMON_BIGGERTHAN_ZERO = "COMMON_BIGGERTHAN_ZERO";
            public const string COMMON_IN_RANGE = "COMMON_IN_RANGE";
            public const string COMMON_INVALID_FIELD = "COMMON_INVALID_FIELD";
            public const string COMMON_LESS_THAN = "COMMON_LESS_THAN";
            public const string COMMON_NOT_NEGATIVE = "COMMON_NOT_NEGATIVE";
            public const string COMMON_OVER_RANGE = "COMMON_OVER_RANGE";
            public const string CONTROLS_1PT = "CONTROLS_1PT";
            public const string CONTROLS_1ST_DATA_ROW = "CONTROLS_1ST_DATA_ROW";
            public const string CONTROLS_2PT = "CONTROLS_2PT";
            public const string CONTROLS_3PT = "CONTROLS_3PT";
            public const string CONTROLS_6PT = "CONTROLS_6PT";
            public const string CONTROLS_AVAILABLE_FIELD = "CONTROLS_AVAILABLE_FIELD";
            public const string CONTROLS_BOLD = "CONTROLS_BOLD";
            public const string CONTROLS_CANCEL = "CONTROLS_CANCEL";
            public const string CONTROLS_CLOSE = "CONTROLS_CLOSE";
            public const string CONTROLS_COLUMNS_TO_BE_DISPLAYED = "CONTROLS_COLUMNS_TO_BE_DISPLAYED";
            public const string CONTROLS_CONFIGURATION_FILE = "CONTROLS_CONFIGURATION_FILE";
            public const string CONTROLS_CUT = "CONTROLS_CUT";
            public const string CONTROLS_DATAEXPORT = "CONTROLS_DATAEXPORT";
            public const string CONTROLS_DATAIMPORT = "CONTROLS_DATAIMPORT";
            public const string CONTROLS_DELETE = "CONTROLS_DELETE";
            public const string CONTROLS_DISPLAY_COLUMNS = "CONTROLS_DISPLAY_COLUMNS";
            public const string CONTROLS_EXPORT = "CONTROLS_EXPORT";
            public const string CONTROLS_EXPORTED_FIELD = "CONTROLS_EXPORTED_FIELD";
            public const string CONTROLS_FIELD_MAPPING = "CONTROLS_FIELD_MAPPING";
            public const string CONTROLS_FIELD_SEPARATOR = "CONTROLS_FIELD_SEPARATOR";
            public const string CONTROLS_FONT = "CONTROLS_FONT";
            public const string CONTROLS_FONTSTYLE = "CONTROLS_FONTSTYLE";
            public const string CONTROLS_FORE_COLOUR = "CONTROLS_FORE_COLOUR";
            public const string CONTROLS_FREEZE_COLUMNS = "CONTROLS_FREEZE_COLUMNS";
            public const string CONTROLS_HEADER = "CONTROLS_HEADER";
            public const string CONTROLS_HIDE_COLUMNS = "CONTROLS_HIDE_COLUMNS";
            public const string CONTROLS_IMPORT = "CONTROLS_IMPORT";
            public const string CONTROLS_ITALIC = "CONTROLS_ITALIC";
            public const string CONTROLS_LINE = "CONTROLS_LINE";
            public const string CONTROLS_LOAD = "CONTROLS_LOAD";
            public const string CONTROLS_M = "CONTROLS_M";
            public const string CONTROLS_MAIN_COLOR = "CONTROLS_MAIN_COLOR";
            public const string CONTROLS_OK = "CONTROLS_OK";
            public const string CONTROLS_OPEN_LICENSE = "CONTROLS_OPEN_LICENSE";
            public const string CONTROLS_OTHER = "CONTROLS_OTHER";
            public const string CONTROLS_PASTE = "CONTROLS_PASTE";
            public const string CONTROLS_PREVIEW = "CONTROLS_PREVIEW";
            public const string CONTROLS_SAMPLE = "CONTROLS_SAMPLE";
            public const string CONTROLS_SAVE = "CONTROLS_SAVE";
            public const string CONTROLS_SAVE_FILE = "CONTROLS_SAVE_FILE";
            public const string CONTROLS_SELECT_ALL = "CONTROLS_SELECT_ALL";
            public const string CONTROLS_SELECT_NONE = "CONTROLS_SELECT_NONE";
            public const string CONTROLS_SIZE = "CONTROLS_SIZE";
            public const string CONTROLS_SORT_ASCENDING = "CONTROLS_SORT_ASCENDING";
            public const string CONTROLS_SORT_DESCENDING = "CONTROLS_SORT_DESCENDING";
            public const string CONTROLS_STRIKEOUT = "CONTROLS_STRIKEOUT";
            public const string CONTROLS_SYMBOL = "CONTROLS_SYMBOL";
            public const string CONTROLS_TABLE_FIELDS = "CONTROLS_TABLE_FIELDS";
            public const string CONTROLS_UNDERLINE = "CONTROLS_UNDERLINE";
            public const string CONTROLS_UNFREEZE_ALL_COLUMNS = "CONTROLS_UNFREEZE_ALL_COLUMNS";
            public const string CONTROLS_UPDATE_RECORDS = "CONTROLS_UPDATE_RECORDS";
            public const string DATA_LOAD_FAIL = "DATA_LOAD_FAIL";
            public const string EXCEL_EXPORT_FAIL = "EXCEL_EXPORT_FAIL";
            public const string EXCEL_LOAD_FAIL = "EXCEL_LOAD_FAIL";
            public const string EXPORT_LEGEND = "EXPORT_LEGEND";
            public const string IMPORT_LEGEND = "IMPORT_LEGEND";
            public const string LABEL_ACTIVITY = "LABEL_ACTIVITY";
            public const string LABEL_CHANNEL_INDEX = "LABEL_CHANNEL_INDEX";
            public const string LABEL_END_BREAK = "LABEL_END_BREAK";
            public const string LABEL_END_COLOR = "LABEL_END_COLOR";
            public const string LABEL_FIELD = "LABEL_FIELD";
            public const string LABEL_FREQUENCY_BAND = "LABEL_FREQUENCY_BAND";
            public const string LABEL_FREQUENCY_SWITCH = "LABEL_FREQUENCY_SWITCH";
            public const string LABEL_INTERVAL = "LABEL_INTERVAL";
            public const string LABEL_LENGTH = "LABEL_LENGTH";
            public const string LABEL_START_BREAK = "LABEL_START_BREAK";
            public const string LABEL_START_COLOR = "LABEL_START_COLOR";
            public const string LONGITUDE_OR_LATITUDE = "LONGITUDE_OR_LATITUDE";
            public const string MAX_VALUE = "MAX_VALUE";
            public const string MIN_VALUE = "MIN_VALUE";
            public const string NAME_EMPTY = "NAME_EMPTY";
            public const string NAME_END_ERROR = "NAME_END_ERROR";
            public const string NAME_LENGTH_ERROR = "NAME_LENGTH_ERROR";
            public const string NAME_REGEX_ERROR = "NAME_REGEX_ERROR";
            public const string NEIGHBOR_CELL = "NEIGHBOR_CELL";
            public const string POINT_ANALYSIS = "POINT_ANALYSIS";
            public const string TASK_EXCUTING = "TASK_EXCUTING";
        }
    }
}


