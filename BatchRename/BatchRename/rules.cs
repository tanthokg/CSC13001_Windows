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
		string IRuleHandler.getRuleName() {
			return "lowercase";
		}
		bool IRuleHandler.isEditable() {
			return false;
		}

		string IRuleHandler.process(string ObjectName) {
			string result = ObjectName.ToLower();
			result = result.Replace(" ", "");
			return result;
		}
	}
	class Rule_uppercase : Rule, IRuleHandler
	{
		string IRuleHandler.getRuleName() {
			return "UPPERCASE";
		}
		bool IRuleHandler.isEditable() {
			return false;
		}

		string IRuleHandler.process(string ObjectName) {
			string result = ObjectName.ToUpper();
			return result;
		}
	}

	class Rule_addPrefix : Rule, IRuleHandler {
		string IRuleHandler.getRuleName()
		{
			return "Add Prefix";
		}

		void IRuleHandler.setParameter(ruleParemeters paremeters)
		{
			this.paremeters = paremeters;	
		}

		bool IRuleHandler.isEditable() { return true; }

		string IRuleHandler.process(string ObjectName) {
			return  this.paremeters.outputStrings + ObjectName;
		}
				
	}

	class Rule_addSuffix : Rule, IRuleHandler {
		string IRuleHandler.getRuleName()
		{
			return "Add Suffix";
		}

		void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
		{
			this.paremeters = ruleParemeters;
		}

		bool IRuleHandler.isEditable() { return true; }

		string IRuleHandler.process(string ObjectName) {
			return ObjectName + this.paremeters.outputStrings; 
		}
	}
	class Rule_improperSpaces : Rule, IRuleHandler {
		
		private List<String> target = new List<String>();
		private string replace = "";
		string IRuleHandler.getRuleName()
		{
			return "Trailing spaces at the end/begining";
		}
		bool IRuleHandler.isEditable() { return false; }

		string IRuleHandler.process(string ObjectName) {
			Regex rg = new Regex(@"^\s+|\s+$");
			return rg.Replace(ObjectName, ""); 
		}
	}
	class Rule_Replace : Rule, IRuleHandler {
		string IRuleHandler.getRuleName()
		{
			return "Replace";
		}
		bool IRuleHandler.isEditable() { return true; }

		void IRuleHandler.setParameter(ruleParemeters ruleParemeters) { 
			this.paremeters= ruleParemeters;
		}

		string IRuleHandler.process(string ObjectName) {
			string result = ObjectName;
			this.paremeters.inputStrings.ForEach(s =>
			{
				result.Replace(s[0], this.paremeters.outputStrings[0]);
			});

			return result;
		}
	}
	class Rule_PascalCase : Rule, IRuleHandler {

		private string upperCaseFirstLetter(string str)
		{
			if (str.Length == 0)
				return str;
			return str.ToUpper()[0] + str.Substring(1);
		}

		string IRuleHandler.getRuleName()
		{
			return "Replace";
		}
		bool IRuleHandler.isEditable() { return true; }

		void IRuleHandler.setParameter(ruleParemeters ruleParemeters)
		{
			this.paremeters = ruleParemeters;
		}

		string IRuleHandler.process(string ObjectName) {
			string[] result = ObjectName.Split(this.paremeters.inputStrings[0]);

			StringBuilder stringBuilder	= new StringBuilder();	

			foreach (string str in result) {
				stringBuilder.Append(upperCaseFirstLetter(str));
			}

			return stringBuilder.ToString();
		}
	}




	

}
