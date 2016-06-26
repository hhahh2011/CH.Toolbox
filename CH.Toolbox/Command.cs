using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CH.Toolbox
{

    public class CommandCategory
    {
        public CommandCategory()
        {
            Commands = new List<Command>();
        }
        public string Name { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public List<Command> Commands { get; set; }
    }

    public class Command
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public string Cmd { get; set; }
    }
}
