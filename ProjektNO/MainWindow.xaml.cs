using Antlr4.Runtime;
using Microsoft.Win32;
using ProjektNO.Halstead;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektNO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IDictionary<string, int> _operatorToCount;
        private IDictionary<string, int> _operandToCount;
        private IDictionary<string, object> _metricToValue;
        public IDictionary<string, int> OperatorToCount
        {
            get
            {
                return _operatorToCount;
            }
            set
            {
                _operatorToCount = value;
                OnPropertyRaised(nameof(OperatorToCount));
            }
        }
        public IDictionary<string, int> OperandToCount
        {
            get
            {
                return _operandToCount;
            }
            set
            {
                _operandToCount = value;
                OnPropertyRaised(nameof(OperandToCount));
            }
        }

        public IDictionary<string, object> MetricToValue
        {
            get
            {
                return _metricToValue;
            }
            set
            {
                _metricToValue = value;
                OnPropertyRaised(nameof(MetricToValue));
            }
        }

        public bool SkipDeclarations
        {
            get
            {
                return TokensExtractor.SkipDeclarations;
            }
            set
            {
                TokensExtractor.SkipDeclarations = value;
                CalculateMetrics();
                OnPropertyRaised(nameof(SkipDeclarations));
            }
        }

        private string _source;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private void SourceFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Kod źródłowy w C (*.c)|*.c";
            if (openFileDialog.ShowDialog() == true)
            {
                _source = File.ReadAllText(openFileDialog.FileName);
                sourceFile.Text = _source;
                CalculateMetrics();
            }
        }

        private void CalculateMetrics()
        {
            if (_source == null)
            {
                return;
            }
            AntlrInputStream inputStream = new AntlrInputStream(_source);
            var tokensExtractor = new TokensExtractor(inputStream);
            var dicionariesCreator = new DictionariesCreator(tokensExtractor);
            OperatorToCount = dicionariesCreator.OperatorToCount;
            OperandToCount = dicionariesCreator.OperandToCount;
            var halstead = new HalsteadCalculator(dicionariesCreator.OperatorToCount, dicionariesCreator.OperandToCount);
            Visualize(halstead);
        }

        private void Visualize(HalsteadCalculator halstead)
        {
            var dictionary = new Dictionary<string, object>();
            var methods = halstead.GetType().GetMethods();
            methods = methods.Where(m => !m.DeclaringType.Name.Equals("Object")).ToArray();
            foreach (var method in methods)
            {
                var metricName = method.Name;
                metricName = metricName.Substring(4);
                var returnType = method.ReturnType;
                var value = method.Invoke(halstead, null);
                dictionary.Add(metricName, value);
            }
            MetricToValue = dictionary;
        }
    }
}
