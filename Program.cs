using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Commands;
using SysCommand.ConsoleApp.Helpers;
using SysCommand.ConsoleApp.Loader;

namespace MarkdownGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
            // sample: --indexFile Sample/Index.xml
            return App.RunApplication(() =>
            {
                var loader = new AppDomainCommandLoader();
                loader.IgnoreCommand<VerboseCommand>();
                loader.IgnoreCommand<ArgsHistoryCommand>();
                var app = new App(loader.GetFromAppDomain());
                return app;
            });
        }
    }
}
