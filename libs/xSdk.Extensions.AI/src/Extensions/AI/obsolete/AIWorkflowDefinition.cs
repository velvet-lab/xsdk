/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//using Microsoft.Agents.AI.Workflows;
//using Microsoft.Extensions.DependencyInjection;

//namespace xSdk.Extensions.AI;

//internal abstract class AIWorkflowDefinition(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//{
//    public string Name { get; } = name;

//    public string? Description { get; } = description;

//    public Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> Factory { get; } = factory;

//    internal abstract (WorkflowBuilder, Executor) CreateBuilder(IServiceProvider provider);
//}

//internal class AIWorkflowDefinition<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory) : AIWorkflowDefinition(name, description, factory) where TStartExecutor : Executor
//{
//    internal override (WorkflowBuilder, Executor) CreateBuilder(IServiceProvider provider)
//    {
//        string executorName = ExecutorExtensions.RetrieveExecutorName<TStartExecutor>();

//        TStartExecutor firstExecutor = provider.GetKeyedService<TStartExecutor>(executorName) ?? throw new InvalidOperationException($"Unable to resolve the starting executor of type {typeof(TStartExecutor).FullName} for workflow '{Name}'. Ensure it is registered in the service collection.");
//        var builder = new WorkflowBuilder(firstExecutor);

//        if (!string.IsNullOrEmpty(Name))
//        {
//            builder.WithName(Name);
//        }

//        if (!string.IsNullOrEmpty(Description))
//        {
//            builder.WithDescription(Description);
//        }

//        return (builder, firstExecutor);
//    }
//}
