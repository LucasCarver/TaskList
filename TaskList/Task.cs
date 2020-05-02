using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList
{
    class Task
    {
        private string assignedMember;
        private DateTime dueDate;
        private string description;
        private bool taskComplete;

        public string AssignedMember
        {
            get
            {
                return assignedMember;
            }
            set
            {
                assignedMember = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public bool TaskComplete
        {
            get
            {
                return taskComplete;
            }
            set
            {
                taskComplete = value;
            }
        }

        public Task()
        {
            description = "default description";
            assignedMember = "default member";
            dueDate = DateTime.Parse("01/01/2020");
            taskComplete = false;
        }

        public Task(string _assignedMember, DateTime _dueDate, string _description)
        {
            assignedMember = _assignedMember;
            dueDate = _dueDate;
            description = _description;
            taskComplete = false;
        }
    }
}