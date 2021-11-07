using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace GraphAlgorithms
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (int.TryParse(vertexNumberInput.Text, out int number))
                graphDataGrid.ItemsSource2D = new double?[number, number];
        }

        private void algList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var alg = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (alg == null) return;

            srcVertexPanel.Visibility = alg == "Bellman–Ford" || alg == "Dijkstra's" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void graphAdjMatrix_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn column = e.Column as DataGridTextColumn;

            Binding binding = column.Binding as Binding;
            binding.Converter = new NullableDoubleToStringConverter();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void vertexNumberInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (!int.TryParse(textBox.Text, out int value))
                value = 0;

            if (value < 0) value = 0;
            else if (value >= 50) value = 50;

            textBox.Text = value.ToString();
            textBox.CaretIndex = textBox.Text.Length;

            if (graphDataGrid != null) graphDataGrid.ItemsSource2D = new double?[value, value];

            if (sourceVertex != null) sourceVertex.Text = "0";
        }

        private void cancelPasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void sourceVertex_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (!int.TryParse(textBox.Text, out int value))
                value = 0;

            if (graphDataGrid == null) return;

            var vertexCount = (graphDataGrid.ItemsSource2D as double?[,]).GetLength(0);

            if (value < 0) value = 0;
            else if (value >= vertexCount && value != 0) value = vertexCount - 1;

            textBox.Text = value.ToString();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void executeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Graph graph = new(graphDataGrid.ItemsSource2D as double?[,]);
                int srcVertex = int.Parse(sourceVertex.Text);

                switch ((algList.SelectedItem as ComboBoxItem).Content as string)
                {
                    case "Bellman–Ford":
                        outputDistances.ItemsSource2D = graph.bellmanFord(srcVertex);
                        break;
                    case "Dijkstra's":
                        outputDistances.ItemsSource2D = graph.dijkstras(srcVertex);
                        break;
                    case "Jonson's":
                        outputDistances.ItemsSource2D = graph.johnsons();
                        break;
                    default:
                        throw new Exception("Unsupported algorithm");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
