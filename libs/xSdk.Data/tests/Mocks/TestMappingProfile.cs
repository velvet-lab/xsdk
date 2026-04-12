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

using Mapster;
using xSdk.Data.Converters;
using xSdk.Tools;

namespace xSdk.Data.Mocks;

internal class TestMappingProfile : MappingProfile
{
    protected override void Configure()
    {

        //this.CreateMap<TestEntity, TestModel>()
        //    .ForMember(x => x.PrimaryKey, opts => opts.Ignore())
        //    .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToEntityProperty()))
        //    .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
        //    .ForMember(x => x.Age, opts => opts.MapFrom(x => x.Age))
        //    .ReverseMap()
        //    .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
        //    .ForMember(x => x.Age, opts => opts.MapFrom(x => x.Age))
        //    .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToModelProperty()))
        //    .ForMember(x => x.PrimaryKey, opts => opts.Ignore());

        CreateMap<TestEntity, TestModel>()
            .Map(dest => dest.Id, src => PrimaryKeyTools.Convert(src.Id))
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Age, src => src.Age);


        CreateMap<TestModel, TestEntity>()
            .Map(dest => dest.Id, src => PrimaryKeyTools.Convert<Guid>(src.Id))
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Age, src => src.Age);
    }
}
