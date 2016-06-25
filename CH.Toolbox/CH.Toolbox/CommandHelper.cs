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
            var path = Path.Combine(basePath, @"Data\Commands");
            var dirs = Directory.GetDirectories(path);
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
    }
}
