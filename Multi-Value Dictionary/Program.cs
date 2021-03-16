using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using Multi_Value_Dictionary.Properties;

namespace Multi_Value_Dictionary {

    class Program {
        private static void WriteToConsole(string command) => Console.WriteLine(command);

        private static void WelcomeMessage(ResourceManager rm) {
            WriteToConsole(rm.GetString("WelcomeMessage"));
            WriteToConsole(rm.GetString("UserSelection"));
        }

        public static void Add(IOperations<string> operations, params string[] par) {
            WriteToConsole(operations.Add(par[0], par[1]) ? "Added" : "ERROR, value already exists");
        }

        public static void Keys(IOperations<string> operations) {
            var i = 1;
            if (!operations.Keys().Any()) {
                WriteToConsole("(empty set)");
                return;
            }
            foreach (var key in operations.Keys()) 
                WriteToConsole($@"{i++}) {key}");
            
        }

        public static void Members(IOperations<string> operations, string key) {
            if (!operations.KeyExists(key)) WriteToConsole("ERROR, key does not exist.");
            else {
                var i = 1;
                foreach (var member in operations.Members(key)) WriteToConsole($@"{i++}) {member}");
            }
        }

        public static void DoesKeyExist(IOperations<string> operations, string key) {
            WriteToConsole(operations.KeyExists(key).ToString());
        }

        public static void Remove(IOperations<string> operations, params string[] par) {
            if (!operations.KeyExists(par[0])) {
                WriteToConsole("ERROR, key does not exist");
                return;
            }
            
            if (operations.ValueExists(par[0], par[1] ?? null)) {
                operations.Remove(par[0], par[1]);
                WriteToConsole("Removed");
            } else {
                WriteToConsole("ERROR, value does not exist");
            }
        }

        public static void RemoveAll(IOperations<string> operations, params string[] par) {
            if (!operations.KeyExists(par[0])) {
                WriteToConsole("ERROR, key does not exist");
                return;
            }

            try {
                operations.RemoveAll(par[0]);
                WriteToConsole("Removed");
            } catch (Exception e) {
                WriteToConsole(e.ToString());
                throw;
            }
        }
        public static void AllMembers(IOperations<string> operations) {
            if (!operations.Keys().Any()) {
                WriteToConsole("(empty set)");
            }
            var i = 0;
            foreach (var member in operations.AllMembers()) {
                foreach (var subMembers in member) {
                    WriteToConsole($"{++i}) {subMembers}");
                }
            }
        }
        public static void AllItems(IOperations<string> operations) {
            if (!operations.Keys().Any()) {
                WriteToConsole("(empty set)");
                return;
            }

            WriteToConsole(operations.Items());
        }

        private static string StringOrEmpty(string cmd) => !string.IsNullOrEmpty(cmd) ? cmd : string.Empty;
        private static void Main(string[] args) {
            IOperations<string> operations = new Operations(new Dictionary<string, List<string>>());

            var rm = Resources.ResourceManager;
            WelcomeMessage(rm);

            string userInput = null;
            while (userInput != UserChoices.EXIT.ToString().Trim()) {
                userInput = Console.ReadLine();
                if (userInput != null) {
                    var cmd = userInput.Split(' ');
                    var userSelection = ConvertEnumToValue(cmd.FirstOrDefault());

                    if (userSelection is null) {
                        WriteToConsole(rm.GetString("IncorrectFormat"));
                        WriteToConsole(rm.GetString("UserSelection"));
                        continue;
                    }

                    switch (userSelection) {
                        case UserChoices.ADD:
                            if (cmd.Length == 3 && StringOrEmpty(cmd[1]) != string.Empty && StringOrEmpty(cmd[2]) != string.Empty) Add(operations, cmd[1], cmd[2]);
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.KEYS:
                            Keys(operations);
                            break;
                        case UserChoices.MEMBERS:
                            if (cmd.Length == 2) Members(operations, cmd[1]);
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.REMOVE:
                            if (cmd.Length == 2 && StringOrEmpty(cmd[1]) != string.Empty) Remove(operations, StringOrEmpty(cmd[1]));
                            else if(cmd.Length == 3 && StringOrEmpty(cmd[1]) != string.Empty && StringOrEmpty(cmd[2]) != string.Empty) Remove(operations, StringOrEmpty(cmd[1]), StringOrEmpty(cmd[2])); 
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.REMOVEALL:
                            if (cmd.Length == 2 && StringOrEmpty(cmd[1]) != string.Empty) RemoveAll(operations, StringOrEmpty(cmd[1]));
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.CLEAR:
                            operations.Clear();
                            WriteToConsole("Cleared");
                            break;
                        case UserChoices.KEYEXISTS:
                            if (cmd.Length == 2 && StringOrEmpty(cmd[1]) != string.Empty) DoesKeyExist(operations, cmd[1]); 
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.VALUEEXISTS:
                            if(cmd.Length == 3 && StringOrEmpty(cmd[1]) != string.Empty && StringOrEmpty(cmd[2]) != string.Empty)WriteToConsole(operations.ValueExists(cmd[1], cmd[2]).ToString());
                            else WriteToConsole("Incorrect arguments\nTry again");
                            break;
                        case UserChoices.ALLMEMBERS:
                            AllMembers(operations);
                            break;
                        case UserChoices.ITEMS:
                            AllItems(operations);
                            break;
                        case UserChoices.EXIT:
                            userInput = "EXIT";
                            break;
                        case UserChoices.REPEATMENU:
                            WriteToConsole(rm.GetString("WelcomeMessage"));
                            break;
                    }
                }
            }
            
        }

        private static UserChoices? ConvertEnumToValue(string functionKey) {
            if (Enum.TryParse<UserChoices>(functionKey, out var userChoice)) return userChoice;
            return null;
        }
    }
}
