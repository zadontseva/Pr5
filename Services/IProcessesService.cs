using SimpleTaskManagerWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManagerWpf.Services
{
    interface IProcessesService
    {
        void KillProcess(int Id);
        void StartProcess(string processName);
        IEnumerable<TaskManagerProcess> GetAllProcesses();

    }
}
