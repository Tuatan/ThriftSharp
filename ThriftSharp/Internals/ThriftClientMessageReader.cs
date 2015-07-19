﻿// Copyright (c) 2014-15 Solal Pirelli
// This code is licensed under the MIT License (see Licence.txt for details).

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ThriftSharp.Protocols;
using ThriftSharp.Utilities;

namespace ThriftSharp.Internals
{
    /// <summary>
    /// Reads Thrift messages sent from a server to a client.
    /// </summary>
    internal static class ThriftClientMessageReader
    {
        private static readonly ThriftStruct ThriftProtocolExceptionStruct = ThriftAttributesParser.ParseStruct( TypeInfos.ThriftProtocolException );
        private static readonly Dictionary<ThriftMethod, object> _knownReaders = new Dictionary<ThriftMethod, object>();


        /// <summary>
        /// Creates a reader for the specified method.
        /// </summary>
        private static LambdaExpression CreateReaderForMethod( ThriftMethod method )
        {
            var protocolParam = Expression.Parameter( typeof( IThriftProtocol ) );

            var headerVariable = Expression.Variable( typeof( ThriftMessageHeader ) );
            ParameterExpression hasReturnVariable = null;
            ParameterExpression returnVariable = null;

            if ( method.ReturnValue.UnderlyingTypeInfo != TypeInfos.Void )
            {
                hasReturnVariable = Expression.Variable( typeof( bool ) );
                returnVariable = Expression.Variable( method.ReturnValue.UnderlyingTypeInfo.AsType() );
            }

            var fieldsAndSetters = new Dictionary<ThriftField, Func<Expression, Expression>>();

            if ( returnVariable != null )
            {
                // Field 0 is the return value
                fieldsAndSetters.Add(
                    method.ReturnValue,
                    expr => Expression.Block(
                        Expression.Assign(
                            returnVariable,
                            expr
                        ),
                        Expression.Assign(
                            hasReturnVariable,
                            Expression.Constant( true )
                        )
                    )
                );
            }

            // All other fields are declared exceptions
            foreach ( var exception in method.Exceptions )
            {
                fieldsAndSetters.Add( exception, Expression.Throw );
            }

            var statements = new List<Expression>
            {
                Expression.Assign(
                    headerVariable,
                    Expression.Call(
                        protocolParam,
                        Methods.IThriftProtocol_ReadMessageHeader
                    )
                ),

                Expression.IfThen(
                    Expression.IsFalse(
                        Expression.Call(
                            Methods.Enum_IsDefined,
                            Expression.Constant( typeof( ThriftMessageType ) ),
                            Expression.Convert(
                                Expression.Field( headerVariable, Fields.ThriftMessageHeader_MessageType ),
                                typeof( object )
                            )
                        )
                    ),
                    Expression.Throw(
                        Expression.New(
                            Constructors.ThriftProtocolException,
                            Expression.Constant( ThriftProtocolExceptionType.InvalidMessageType )
                        )
                    )
                ),

                Expression.IfThen(
                    Expression.Equal(
                        Expression.Field(  headerVariable,Fields.ThriftMessageHeader_MessageType  ),
                        Expression.Constant( ThriftMessageType.Exception )
                    ),
                    Expression.Throw(
                        Expression.Call(
                            typeof( ThriftStructReader ),
                            "Read",
                            new[] { typeof( ThriftProtocolException ) },
                            Expression.Constant( ThriftProtocolExceptionStruct ), protocolParam
                        )
                    )
                ),

                ThriftStructReader.CreateReaderForFields( protocolParam, fieldsAndSetters ),

                Expression.Call( protocolParam, Methods.IThriftProtocol_ReadMessageEnd ),

                Expression.Call( protocolParam, Methods.IDisposable_Dispose )
            };

            if ( returnVariable != null )
            {
                statements.Add( Expression.IfThen(
                    Expression.Equal(
                        hasReturnVariable,
                        Expression.Constant( false )
                    ),
                    Expression.Throw(
                        Expression.New(
                            Constructors.ThriftProtocolException,
                            Expression.Constant( ThriftProtocolExceptionType.MissingResult )
                        )
                    ) )
                );
            }

            if ( returnVariable == null )
            {
                statements.Add( Expression.Constant( null ) );
            }
            else
            {
                statements.Add( returnVariable );
            }

            return Expression.Lambda(
                Expression.Block(
                    returnVariable == null ? typeof( object ) : returnVariable.Type,
                    returnVariable == null ? new[] { headerVariable } : new[] { headerVariable, hasReturnVariable, returnVariable },
                    statements
                ),
                new[] { protocolParam }
            );
        }

        /// <summary>
        /// Reads a ThriftMessage returned by the specified ThriftMethod on the specified ThriftProtocol.
        /// </summary>
        public static T Read<T>( ThriftMethod method, IThriftProtocol protocol )
        {
            if ( !_knownReaders.ContainsKey( method ) )
            {
                _knownReaders.Add( method, CreateReaderForMethod( method ).Compile() );
            }

            return ( (Func<IThriftProtocol, T>) _knownReaders[method] )( protocol );
        }
    }
}