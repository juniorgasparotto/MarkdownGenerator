﻿using Publisher.CommandSpecific.Chocolatey;
using Publisher.Core;
using SysCommand.ConsoleApp;

namespace Publisher.CommandSpecific
{
    public class PublishCommand : Command
    {
        public void Publish()
        {
            var build = App.Commands.Get<BuildCommand>();
            var pack = App.Commands.Get<PackCommand>();
            var version = App.Commands.Get<VersionCommand>();
            var github = App.Commands.Get<GitHubCommand>();
            var pathCommand = App.Commands.Get<PathCommand>();
            var chocolateyCommand = App.Commands.Get<ChocolateyCommand>();

            Title("BUILD");
            build.Clear();
            build.Build();

            Title("PACK");
            pack.Pack();

            Title("SET NEXT VERSION");
            version.Next();

            if (Utils.Continue(this, "Do you create a new release in github?"))
            {
                Title("GITHUB - CREATE NEW RELEASE");

                // Check error in connection
                github.TestConnection();

                github.CreateRelease();
                                
                if (Utils.Continue(this, "Do you want to upload the package to github?"))
                {
                    Title("GITHUB - UPLOADING PACKS");
                    github.UploadPackFolder();
                }
            }

            if (Utils.Continue(this, "Do you create a new release in Chocolatey?"))
            {
                Title("CHOCOLATEY - CREATE NEW RELEASE");

                chocolateyCommand.Build();
                chocolateyCommand.Pack();

                if (Utils.Continue(this, "Do you want to upload the package to Chocolatey.org?"))
                {
                    Title("CHOCOLATEY - PUSHING PACK");
                    chocolateyCommand.Push();
                }
            }
        }

        private void Title(string text)
        {
            var count = 80;
            App.Console.Write(new string('-', count));
            App.Console.Write(text);
            App.Console.Write(new string('-', count));
        }
    }
}