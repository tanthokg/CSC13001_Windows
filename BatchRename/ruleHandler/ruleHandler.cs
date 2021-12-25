using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace RuleHandler
{
    public interface IRuleEditor
    {
        public RuleParameter GetParameter();

        public bool? ShowDialog();
    }

    public class Rule
    {
        protected RuleParameter parameter = new RuleParameter();
    }
    public interface IRuleHandler
    {
        public bool IsEditable();
        public string GetRuleType();
        public void SetParameter(RuleParameter ruleParameter)
        {
            return;
        }

        public RuleParameter GetParameter();
        public string Process(string ObjectName, bool isFileType = true);

        public IRuleEditor ParamsEditorWindow();

        public IRuleHandler Clone();
    }

    public class RuleParameter
    {

        public RuleParameter()
        {
            InputStrings = new List<string>();
            OutputStrings = string.Empty;
            Counter = -1;
        }

        public List<string> InputStrings { get; set; }
        public string OutputStrings { get; set; }
        public int Counter { get; set; }
    }

    public class RuleJsonFormat
    {
        public string RuleType { get; set; }
        public List<string> InputStrings { get; set; }
        public string OutputStrings { get; set; }
        public int Counter { get; set; }
    }

    public class FolderFormat
	{
        public string NewName { get; set; }
        public string CurrentName { get; set; }
        public string Path { get; set; }    
        public string result { get; set; }  
	}
    public class FileFormat
	{
        public string NewName { get; set; }
        public string CurrentName { get; set; }
        public string Path { get; set; }    
        public string result { get; set; }  
	}

    public class ProjectFormat
    {
        public List<RuleJsonFormat> rules { get; set; }
        public List<FileFormat> files { get; set; }
        public List<FolderFormat> folders { get; set; }
    }

    public class Util
    {
        public static string checkValid(string str, bool isFileType = true)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "Empty name";

            string[] parts = str.Split('.');
            string extension = parts[^1];
            string name;
            if (isFileType)
                name = string.Join(".", parts.SkipLast(1));
            else
                name = str;

            char[] invalidCharacter =
            {
                '<',
                '>',
                ':',
                '\"',
                '/',
                '\\',
                '|',
                '?',
                '*',
                (char)0,
                (char)1,
                (char)2,
                (char)3,
                (char)4,
                (char)5,
                (char)6,
                (char)7,
                (char)8,
                (char)9,
                (char)10,
                (char)11,
                (char)12,
                (char)13,
                (char)14,
                (char)15,
                (char)16,
                (char)17,
                (char)18,
                (char)19,
                (char)20,
                (char)21,
                (char)22,
                (char)23,
                (char)24,
                (char)25,
                (char)26,
                (char)27,
                (char)28,
                (char)29,
                (char)30,
                (char)31
            };

            string[] invalidName =
            {
                "CON",
                "PRN",
                "AUX",
                "NUL",
                "COM1",
                "COM2",
                "COM3",
                "COM4",
                "COM5",
                "COM6",
                "COM7",
                "COM8",
                "COM9",
                "LPT1",
                "LPT2",
                "LPT3",
                "LPT4",
                "LPT5",
                "LPT6",
                "LPT7",
                "LPT8",
                "LPT9"
            };

            //if contain any forbidden character
            foreach (char c in invalidCharacter)
                if (name.Contains(c))
                    return $"contain \"{c}\"";

            //if the name is forbiddent
            foreach (string s in invalidName)
                if (name.Equals(s))
                    return $"forbidden name \"{s}\"";

            //if end with spaces or dots
            Regex trailingSpacesOrDots = new Regex(@"\s+$|\.+$");

            if (trailingSpacesOrDots.IsMatch(name))
                return "space or dot at the end";

            //if too long
            if (name.Count() > 225)
                return "name too long";

            return "";
        }
    }
}
