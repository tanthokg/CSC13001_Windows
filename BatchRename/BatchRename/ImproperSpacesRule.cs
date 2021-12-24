using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RuleHandler;
using System.Windows.Controls;
using System.Text.Json;

namespace BatchRename
{
	class ImproperSpacesRule : Rule, IRuleHandler
    {

		public ImproperSpacesRule()
		{
            this.parameter = new RuleParameter();
		}
        public override string ToString()
        {
            return "Remove trailing spaces at the end/begining";
        }
        string IRuleHandler.GetRuleType()
		{
            return "ImproperSpacesRule";
		}
        string IRuleHandler.ToJson()
		{
            string RuleType = ((IRuleHandler)this).GetRuleType();
            List<string> InputStrings = this.parameter.InputStrings;
            string OutputStrings = this.parameter.OutputStrings;
            int Counter = this.parameter.Counter;

            RuleJsonFormat format = new RuleJsonFormat
            {
                RuleType = RuleType,
                InputStrings = InputStrings,
                OutputStrings = OutputStrings,
                Counter = Counter,
			};
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(format, options);
		}

        bool IRuleHandler.IsEditable() { return false; }
        RuleParameter IRuleHandler.GetParameter()
		{
            return this.parameter;
		}

        string IRuleHandler.Process(string ObjectName, bool isFileType)
        {
            if(string.IsNullOrEmpty(ObjectName))
                return "";

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
        IRuleEditor IRuleHandler.ParamsEditorWindow()
		{
            return null;
		}
        IRuleHandler IRuleHandler.Clone()
		{
            ImproperSpacesRule clone = new ImproperSpacesRule();
            clone.parameter.InputStrings = this.parameter.InputStrings.Select(x => x.ToString()).ToList();
            clone.parameter.OutputStrings = this.parameter.OutputStrings;
            clone.parameter.Counter = this.parameter.Counter;
            return clone;
		}
    }
}
