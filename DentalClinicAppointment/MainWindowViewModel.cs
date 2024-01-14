using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace DentalClinicScheduler
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Appointment> appointments;

        public ObservableCollection<Appointment> Appointments
        {
            get { return appointments; }
            set { appointments = value; OnPropertyChanged(nameof(Appointments)); }
        }

        public MainWindowViewModel()
        {
            Appointments = new ObservableCollection<Appointment>();

            try
            {
                using (FileStream fs = new FileStream("data.xml", FileMode.Open))
                {
                    Appointments = (ObservableCollection<Appointment>)new XmlSerializer(typeof(ObservableCollection<Appointment>)).Deserialize(fs);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading data: " + e.Message);
            }
        }

        public void AddAppointment(DateTime date, string time, string patients, string procedures, string notes)
        {

            Appointment appointment = new Appointment(
                GetNextAppointmentId(), // Use the method to get the next ID
                date,
                time,
                patients,
                procedures,
                notes
               );

            Appointments.Add(appointment);
            SaveData();
            OnPropertyChanged(nameof(Appointments));
        }

        public void SaveData()
        {
            // Serialize data to XML files
            try
            {
                using (FileStream fs = new FileStream("data.xml", FileMode.Create))
                {
                    new XmlSerializer(typeof(ObservableCollection<Appointment>)).Serialize(fs, Appointments);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving data: " + e.Message);
            }
        }

        // Add a method to generate the next appointment ID
        private int GetNextAppointmentId()
        {
            // Find the maximum existing appointment ID and increment it
            int maxId = Appointments.Any() ? Appointments.Max(a => a.Id) : 0;
            return maxId + 1;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
