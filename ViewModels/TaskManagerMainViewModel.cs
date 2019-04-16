using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SimpleTaskManagerWpf.Models;
using SimpleTaskManagerWpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SimpleTaskManagerWpf.ViewModels
{
    public class TaskManagerMainViewModel : ViewModelBase
    {
        public ObservableCollection<TaskManagerProcess> ObservableProcesses {
            get { return observableProcesses; } set { Set(ref observableProcesses, value);}
        }
        private ObservableCollection<TaskManagerProcess> observableProcesses;

        public string SearchedProcess { get { return searchedProcess; } set { Set(ref searchedProcess, value); } }
        private string searchedProcess;

        public string ProcessToStart { get { return processToStart; } set { Set(ref processToStart, value); } }
        private string processToStart;
        

        private Visibility visibility;
        public Visibility Visibility { get { return visibility; } set { Set(ref visibility, value); } }

        IProcessesService iProcessesService;

        public RelayCommand<String> ButtonKillProcess { get { return buttonKillProcess; } set { Set(ref buttonKillProcess, value);} }
        private RelayCommand<String> buttonKillProcess;

        public RelayCommand SearchProcess { get { return searchProcess; } set { Set(ref searchProcess, value); } }
        private RelayCommand searchProcess;

        public RelayCommand StartProcess { get { return startProcess; } set { Set(ref startProcess, value); } }
        private RelayCommand startProcess;

        private Timer timer;

        public TaskManagerMainViewModel()
        {
            SearchedProcess = "";
            ProcessToStart = "Input your process to start and press enter";
            iProcessesService = new CoreProcessesService();
            ObservableProcesses = new ObservableCollection<TaskManagerProcess>();
            ObservableProcesses.Add(new TaskManagerProcess());
            foreach (TaskManagerProcess tmp in iProcessesService.GetAllProcesses())
                ObservableProcesses.Add(tmp);

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
            {
                LinkedList<TaskManagerProcess> taskManagerProcesses = new LinkedList<TaskManagerProcess>(iProcessesService.GetAllProcesses());

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ObservableProcesses = new ObservableCollection<TaskManagerProcess>();
                    ObservableProcesses.Add(new TaskManagerProcess());
                    taskManagerProcesses.ToList().ForEach(x => {
                        if(x.ToString().ToLower().Contains(SearchedProcess.ToLower()))
                            ObservableProcesses.Add(x);
                    });

                });             

            });



            timer.Interval = 5000;
            timer.Start();

            SearchProcess = new RelayCommand(() => {
                ObservableProcesses.Where(x => !x.ToString().ToLower().Contains(SearchedProcess.ToLower()) && x != ObservableProcesses.ElementAt(0)).
                ToList().All(x => ObservableProcesses.Remove(x));
            });

            StartProcess = new RelayCommand(() => {
                try
                {
                    iProcessesService.StartProcess(ProcessToStart);
                }
                catch(Exception e)
                {
                    MessageBox.Show("No such process");
                }
            });


            ButtonKillProcess = new RelayCommand<String>((String str) => 
            {
                try
                {
                    iProcessesService.KillProcess(Int32.Parse(str));
                    ObservableProcesses.Where(x => x.TMProcessId.Equals(str)).ToList().All(x => ObservableProcesses.Remove(x));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString());
                    ObservableProcesses.Where(x => x.TMProcessId.Equals(str)).ToList().All(x => ObservableProcesses.Remove(x));
                }

            });

        }
    }
}
