using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SimpleTaskManagerWpf.Models;

namespace SimpleTaskManagerWpf.Services
{
    public class CoreProcessesService : IProcessesService
    {


        public IEnumerable<TaskManagerProcess> GetAllProcesses()
        {

            LinkedList<TaskManagerProcess> taskManagerProcesses = new LinkedList<TaskManagerProcess>();

            foreach (Process p in Process.GetProcesses())
            {
                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName, true);

                try
                {
                    float cpu = cpuCounter.NextValue();
                    double memory = p.PrivateMemorySize64 / 1000000;


                    TaskManagerProcess taskManagerProcess = new TaskManagerProcess();
                    taskManagerProcess.TMProcessId = p.Id.ToString();
                    taskManagerProcess.TMProcessName = p.ProcessName;                    
                    try {taskManagerProcess.TMPriority = p.PriorityClass.ToString();}catch(Exception ){taskManagerProcess.TMPriority = "Unidentified";}
                    try{taskManagerProcess.TMPath = p.MainModule.FileName.ToString();}catch (Exception){taskManagerProcess.TMPath = "Unidentified";}

                    //TMProcessCpuUsage = string.Format("{0.00}", cpuCounter.NextValue()).ToString() + "%",
                    //TMProcessMemoryUsage = string.Format("{0.00}", ((p.PrivateMemorySize64) / sumMemory) * 100) + "%",
                    taskManagerProcess.TMProcessCpuUsage = cpu.ToString() + "%";
                    taskManagerProcess.TMProcessMemoryUsage = memory.ToString().Substring(0, 3) + " MB";

                    //taskManagerProcess.TMProcessDescription = p.MainModule.FileVersionInfo.FileDescription;

                    taskManagerProcesses.AddLast(taskManagerProcess);
                }
                catch (Exception e)
                {
                    continue;
                }

            }

            return taskManagerProcesses;
        }

        public void KillProcess(int Id)
        {
            Process p = Process.GetProcessById(Id);
            p.Kill();
        }

        public void StartProcess(string processName)
        {
           Process.Start(processName);
        }
    }
}
