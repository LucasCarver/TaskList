using System;
using System.Collections.Generic;

namespace TaskList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>() {
            new Task("Joe", DateTime.Parse("12/10/2021"), "Develop the new algorithm"),
            new Task("Jessica", DateTime.Parse("11/15/2022"), "Arrange a meeting")
            };
            string input;
            int selection = 0;
            bool validInput;
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
                validInput = false;
                // Draw menu -> Get input -> Validate input
                while (!validInput)
                {
                    DrawList(menu);
                    input = PromptUserInLine("Input menu selection: ");
                    selection = SelectMenuItem(menu, input);
                    if (selection != -1)
                    {
                        validInput = true;
                    }
                }
                // Make selection
                exitCondition = MakeSelection(selection, tasks);
            }
        }

        public static bool MakeSelection(int selection, List<Task> tasks)
        {
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
            string input;
            while (true)
            {
                input = PromptUserInLine("Are you sure you wish to quit? Y/N ");
                if (input != "Y" && input != "N")
                {
                    Console.WriteLine("Y or N only please.");
                    continue;
                }
                else if (input == "Y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void MarkTaskComplete(List<Task> tasks)
        {
            int temp = -1;
            string input;
            bool validInput = false;
            string taskToComplete;
            if (tasks.Count != 0)
            {
                while (!validInput)
                {
                    DrawTasks(tasks);
                    input = PromptUserInLine("Please enter task to mark as completed: ");
                    try
                    {
                        temp = SelectTaskItem(tasks, input);
                        if (temp != -1)
                        {
                            validInput = true;
                        }
                        taskToComplete = $"{tasks[temp - 1].DueDate.ToShortDateString()} {tasks[temp - 1].AssignedMember} {tasks[temp - 1].Description}";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                }
                validInput = false;
                while (!validInput)
                {
                    input = PromptUserInLine($"\nAre you sure you wish to mark task \"{temp}\" as completed? Y/N ").Trim();
                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("Invalid input. Y or N only.");
                        continue;
                    }
                    else if (input == "Y")
                    {
                        Console.WriteLine($"Item successfully completed");
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine($"Item not completed. Canceling mark complete operation.");
                        validInput = true;
                    }
                }
                tasks[temp - 1].TaskComplete = true; ;
            }
            else
            {
                Console.WriteLine("\nNo tasks to mark as completed. The list is empty.\n");
            }
        }

        public static void AddTask(List<Task> tasks)
        {
            DateTime dueDate = AddDueDate();
            string assignedMember = AssignMember();
            string description = SetDescription();
            tasks.Add(new Task(assignedMember, dueDate, description));
            Console.WriteLine($"Task \"{tasks.Count} {assignedMember} {dueDate} {description}\" added successfully.");
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

        public static void DeleteTask(List<Task> tasks)
        {
            int temp = -1;
            string justRemoved = "";
            string input;
            bool validInput = false;
            if (tasks.Count != 0)
            {
                while (!validInput)
                {
                    DrawTasks(tasks);

                    input = PromptUserInLine("Please enter task to delete: ");
                    try
                    {
                        temp = SelectTaskItem(tasks, input);
                        if (temp != -1)
                        {
                            validInput = true;
                        }
                        justRemoved = $"{tasks[temp - 1].DueDate.ToShortDateString()} {tasks[temp - 1].AssignedMember} {tasks[temp - 1].Description}";
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                }
                validInput = false;
                while (!validInput)
                {
                    input = PromptUserInLine($"\nAre you sure you wish to delete task entry\n\"{temp} {justRemoved}\"? Y/N ").Trim();
                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("Invalid input. Y or N only.");
                        continue;
                    }
                    else if (input == "Y")
                    {
                        Console.WriteLine($"Item successfully deleted");
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine($"Item not deleted. Canceling delete operation.");
                        validInput = true;
                    }
                }
                tasks.RemoveAt(temp - 1);
            }
            else
            {
                Console.WriteLine("\nNo tasks to delete. The list is empty.\n");
            }
        }

        public static string PromptUser(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine().Trim();
        }

        public static string PromptUserInLine(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim();
        }

        public static void DrawList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("{0,-3}{1,-10}", $"{i + 1}", $"{list[i]}");
            }
        }

        public static void DrawTasks(List<Task> tasks)
        {
            if (tasks.Count != 0)
            {
                Console.WriteLine();
                string format = "{0,-3}{1,-12}{2,-16}{3,-31}{4,-11}";
                Console.WriteLine(format, "#", "Due Date", "Member", "Description", "Status");
                for (int i = 0; i < tasks.Count; i++)
                {
                    int index = i + 1;
                    string date = tasks[i].DueDate.ToShortDateString();
                    string member = tasks[i].AssignedMember;
                    string description = tasks[i].Description;
                    string complete = IsComplete(tasks[i].TaskComplete);

                    Console.WriteLine(format, index, date, member, description, complete);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
            }
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

        public static int SelectMenuItem(List<string> list, string input)
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
                    throw new Exception("Input must be a whole number.");
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public static int SelectTaskItem(List<Task> list, string input)
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
            int temp = SelectTaskItem(tasks, input);
            DateTime dueDate = AddDueDate();
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
            bool validInput = false;
            string input;
            DateTime dueBy;
            DateTime dateString;
            if (tasks.Count != 0)
            {
                while (!validInput)
                {
                    input = PromptUserInLine("View tasks due before (mm/dd/yy): ");
                    try
                    {
                        dueBy = DateTime.Parse(input);
                        validInput = true;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid format.");
                        continue;
                    }
                    string format = "{0,-3}{1,-12}{2,-16}{3,-31}{4,-11}";
                    Console.WriteLine();
                    Console.WriteLine(format, "#", "Due Date", "Member", "Description", "Status");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        dateString = tasks[i].DueDate;
                        int index = i + 1;
                        if (DateTime.Compare(dueBy, dateString) >= 0)
                        {
                            string date = tasks[i].DueDate.ToShortDateString();
                            string member = tasks[i].AssignedMember;
                            string description = tasks[i].Description;
                            string complete = IsComplete(tasks[i].TaskComplete);
                            Console.WriteLine(format, index, date, member, description, complete);
                        }
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nNo tasks to display. The list is empty.\n");
            }
        }

        public static DateTime AddDueDate()
        {
            DateTime dueDate = DateTime.Parse("01/01/2020");
            bool validInput = false;
            string input;
            while (!validInput)
            {
                input = PromptUserInLine("Please enter a due date (mm/dd/yy): ");
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