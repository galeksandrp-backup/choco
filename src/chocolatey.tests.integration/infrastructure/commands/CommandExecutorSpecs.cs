﻿// Copyright © 2011 - Present RealDimensions Software, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// 
// You may obtain a copy of the License at
// 
// 	http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.tests.integration.infrastructure.commands
{
    using System;
    using Should;
    using chocolatey.infrastructure.app;
    using chocolatey.infrastructure.commands;
    using chocolatey.infrastructure.filesystem;

    public class CommandExecutorSpecs
    {
        public abstract class CommandExecutorSpecsBase : TinySpec
        {
            public override void Context()
            {
            }
        }

        public class when_CommandExecutor_errors : CommandExecutorSpecsBase
        {
            private int result;
            private string errorOutput;
            private readonly IFileSystem file_system = new DotNetFileSystem();

            public override void Context()
            {
                base.Context();
            }

            public override void Because()
            {
                result = CommandExecutor.execute("cmd.exe", "/c bob123123", ApplicationParameters.DefaultWaitForExitInSeconds, file_system.get_current_directory(), null, (s, e) => { errorOutput += e.Data; }, updateProcessPath: false);
            }

            [Fact]
            public void should_not_return_an_exit_code_of_zero()
            {
                result.ShouldNotEqual(0);
            }

            [Fact]
            public void should_contain_error_output()
            {
                errorOutput.ShouldNotBeNull();
            }

            [Fact]
            public void should_message_the_error()
            {
                errorOutput.ShouldEqual("'bob123123' is not recognized as an internal or external command,operable program or batch file.");
            }
        }

        public class when_CommandExecutor_is_given_a_nonexisting_process : CommandExecutorSpecsBase
        {
            private string result;
            private string errorOutput;

            public override void Because()
            {
                try
                {
                    CommandExecutor.execute("noprocess.exe", "/c bob123123", ApplicationParameters.DefaultWaitForExitInSeconds, null, (s, e) => { errorOutput += e.Data; });
                }
                catch (Exception e)
                {
                    result = e.Message;
                }
            }

            [Fact]
            public void should_have_an_error_message()
            {
                result.ShouldNotBeNull();
            }
        }
    }
}