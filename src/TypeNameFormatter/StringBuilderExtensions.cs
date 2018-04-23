﻿// Copyright (c) 2018 stakx
// License available at https://github.com/stakx/TypeNameFormatter/blob/master/LICENSE.md.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace TypeNameFormatter
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class StringBuilderExtensions
    {
        public static StringBuilder AppendName(this StringBuilder stringBuilder, Type type)
        {
            stringBuilder.AppendName(type, withNamespace: false);
            return stringBuilder;
        }

        public static StringBuilder AppendFullName(this StringBuilder stringBuilder, Type type)
        {
            stringBuilder.AppendName(type, withNamespace: true);
            return stringBuilder;
        }

        private static void AppendName(this StringBuilder stringBuilder, Type type, bool withNamespace)
        {
            if (type.IsGenericParameter == false)
            {
                if (type.IsNested)
                {
                    stringBuilder.AppendName(type.DeclaringType, withNamespace);
                    stringBuilder.Append('.');
                }
                else if (withNamespace)
                {
                    string @namespace = type.Namespace;
                    if (string.IsNullOrEmpty(@namespace) == false)
                    {
                        stringBuilder.Append(type.Namespace);
                        stringBuilder.Append('.');
                    }
                }
            }

            var name = type.Name;
            if (type.IsGenericType)
            {
                var backtickIndex = name.LastIndexOf('`');
                Debug.Assert(backtickIndex >= 0);
                stringBuilder.Append(name, 0, backtickIndex);

                stringBuilder.Append('<');

                var genericTypeArgs = type.GetGenericArguments();
                for (int i = 0, n = genericTypeArgs.Length; i < n; ++i)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.AppendName(genericTypeArgs[i], withNamespace);
                }

                stringBuilder.Append('>');
            }
            else
            {
                stringBuilder.Append(name);
            }
        }
    }
}