using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using ToDoList.Models;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for WindowMessageDisplay.xaml
    /// </summary>
    public partial class WindowMessageDisplay : Window
    {
        private DispatcherTimer closeTimer;
        private DispatcherTimer snoozeTimer;
        public ObservableCollection<TaskItem> Tasks { get; set; }

        public WindowMessageDisplay()
        {
            InitializeComponent();
            InitializeSnoozeTimer();
            closeTimer = new DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromSeconds(30); // Set the interval to 5 seconds
            closeTimer.Tick += CloseTimer_Tick;
        }

        private void InitializeSnoozeTimer()
        {
            snoozeTimer = new DispatcherTimer();
            snoozeTimer.Tick += SnoozeTimer_Tick;
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Close();
        }

        public void SetMessage(string message, ObservableCollection<TaskItem> tasks)
        {
            MessageContent.Text = message;
            Tasks = tasks;
            closeTimer.Start();
           

        }
        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SnoozeButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement snooze functionality (e.g., hide for a specific period)
            this.Hide();
            snoozeTimer.Interval = TimeSpan.FromSeconds(20); // Set snooze duration (20 seconds in this example)
            snoozeTimer.Start();
            this.Close();
        }
        private void SnoozeTimer_Tick(object sender, EventArgs e)
        {
            // Show the window again and stop the timer
            snoozeTimer.Stop();
            this.Show();
        }
        private void ClearCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = Tasks.Count - 1; i >= 0; i--)
            {
                if (Tasks[i].IsComplete)
                {
                    Tasks.RemoveAt(i);
                }
            }
            this.Close();
        }
    }
}
