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
	class Rule_Replace : Rule, IRuleHandler
    {

        public override string ToString()
        {
            return "Replace";
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

            string result = fileName;

            this.paremeters.inputStrings.ForEach(s =>
            {
                result = result.Replace(s[0], this.paremeters.outputStrings[0]);
            });

            if (isFileType)
                return result + "." + extension;
            return result;
        }
        IRuleEditor IRuleHandler.parametersEditorWindow()
		{
            return null;
		}
        IRuleHandler IRuleHandler.getClone()
		{
            Rule_Replace clone = new Rule_Replace();
            clone.paremeters.inputStrings = this.paremeters.inputStrings.Select(x => x.ToString()).ToList();
            clone.paremeters.outputStrings = this.paremeters.outputStrings;
            clone.paremeters.counter = this.paremeters.counter;
            return clone;
		}
    }
}
