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

//using xSdk.Data.Converters.Mapper;

//namespace xSdk.Data.Mocks
//{
//    internal class TestMapper : Profile
//    {
//        public TestMapper()
//        {
//            this.CreateMap<TestEntity, TestModel>()
//                .ForMember(x => x.PrimaryKey, opts => opts.Ignore())
//                .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToEntityProperty()))
//                .ForMember(x => x.MyName, opts => opts.MapFrom(x => x.Name))
//                .ForMember(x => x.MyAge, opts => opts.MapFrom(x => x.Age))
//                .ReverseMap()
//                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.MyName))
//                .ForMember(x => x.Age, opts => opts.MapFrom(x => x.MyAge))
//                .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToModelProperty()))
//                .ForMember(x => x.PrimaryKey, opts => opts.Ignore());
//        }
//    }
//}
