using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManagerWpf.Models
{
    public class TaskManagerProcess
    {
        public string TMProcessId { get; set; }
        public string TMProcessName { get; set; }
        public string TMProcessCpuUsage { get; set; }
        public string TMProcessMemoryUsage { get; set; }
        public string TMPriority { get; set; }

        public string TMPath { get; set; }

        public TaskManagerProcess()
        {
            TMProcessId = "Process Id";
            TMProcessName = "Process Name";
            TMProcessCpuUsage = "Cpu Usage";
            TMProcessMemoryUsage = "Memory Usage";
            TMPriority = "Priority";
            TMPath = "Path";
        }

        public override string ToString()
        {
            return TMProcessId + TMProcessName + TMProcessCpuUsage + TMProcessMemoryUsage + TMPriority + TMPath;
        }
    }
}
