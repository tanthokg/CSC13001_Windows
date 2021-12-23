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
	class Rule_improperSpaces : Rule, IRuleHandler
    {

        public override string ToString()
        {
            return "Trailing spaces at the end/begining";
        }
        bool IRuleHandler.isEditable() { return false; }
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

            Regex rg = new Regex(@"^\s+|\s+$");
            if (isFileType)
                return rg.Replace(fileName, "") + "." + extension;
            return rg.Replace(fileName, "");
        }
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}
        IRuleHandler IRuleHandler.getClone()
		{
            Rule_improperSpaces clone = new Rule_improperSpaces();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}
    }
}
