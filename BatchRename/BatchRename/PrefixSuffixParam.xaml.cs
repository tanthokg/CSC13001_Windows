using ruleHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for PrefixSuffixParam.xaml
    /// </summary>
    public partial class PrefixSuffixParam : Window
    {
        public IRuleHandler Rule { get; set; }
        public PrefixSuffixParam(IRuleHandler rule)
        {
            InitializeComponent();
            Rule = rule;
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string str = editTextBox.Text;
            if (str.Length != 0)
            {
                var param = new ruleParemeters() { outputStrings = str };
                Rule.setParameter(param);
                DialogResult = true;
            }
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
