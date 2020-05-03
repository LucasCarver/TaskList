using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>()
            {
                new Task("Develop the new system", "Joe", DateTime.Parse("12/10/2021")),
                new Task( "Arrange a meeting", "Jessica", DateTime.Parse("11/15/2022")),
                new Task("Create the presentation", "Tony", DateTime.Parse("10/14/2022")),
                new Task("Meet with client", "Maxine", DateTime.Parse("8/10/2020"))
            };


            int selection = 0;
            bool exitCondition = false;
            List<string> menu = new List<string>() {
                new string("List tasks"),
                new string("Add task"),
                new string("Delete task"),
                new string("Mark task complete"),
                new string("Edit task"),
                new string("View tasks due before date"),
                new string("Quit")
            };

            while (!exitCondition)
            {
                tasks = tasks.OrderBy(task => task.DueDate).ToList();
                exitCondition = MakeSelection(selection, tasks, menu);
            }
        }

        public static bool MakeSelection(int selection, List<Task> tasks, List<string> menu)
        {
            bool validInput = false;
            string input;
            while (!validInput)
            {
                DrawMenu(menu);
                Console.WriteLine();
                input = PromptUserInLine("Input menu selection: ");
                selection = SelectListItem(menu, input);
                if (selection != -1)
                {
                    validInput = true;
                }
            }

            switch (selection)
            {
                case 1:
                    DrawTasks(tasks);
                    break;
                case 2:
                    AddTask(tasks);
                    break;
                case 3:
                    DeleteTask(tasks);
                    break;
                case 4:
                    MarkTaskComplete(tasks);
                    break;
                case 5:
                    EditTask(tasks);
                    break;
                case 6:
                    ViewTasksDueBy(tasks);
                    break;
                case 7:
                    return QuitRoutine();
            }

            return false;
        }

        public static bool QuitRoutine()
        {
            return AreYouSure("quit");
        }

        public static void MarkTaskComplete(List<Task> tasks)
        {
            int temp = -1;
            string input;
            bool validInput = false;
            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to mark as completed. The list is empty.\n");
                return;
            }
            while (!validInput)
            {
                DrawTasks(tasks);
                input = PromptUserInLine("Please enter task to mark as completed: ");
                temp = SelectListItem(tasks, input);
                if (temp != -1)
                {
                    validInput = true;
                }
            }
            if (AreYouSure($"mark task \"{temp}\" as completed"))
            {
                tasks[temp - 1].Status = true;
            }
        }

        public static void AddTask(List<Task> tasks)
        {
            string description = AddDescription();
            string assignedMember = AddMember();
            DateTime dueDate = AddDate();
            tasks.Add(new Task(description, assignedMember, dueDate));
            Console.WriteLine($"\nTask \"{description} {assignedMember} {dueDate.ToShortDateString()}\" added successfully.\n");
        }

        public static string AddMember()
        {
            string member = "";
            bool validInput = false;
            while (!validInput)
            {
                member = PromptUserInLine("Please enter assigned team member: ");
                if (IsEmpty(member))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }
                else if (member.Length > 18)
                {
                    Console.WriteLine("Name cannot exceed 18 characters.");
                    continue;
                }
                validInput = true;
            }
            return member;
        }

        public static string AddDescription()
        {
            bool validInput = false;
            string description = "";
            while (!validInput)
            {
                description = PromptUserInLine("Please enter task description: ");
                if (IsEmpty(description))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }
                else if (description.Length > 30)
                {
                    Console.WriteLine("Description cannot 30 characters.");
                    continue;
                }
                validInput = true;
            }
            return description;
        }

        public static bool IsEmpty<T>(List<T> list)
        {
            return list.Count == 0;
        }

        public static void DeleteTask(List<Task> tasks)
        {
            int temp = -1;
            string input;
            bool validInput = false;

            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to delete. The list is empty.\n");
                return;
            }
            while (!validInput)
            {
                DrawTasks(tasks);
                input = PromptUserInLine("Please enter task to delete: ");
                temp = SelectListItem(tasks, input);
                if (temp != -1)
                {
                    validInput = true;
                }
            }
            if (AreYouSure($"delete task \"{temp} {tasks[temp - 1].Description}\""))
            {
                tasks.RemoveAt(temp - 1);
            }

        }

        public static bool AreYouSure(string message)
        {
            string input;
            while (true)
            {
                input = PromptUserInLine($"\nAre you sure you wish to {message}?\n Y/N ").Trim();
                if (input != "Y" && input != "N")
                {
                    Console.WriteLine("Please use Uppercase Y or N only.");
                    continue;
                }
                else if (input == "Y")
                {
                    Console.WriteLine($"{message[0].ToString().ToUpper() + message.Substring(1, message.Length - 1)} success.\n");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Operation canceled.");
                    return false;
                }
            }
        }

        public static string PromptUser(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim();
        }

        public static string PromptUserInLine(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim();
        }

        public static void DrawMenu<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("{0,2} {1,-10}", $"{i + 1}", $"{list[i]}");
            }
        }

        public static void DrawTasks(List<Task> tasks)
        {
            string date;
            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
                return;
            }
            Console.WriteLine();
            string format = "{0,2} {1,-30} {2,-18} {3,-11} {4,-10}";
            Console.WriteLine(format, "#", "Description", "Member", "Status", "Due Date");
            for (int i = 0; i < tasks.Count; i++)
            {
                int index = i + 1;
                Task task = tasks[i];
                date = FormatDate(task);
                string member = task.AssignedMember;
                string description = task.Description;
                string status = IsComplete(task.Status);
                Console.WriteLine(format, index, description, member, status, date);
            }
            Console.WriteLine();
        }

        public static string FormatDate(Task task)
        {
            string date = "";
            if (task.DueDate.Month < 10)
            {
                date += 0;
            }
            date += task.DueDate.Month + "/";
            if (task.DueDate.Day < 10)
            {
                date += 0;
            }
            date += task.DueDate.Day + "/" + task.DueDate.Year;
            return date;
        }

        public static string IsComplete(bool status)
        {
            string temp;
            if (status)
            {
                temp = "COMPLETE";
            }
            else
            {
                temp = "IN PROGRESS";
            }
            return temp;
        }

        public static int SelectListItem<T>(List<T> list, string input)
        {
            int temp;
            try
            {
                if (IsEmpty(input))
                {
                    throw new Exception("Input cannot be empty.");
                }
                else if (!int.TryParse(input, out temp))
                {
                    throw new Exception("Please input a whole number.");
                }
                else if (temp < 1 || temp > list.Count)
                {
                    throw new Exception("Input must be a number from the list.");
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public static void EditTask(List<Task> tasks)
        {
            DrawTasks(tasks);
            string input = PromptUserInLine("Which task would you like to edit: ");
            int temp = SelectListItem(tasks, input);
            DateTime dueDate = AddDate();
            string member = AddMember();
            string description = AddDescription();
            tasks[temp - 1].DueDate = dueDate;
            tasks[temp - 1].AssignedMember = member;
            tasks[temp - 1].Description = description;
        }

        public static bool IsEmpty(string input)
        {
            return input == "";
        }

        public static void ViewTasksDueBy(List<Task> tasks)
        {
            DateTime dueBy;
            string date;
            string member;
            string description;
            string status;

            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
                return;
            }
            dueBy = AddDate();
            Console.WriteLine($"Tasks due before {dueBy.ToShortDateString()}:");
            string format = "{0,2} {1,-30} {2,-18} {3,-11} {4,-10}";
            Console.WriteLine(format, "#", "Description", "Member", "Status", "Due Date");
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[i];
                if (DateTime.Compare(task.DueDate, dueBy) <= 0)
                {
                    date = FormatDate(task);
                    member = task.AssignedMember;
                    description = task.Description;
                    status = IsComplete(task.Status);
                    Console.WriteLine(format, i + 1, description, member, status, date);
                }
            }
            Console.WriteLine();
        }

        public static DateTime AddDate()
        {
            DateTime dueDate = new DateTime();
            bool validInput = false;
            string input;
            while (!validInput)
            {
                input = PromptUserInLine("Please enter a date (mm/dd/yy): ");
                try
                {
                    dueDate = DateTime.Parse(input);
                    validInput = true;
                }
                catch
                {
                    Console.WriteLine("Invalid format.");
                }
            }
            return dueDate;
        }
    }
}