namespace UPlan.Common.Validate
{
    using UPlan.Common.GlobalResource;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public static class DataValidate
    {
        public static bool IsBoolean(ref string info, string name, object inputobj, out bool output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!bool.TryParse(str.Trim(), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            return true;
        }

        public static bool IsCorrectName(ref string info, string name, string CurrentName, out string outPut)
        {
            if ("".Equals(CurrentName))
            {
                info = name + " " + CommonResource.NAME_EMPTY;
                outPut = string.Empty;
                return false;
            }
            return ValidateName(ref info, name, CurrentName, out outPut);
        }

        public static bool IsCurrency(ref string info, string CurrencyString)
        {
            string[] strArray = CurrencyString.Split(new char[] { ',' });
            string str = "";
            foreach (string str2 in strArray)
            {
                str = str + str2;
            }
            CurrencyString = str;
            bool flag = true;
            try
            {
                double.Parse(CurrencyString);
            }
            catch
            {
                info = CommonResource.COMMON_INVALID_FIELD + CurrencyString;
                flag = false;
            }
            if (flag)
            {
                string[] strArray2 = CurrencyString.Split(new char[] { '.' });
                string s = "";
                if (strArray2.Length > 1)
                {
                    if (strArray2[1].Length > 4)
                    {
                        flag = false;
                    }
                    s = strArray2[0] + strArray2[1];
                }
                else
                {
                    s = strArray2[0];
                }
                try
                {
                    long.Parse(s);
                }
                catch
                {
                    info = CommonResource.COMMON_INVALID_FIELD + CurrencyString;
                    flag = false;
                }
            }
            return flag;
        }

        public static bool IsCurrency(ref string info, string name, string CurrencyString, out double outPut)
        {
            string[] strArray = CurrencyString.Split(new char[] { ',' });
            string s = "";
            foreach (string str2 in strArray)
            {
                s = s + str2;
            }
            CurrencyString = s;
            bool flag = true;
            if (!double.TryParse(CurrencyString, out outPut))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                flag = false;
            }
            if (flag && !double.TryParse(s, out outPut))
            {
                info = CommonResource.COMMON_INVALID_FIELD + CurrencyString;
                flag = false;
            }
            return flag;
        }

        public static bool IsDateTime(ref string info, string TimeString)
        {
            bool flag = true;
            try
            {
                DateTime time = DateTime.Parse(TimeString);
            }
            catch
            {
                info = CommonResource.COMMON_INVALID_FIELD + TimeString;
                flag = false;
            }
            return flag;
        }

        public static bool IsDateTime(ref string info, string name, object inputobj, out DateTime outPut)
        {
            string s = (inputobj == null) ? "" : inputobj.ToString();
            if (!DateTime.TryParse(s, out outPut))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            return true;
        }

        public static bool IsDiskSpaceEnough(string path, long needSize)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }
                string pathRoot = Path.GetPathRoot(path);
                if (string.IsNullOrEmpty(pathRoot))
                {
                    return false;
                }
                DriveInfo info = new DriveInfo(pathRoot);
                if (!info.IsReady)
                {
                    return false;
                }
                return (info.AvailableFreeSpace > needSize);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDouble(ref string info, string name, object inputobj, out double output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!double.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            return true;
        }

