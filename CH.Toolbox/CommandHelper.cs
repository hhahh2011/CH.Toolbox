using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Toolbox
{
    public static class CommandHelper
    {
        public static List<CommandCategory> GetAllCommands(string basePath)
        {
            var list = new List<CommandCategory>();
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var dirs = Directory.GetDirectories(basePath);
            foreach (var dir in dirs)
            {
                var category = new CommandCategory
                {
                    Name =Path.GetFileNameWithoutExtension(dir),
                    FullName = dir,
                    DisplayName = Path.GetFileNameWithoutExtension(dir)
                };
                var files = Directory.GetFiles(category.FullName);

                foreach (var file in files)
                {
                    using (var fs=new FileStream(file,FileMode.Open,FileAccess.Read))
                    {
                        using (var sr=new StreamReader(fs))
                        {
                            var json = sr.ReadToEnd();
                            if (!string.IsNullOrEmpty(json))
                            {
                                var cmd = json.ToObject<Command>();
                                if (cmd != null)
                                {
                                    cmd.FullName = file;
                                    cmd.Name = Path.GetFileNameWithoutExtension(file);
                                    category.Commands.Add(cmd);
                                }
                            }
                        }
                    }
                }
                list.Add(category);
            }
            return list;
        }

        public static Command CreateCommand(string fullName, CommandCategory category)
        {
            if (string.IsNullOrEmpty(fullName) || category == null)
            {
                return null;
            }

            var command = new Command
            {
                DisplayName = Path.GetFileNameWithoutExtension(fullName),
                Cmd = fullName
            };

            var savePath = Path.Combine(category.FullName, command.DisplayName + ".json");
            using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(command.ToJsonString(indented:true,isIgnoreNull: true));
                }
            }
            command.Name = Path.GetFileNameWithoutExtension(fullName);
            command.FullName = savePath;
            return command;
        }

    }
}
