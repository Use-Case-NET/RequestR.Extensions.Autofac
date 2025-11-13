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

using System.Reflection;

namespace DustInTheWind.RequestR.Extensions.Autofac;

public class UseCaseEngineOptions
{
    internal HashSet<Type> UseCaseTypes { get; } = [];

    internal HashSet<Type> ValidatorTypes { get; } = [];

    public void AddFromAssembly(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        GetAllUseCasesOrValidators(assembly);
    }

    public void AddFromAssemblyContaining<T>()
    {
        Assembly assembly = typeof(T).Assembly;
        GetAllUseCasesOrValidators(assembly);
    }

    private void GetAllUseCasesOrValidators(Assembly assembly)
    {
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                Type[] interfaces = type.GetInterfaces();

                foreach (Type @interface in interfaces)
                {
                    if (@interface.IsGenericType)
                    {
                        Type genericTypeDefinition = @interface.GetGenericTypeDefinition();

                        if (genericTypeDefinition == typeof(IUseCase<,>) ||
                            genericTypeDefinition == typeof(IUseCase<>))
                        {
                            UseCaseTypes.Add(type);
                            break;
                        }
                        else if (genericTypeDefinition == typeof(IRequestValidator<>))
                        {
                            ValidatorTypes.Add(type);
                            break;
                        }
                    }
                }
            }
        }
    }
}
