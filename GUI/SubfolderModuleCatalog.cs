using Prism.Modularity;
using System.Collections.Generic;
using System.IO;

namespace GUI
{
    public class SubfolderModuleCatalog : DirectoryModuleCatalog
    {
        protected override void InnerLoad()
        {
            base.InnerLoad();
            string solutionFolder = ".";
            for (int i = 0; i < 3; i++)
            {
                solutionFolder = Directory.GetParent(solutionFolder).ToString();
            }

            this.LoadModulesFromDirectories(
                Directory.GetDirectories(solutionFolder, "*Module", SearchOption.AllDirectories));
        }

        private void LoadModulesFromDirectories(IEnumerable<string> directories)
        {
            foreach (string directory in directories)
            {
#if DEBUG
                this.ModulePath = Path.Combine(directory, "bin", "debug");
#else
				this.ModulePath = Path.Combine(directory, "bin", "release");
#endif
                base.InnerLoad();
            }
        }
    }
}
