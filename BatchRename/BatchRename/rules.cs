using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ruleHandler;

namespace BatchRename
{
    class Rule_lowercase : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "lowercase";
        }
        public override string ToString()
        {
            return "lowercase";
        }
        bool IRuleHandler.isEditable()
        {
            return false;
        }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string result = fileName;
            result = result.ToLower();
            result = result.Replace(" ", "");
            if (isFileType)
                return result + "." + extension;
            return result;
        }
    }
    class Rule_uppercase : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "UPPERCASE";
        }
        public override string ToString()
        {
            return "UPPERCASE";
        }
        bool IRuleHandler.isEditable()
        {
            return false;
        }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string result = fileName.ToUpper();
            if (isFileType)
                return result + "." + extension;
            return result;
        }
    }

    class Rule_addPrefix : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "Add Prefix";
        }


        public override string ToString()
        {
            if (paremeters.outputStrings.Length == 0)
                return "Add Prefix";
            return "Add Prefix: " + paremeters.outputStrings;
        }

        void IRuleHandler.setParameter(ruleParemeters paremeters)
        {
            this.paremeters = paremeters;
        }

        bool IRuleHandler.isEditable() { return true; }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return this.paremeters.outputStrings + fileName + "." + extension;
            return this.paremeters.outputStrings + fileName;
        }

    }

    class Rule_addSuffix : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "Add Suffix";
        }

        public override string ToString()
        {
            if (paremeters.outputStrings.Length == 0)
                return "Add Suffix";
            return "Add Suffix: " + paremeters.outputStrings;
        }

        void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
        {
            this.paremeters = ruleParemeters;
        }

        bool IRuleHandler.isEditable() { return true; }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            if (isFileType)
                return fileName + this.paremeters.outputStrings + "." + extension;
            return fileName + this.paremeters.outputStrings;
        }
    }
    class Rule_improperSpaces : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "Trailing spaces at the end/begining";
        }

        public override string ToString()
        {
            return "Trailing spaces at the end/begining";
        }
        bool IRuleHandler.isEditable() { return false; }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            Regex rg = new Regex(@"^\s+|\s+$");
            if (isFileType)
                return rg.Replace(fileName, "") + "." + extension;
            return rg.Replace(fileName, "");
        }
    }
    class Rule_Replace : Rule, IRuleHandler
    {
        string IRuleHandler.getRuleName()
        {
            return "Replace";
        }

        public override string ToString()
        {
            return "Replace";
        }

        bool IRuleHandler.isEditable() { return true; }

        void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
        {
            this.paremeters = ruleParemeters;
        }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string result = fileName;

            this.paremeters.inputStrings.ForEach(s =>
            {
                result = result.Replace(s[0], this.paremeters.outputStrings[0]);
            });

            if (isFileType)
                return result + "." + extension;
            return result;
        }
    }
    class Rule_PascalCase : Rule, IRuleHandler
    {

        private string upperCaseFirstLetter(string str)
        {
            if (str.Length == 0)
                return str;
            return str.ToUpper()[0] + str.Substring(1);
        }

        string IRuleHandler.getRuleName()
        {
            return "PascalCase";
        }

        public override string ToString()
        {
            return "PascalCase";
        }
        bool IRuleHandler.isEditable() { return true; }

        void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
        {
            this.paremeters = ruleParemeters;
        }

        string IRuleHandler.process(string ObjectName, bool isFileType)
        {
            string[] parts = ObjectName.Split('.');
            string extension = parts[^1];
            string fileName;
            if (isFileType)
                fileName = string.Join("", parts.SkipLast(1));
            else
                fileName = ObjectName;

            string[] result = fileName.Split(this.paremeters.inputStrings[0]);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in result)
            {
                stringBuilder.Append(upperCaseFirstLetter(str));
            }

            if (isFileType)
                return stringBuilder.ToString() + "." + extension;
            return stringBuilder.ToString();
        }
    }






}
