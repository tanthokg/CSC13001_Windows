using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ruleHandler;
using System.Windows.Controls;

namespace BatchRename
{
    class Rule_PascalCase : Rule, IRuleHandler
    {
        private string upperCaseFirstLetter(string str)
        {
            if (str.Length == 0)
                return str;
            return str.ToUpper()[0] + str.Substring(1);
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
        ruleParemeters IRuleHandler.GetParemeters()
		{
            return this.paremeters;
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
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}
        
        IRuleHandler IRuleHandler.getClone()
		{
            Rule_PascalCase clone = new Rule_PascalCase();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}
    }
}
