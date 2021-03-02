// RequestR.Extensions.Autofac
// Copyright (C) 2021 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;

namespace DustInTheWind.RequestR.Extensions.Autofac
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRequestBus(this ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterInstance(kernel).As<IServiceProvider>();
            containerBuilder.RegisterType<RequestHandlerFactory>().As<IRequestHandlerFactory>();
            containerBuilder.RegisterType<RequestBus>().AsSelf().SingleInstance();
            containerBuilder.AddAllHandlersAndValidators();
        }

        private static void AddAllHandlersAndValidators(this ContainerBuilder kernel)
        {
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));

            AppDomain appDomain = AppDomain.CurrentDomain;

            Assembly[] assemblies = appDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> requestHandlersOrValidators = assembly.GetAllRequestHandlersOrValidators();

                foreach (Type handlerOrValidatorType in requestHandlersOrValidators)
                    kernel.RegisterType(handlerOrValidatorType).AsSelf();
            }
        }
    }
}