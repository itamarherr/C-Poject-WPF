using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;
using System;
using System.Windows.Threading;
using ToDoList.Models;
using Common;
using System.Windows.Media.Animation;

namespace ToDoList;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler ToManyTasks;
    private ObservableCollection<TaskItem> tasks;

    private readonly string tasksFilePath = System.IO.Path.Combine(
     System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
     "tasks.json");
  
    public List<string> adviserMessages = new List<string>
         {
        "Note that too many undone tasks are not recommended.",
        "what the point of making to do list of tasks if you not gonna complete non of them?.",
        "I hope that is just for checking, because if not this is pointless!!!."
         };
    public ObservableCollection<TaskItem> Tasks
    {
        get { return tasks; }
        set
        {
            tasks = value;
            OnPropertyChanged(nameof(Tasks));

        }
    }


    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Tasks = new ObservableCollection<TaskItem>();
        LoadTasks();
        ToDoDataGrid.ItemsSource = Tasks;
        ToManyTasks += HandleToManyTasks;


    }

    private void HandleToManyTasks(object? sender, EventArgs e)
    {
        WindowMessageDisplay windowMessageDisplay = new WindowMessageDisplay();
        string message = tasks.Count == 12 ? adviserMessages[2] :
                              tasks.Count == 11 ? adviserMessages[1] :
                              adviserMessages[0];

        windowMessageDisplay.SetMessage(message, tasks);
        windowMessageDisplay.Show();


    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void LoadTasks()
    {

        if (File.Exists(tasksFilePath))
        {
            try
            {
                string json = File.ReadAllText(tasksFilePath);
                var loadedTasks = JsonSerializer.Deserialize<ObservableCollection<TaskItem>>(json);
                if (loadedTasks != null)
                {
                    Tasks = loadedTasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading or deserializing tasks: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show($"No existing tasks file found at {tasksFilePath}. A new file will be created when you add tasks.", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void SaveTasks()
    {
        try
        {

            string directory = System.IO.Path.GetDirectoryName(tasksFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(tasksFilePath, json);

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskTextBox.Text) && CategoryComboBox.SelectedItem != null)
        {

            TaskItem newTask = new TaskItem
            {
                Description = TaskTextBox.Text,
                Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString()
            };
            tasks.Add(newTask);
            SaveTasks();
            if (tasks.Count > 9)
            {
                OnToManyTasks(sender, e);
            }

        }
     
        ComboBoxItem selectedItem = CategoryComboBox.SelectedItem as ComboBoxItem;
        if (selectedItem != null)
        {
            string selectedCategory = selectedItem.Content.ToString();
            SortDataGridByCategory(selectedCategory);
        }
    }
    private void RemoveTask_Click(Object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is TaskItem selectedTask)
        {
            tasks.Remove(selectedTask);
            SaveTasks();

        }
    }
    private void EditTask_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is TaskItem selectedTask)
        {
            TaskTextBox.Text = selectedTask.Description;
            CategoryComboBox.SelectedItem = CategoryComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == selectedTask.Category);
            Tasks.Remove(selectedTask);
            SaveTasks();
        }
    }


    private void SortDataGridByCategory(string selectedCategory)
    {
        if (selectedCategory == "All")
        {
            ToDoDataGrid.ItemsSource = Tasks;
        }
        else
        {
            var sortedTasks = Tasks.Where(task => task.Category == selectedCategory);
            ToDoDataGrid.ItemsSource = new ObservableCollection<TaskItem>(sortedTasks);
        }
    }

    private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = CategoryComboBox.SelectedItem as ComboBoxItem;
        if (selectedItem != null)
        {
            string selectedCategory = selectedItem.Content.ToString();
            SortDataGridByCategory(selectedCategory);
        }
    }

    public void OnToManyTasks(Object sender, EventArgs e)
    {
        ToManyTasks?.Invoke(this, EventArgs.Empty);
    }

  
}