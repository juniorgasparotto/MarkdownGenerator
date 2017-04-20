using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Commands;
using SysCommand.ConsoleApp.Helpers;
using SysCommand.ConsoleApp.Loader;
using System;

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
                loader.IgnoreCommand<ArgsHistoryCommand>();
                var app = new App(loader.GetFromAppDomain());

                app.OnException += (result, ex) =>
                {
                    var msg = ex.Message + (ex.InnerException == null ? "" : "\r\n" + ex.InnerException.Message);
                    app.Console.Error(msg, false, true);
                    app.Console.Error(ex.StackTrace);

                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException is AggregateException list)
                        {
                            foreach (var exInner in list.InnerExceptions)
                            {
                                app.Console.Error(exInner.Message, false, true);
                                app.Console.Error(" --- Inner exception: " + ex.InnerException.StackTrace, false, true);
                            }
                        }
                        else
                        {
                            app.Console.Error(" --- Inner exception: " + ex.InnerException.StackTrace);
                        }
                    }
                };

                return app;
            });
        }
    }
}
