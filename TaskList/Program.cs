using System;
using System.Collections.Generic;

namespace TaskList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>()
            {
                    new Task("Joe", DateTime.Parse("12/10/2021"), "Develop the new sytem"),
                     new Task("Jessica", DateTime.Parse("11/15/2022"), "Arrange a meeting"),
                     new Task("Tony", DateTime.Parse("10/14/2022"), "Create the presentation")
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
                DrawMenu(menu);
                exitCondition = MakeSelection(selection, tasks, menu);
            }
        }

        public static bool MakeSelection(int selection, List<Task> tasks, List<string> menu)
        {
            bool validInput = false;
            string input;
            while (!validInput)
            {
                input = PromptUserInLine("Input menu selection: ");
                selection = SelectListItem(menu, input);
                if (selection != -1)
                {
                    validInput = true;
                }
            }

            if (selection == 1)
            {
                DrawTasks(tasks);
            }
            if (selection == 2)
            {
                AddTask(tasks);
            }
            if (selection == 3)
            {
                DeleteTask(tasks);
            }
            if (selection == 4)
            {
                MarkTaskComplete(tasks);
            }
            if (selection == 5)
            {
                EditTask(tasks);
            }
            if (selection == 6)
            {
                ViewTasksDueBy(tasks);
            }
            if (selection == 7)
            {
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
            DateTime dueDate = AddDate();
            string assignedMember = AssignMember();
            string description = SetDescription();
            tasks.Add(new Task(assignedMember, dueDate, description));
            Console.WriteLine($"\nTask \"{tasks.Count} {assignedMember} {dueDate.ToShortDateString()} {description}\" added successfully.\n");
        }


        public static string AssignMember()
        {
            string member = "";
            string input;
            bool validInput = false;
            while (!validInput)
            {
                input = PromptUserInLine("Please enter assigned team member: ");
                try
                {
                    if (IsEmpty(input))
                    {
                        throw new Exception("Task must include assigned team member.");
                    }
                    member = input;
                    validInput = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return member;
        }



        public static string SetDescription()
        {
            bool validInput = false;
            string description = "";
            string input;
            while (!validInput)
            {
                input = PromptUserInLine("Please enter task description: ");
                try
                {
                    if (IsEmpty(input))
                    {
                        throw new Exception("Task must include description.");
                    }
                    description = input;
                    validInput = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
                    Console.WriteLine($"{message[0].ToString().ToUpper() + message.Substring(1, message.Length - 1)} cancelled.");
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
                Console.WriteLine("{0,-3}{1,-10}", $"{i + 1}", $"{list[i]}");
            }
        }

        public static void DrawTasks(List<Task> tasks)
        {
            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
                return;
            }
            Console.WriteLine();
            string format = "{0,-3}{1,-26}{2,-16}{3,-12}{4,-11}";
            Console.WriteLine(format, "#", "Description", "Member", "Due Date", "Status");
            for (int i = 0; i < tasks.Count; i++)
            {
                int index = i + 1;

                string date = tasks[i].DueDate.ToShortDateString();
                string member = tasks[i].AssignedMember;
                string description = tasks[i].Description;
                string complete = IsComplete(tasks[i].Status);
                Console.WriteLine(format, index, description, member, date, complete);
            }
            Console.WriteLine();
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
                    throw new Exception("Input can not be empty.");
                }
                else if (!int.TryParse(input, out temp))
                {
                    throw new Exception("Please input a whole number.");
                }
                else if (temp < 1 || temp > list.Count)
                {
                    throw new Exception("Input must be a number from the menu.");
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public static int ParseInt(string input)
        {
            int temp = 0;
            try
            {
                temp = int.Parse(input);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
            }
            return temp;
        }

        public static void EditTask(List<Task> tasks)
        {
            DrawTasks(tasks);
            string input = PromptUserInLine("Which task would you like to edit: ");
            int temp = SelectListItem(tasks, input);
            DateTime dueDate = AddDate();
            string member = AssignMember();
            string description = SetDescription();
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
            DateTime dateString;
            string date;
            string member;
            string description;
            string complete;

            if (IsEmpty(tasks))
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
                return;
            }

            dueBy = AddDate();
            Console.WriteLine($"Tasks due before {dueBy}:");
            string format = "{0,-3}{1,-26}{2,-16}{3,-12}{4,-11}";
            Console.WriteLine(format, "#", "Description", "Member", "Due Date", "Status");
            for (int i = 0; i < tasks.Count; i++)
            {
                dateString = tasks[i].DueDate;
                int index = i + 1;
                if (DateTime.Compare(dateString, dueBy) <= 0)
                {
                    date = tasks[i].DueDate.ToShortDateString();
                    member = tasks[i].AssignedMember;
                    description = tasks[i].Description;
                    complete = IsComplete(tasks[i].Status);
                    Console.WriteLine(format, index, description, member, date, complete);
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