        public static bool IsDouble(ref string info, string name, object inputobj, double minvalue, double maxvalue, out double output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!double.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((output < minvalue) || (output > maxvalue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_OVER_RANGE, "[", minvalue, "~", maxvalue, "]: ", name });
                return false;
            }
            return true;
        }

        public static bool IsFloat(ref string info, string name, object inputobj, out float output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (str.Equals("-Infinity"))
            {
                output = float.NegativeInfinity;
                return true;
            }
            if (!float.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            return true;
        }

        public static bool IsFloat(ref string info, string name, object inputobj, int res, out float output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!IsFloat(ref info, name, str, out output))
            {
                return false;
            }
            output = float.Parse(((float)output).ToString("f" + res));
            return true;
        }

        public static bool IsFloat(ref string info, string name, object inputobj, float minvalue, float maxvalue)
        {
            float num;
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!float.TryParse(str.Trim().Replace(",", ""), out num))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((num < minvalue) || (num > maxvalue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_INVALID_FIELD, name, ", ", CommonResource.COMMON_IN_RANGE, " [", minvalue, ",", maxvalue, "]" });
                return false;
            }
            return true;
        }

        public static bool IsFloat(ref string info, string name, object inputobj, float minvalue, float maxvalue, out float output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!float.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((output < minvalue) || (output > maxvalue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_OVER_RANGE, "[", minvalue, "~", maxvalue, "]: ", name });
                return false;
            }
            return true;
        }

        public static bool IsInRange(ref string info, string name, int value, int minValue, int maxValue)
        {
            if ((value < minValue) || (value > maxValue))
            {
                info = string.Format(CommonResource.COMMON_BIGGERTHAN_ZERO, name, minValue, maxValue);
                return false;
            }
            return true;
        }

        public static bool IsInt(ref string info, string name, object inputobj, out int output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!int.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            return true;
        }

        public static bool IsInt(ref string info, string name, object inputobj, int minValue, int maxValue)
        {
            int num;
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!int.TryParse(str.Trim().Replace(",", ""), out num))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((num < minValue) || (num > maxValue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_INVALID_FIELD, name, ", ", CommonResource.COMMON_IN_RANGE, " [", minValue, ",", maxValue, "]" });
                return false;
            }
            return true;
        }

        public static bool IsInt(ref string info, string name, object inputobj, int minvalue, int maxvalue, out int output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!int.TryParse(str.Trim().Replace(",", ""), out output))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((output < minvalue) || (output > maxvalue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_OVER_RANGE, "[", minvalue, "~", maxvalue, "]: ", name });
                return false;
            }
            return true;
        }

        public static bool IsIntBiggerThanMin(ref string info, string name, object inputobj, int minValue, int maxValue)
        {
            int num;
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (!int.TryParse(str.Trim().Replace(",", ""), out num))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            if ((num <= minValue) || (num > maxValue))
            {
                info = string.Concat(new object[] { CommonResource.COMMON_INVALID_FIELD, name, ", ", CommonResource.COMMON_IN_RANGE, " (", minValue, ",", maxValue, "]" });
                return false;
            }
            return true;
        }

        public static bool isMinLessThanMax(ref string info, string MiniName, string MaxName, string min, string max)
        {
            double num = double.Parse(min);
            double num2 = double.Parse(max);
            bool flag = true;
            if (num > num2)
            {
                flag = false;
                info = string.Format(CommonResource.COMMON_LESS_THAN, MiniName, MaxName);
            }
            return flag;
        }

        public static bool IsPositive(ref string info, string name, int value)
        {
            return IsPositive(ref info, name, Convert.ToSingle(value));
        }

        public static bool IsPositive(ref string info, string name, float value)
        {
            if (value < 0f)
            {
                info = name + CommonResource.COMMON_NOT_NEGATIVE;
                return false;
            }
            return true;
        }

        public static bool IsUserProfileFloat(ref string info, string name, object inputobj, float minvalue, float maxvalue)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (inputobj != null)
            {
                float num;
                if (!float.TryParse(str.Trim().Replace(",", ""), out num))
                {
                    info = CommonResource.COMMON_INVALID_FIELD + name;
                    return false;
                }
                if ((num < minvalue) || (num > maxvalue))
                {
                    info = string.Concat(new object[] { CommonResource.COMMON_INVALID_FIELD, name, ", ", CommonResource.COMMON_IN_RANGE, " [", minvalue, ",", maxvalue, "]" });
                    return false;
                }
            }
            return true;
        }

        public static bool NotEmptyString(object inputobj)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            if (string.IsNullOrEmpty(str) || (str.Trim() == string.Empty))
            {
                return false;
            }
            return true;
        }

        public static bool NotEmptyString(ref string info, string name, object inputobj, out string output)
        {
            string str = (inputobj == null) ? "" : inputobj.ToString();
            output = "";
            if (string.IsNullOrEmpty(str) || (str.Trim() == string.Empty))
            {
                info = CommonResource.COMMON_INVALID_FIELD + name;
                return false;
            }
            output = str.Trim();
            return true;
        }

        private static bool ValidateName(ref string info, string name, string CurrentName, out string outPut)
        {
            string pattern = @"\w";
            string input = CurrentName.Substring(0, 1);
            bool flag = true;
            outPut = CurrentName;
            if (!Regex.IsMatch(input, pattern))
            {
                info = name + " " + CommonResource.NAME_REGEX_ERROR;
                outPut = string.Empty;
                return false;
            }
            if (CurrentName.Length > 100)
            {
                info = name + " " + CommonResource.NAME_LENGTH_ERROR;
                outPut = string.Empty;
                return false;
            }
            if (CurrentName.EndsWith(" "))
            {
                info = name + " " + CommonResource.NAME_END_ERROR;
                outPut = string.Empty;
                flag = false;
            }
            return flag;
        }
    }
}



