///*
// * Copyright 2026 Roland Breitschaft
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using xSdk.Extensions.Variable.Attributes;

//namespace xSdk.Extensions.Variable.Fakes;

//[VariableNoPrefix()]
//internal class SetupWithNoPrefix : Setup
//{
//    [Variable(
//        name: Definitions.StringValue.Name,
//        template: Definitions.StringValue.Template,
//        helpText: Definitions.StringValue.HelpText,
//        defaultValue: Definitions.StringValue.DefaultValue,
//        resourceNames: new[] { "xsdk.environment.stage", "deployment.environment" }
//    )]
//    public string StringValue
//    {
//        get => ReadValue<string>(Definitions.StringValue.Name);
//        set => SetValue(Definitions.StringValue.Name, value);
//    }

//    public static class Definitions
//    {
//        public static class StringValue
//        {
//            public const string Name = "stringValue";
//            public const string Template = "--string-value <value>";
//            public const string HelpText = "StringValue for use.";
//            public const string DefaultValue = "MyDefaultValue";
//        }
//    }
//}
