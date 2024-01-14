using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace DentalClinicScheduler
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Appointment> appointments;
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            appointments = viewModel.Appointments;
            dataGridAppointments.ItemsSource = appointments;

            // Initialize time combo box
            for (int hour = 8; hour < 20; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    cmbTime.Items.Add(new DateTime(1, 1, 1, hour, minute, 0));
                }
            }

            // Load data from XML files
            try
            {
                using (FileStream fs = new FileStream("data.xml", FileMode.Open))
                {
                    appointments = (ObservableCollection<Appointment>)new XmlSerializer(typeof(ObservableCollection<Appointment>)).Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading data: " + e.Message);
                appointments = new ObservableCollection<Appointment>();
            }
        }


        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidAppointment())
            {

                viewModel.AddAppointment(
                    datePickerDate.SelectedDate.Value,
                    cmbTime.SelectedItem.ToString(),
                    txtPatient.Text,
                    txtProcedure.Text,
                    txtNotes.Text);

                // Save data to XML files
                viewModel.SaveData();

                // Clear input fields
                datePickerDate.SelectedDate = null;
                cmbTime.SelectedIndex = 0;
                txtPatient.Text = "";
                txtProcedure.Text = "";
                txtNotes.Text = "";
            }
        }

        private bool IsValidAppointment()
        {
            if (datePickerDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.");
                return false;
            }
            if (cmbTime.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a time.");
                return false;
            }
            return true;
        }
    }
}
