﻿// Copyright (c) 2014 Solal Pirelli
// This code is licensed under the MIT License (see Licence.txt for details).
// Redistributions of this source code must retain the above copyright notice.

using System;

namespace ThriftSharp
{
    /// <summary>
    /// Converts 32-bit integer timestamps to <see cref="DateTime" /> using the Unix format, 
    /// i.e. the number of seconds since Jan. 1, 1970 00:00:00 UTC.
    /// </summary>
    /// <remarks>
    /// Consider using <see cref="ThriftUnixDateOffsetConverter" /> instead; 
    /// the <see cref="DateTimeOffset" /> class replaces the old <see cref="DateTime" /> class 
    /// for most use cases.
    /// </remarks>
    public sealed class ThriftUnixDateConverter : ThriftValueConverter<int, DateTime>
    {
        private static readonly ThriftUnixDate64Converter Converter = new ThriftUnixDate64Converter();

        /// <summary>
        /// Converts the specified 32-bit Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTime Convert( int value )
        {
            return Converter.Convert( value );
        }

        /// <summary>
        /// Converts the specified DateTime to a 32-bit Unix timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override int ConvertBack( DateTime value )
        {
            return (int) Converter.ConvertBack( value );
        }
    }

    /// <summary>
    /// Converts 64-bit integer timestamps to <see cref="DateTime" /> using the Unix format, 
    /// i.e. the number of seconds since Jan. 1, 1970 00:00:00 UTC.
    /// </summary>
    /// <remarks>
    /// Consider using <see cref="ThriftUnixLongDateOffsetConverter" /> instead; 
    /// the <see cref="DateTimeOffset" /> class replaces the old <see cref="DateTime" /> class 
    /// for most use cases.
    /// </remarks>
    public sealed class ThriftUnixDate64Converter : ThriftValueConverter<long, DateTime>
    {
        // The Unix time start: Jan. 1, 1970 00:00:00 UTC.
        private static readonly DateTime UnixTimeStart = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

        /// <summary>
        /// Converts the specified 64-bit Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTime Convert( long value )
        {
            return UnixTimeStart.AddSeconds( value ).ToLocalTime();
        }

        /// <summary>
        /// Converts the specified DateTime to a 64-bit Unix timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override long ConvertBack( DateTime value )
        {
            return (long) Math.Round( ( value.ToUniversalTime() - UnixTimeStart ).TotalSeconds );
        }
    }

    /// <summary>
    /// Converts 64-bit integer timestamps to <see cref="DateTime" /> using the Java format, 
    /// i.e. the number of milliseconds since Jan. 1, 1970 00:00:00.000 UTC.
    /// </summary>
    /// <remarks>
    /// Consider using <see cref="ThriftJavaDateOffsetConverter" /> instead; 
    /// the <see cref="DateTimeOffset" /> class replaces the old <see cref="DateTime" /> class 
    /// for most use cases.
    /// </remarks>
    public sealed class ThriftJavaDateConverter : ThriftValueConverter<long, DateTime>
    {
        // The Java time start: Jan. 1, 1970 00:00:00 UTC.
        private static readonly DateTime JavaTimeStart = new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc );

        /// <summary>
        /// Converts the specified 64-bit Java timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTime Convert( long value )
        {
            return JavaTimeStart.AddMilliseconds( value ).ToLocalTime();
        }

        /// <summary>
        /// Converts the specified DateTime to a 64-bit Java timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override long ConvertBack( DateTime value )
        {
            return (long) Math.Round( ( value.ToUniversalTime() - JavaTimeStart ).TotalMilliseconds );
        }
    }


    /// <summary>
    /// Converts 32-bit integer timestamps to <see cref="DateTimeOffset" /> using the Unix format, 
    /// i.e. the number of seconds since Jan. 1, 1970 00:00:00 UTC.
    /// </summary>
    public sealed class ThriftUnixDateOffsetConverter : ThriftValueConverter<int, DateTimeOffset>
    {
        private static readonly ThriftUnixLongDateOffsetConverter Converter = new ThriftUnixLongDateOffsetConverter();

        /// <summary>
        /// Converts the specified 32-bit Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTimeOffset Convert( int value )
        {
            return Converter.Convert( value );
        }

        /// <summary>
        /// Converts the specified DateTime to a 32-bit Unix timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override int ConvertBack( DateTimeOffset value )
        {
            return (int) Converter.ConvertBack( value );
        }
    }

    /// <summary>
    /// Converts 64-bit integer timestamps to <see cref="DateTimeOffset" /> using the Unix format, 
    /// i.e. the number of seconds since Jan. 1, 1970 00:00:00 UTC.
    /// </summary>
    public sealed class ThriftUnixLongDateOffsetConverter : ThriftValueConverter<long, DateTimeOffset>
    {
        // The Unix time start: Jan. 1, 1970 00:00:00 UTC.
        private static readonly DateTimeOffset UnixTimeStart = new DateTimeOffset( 1970, 1, 1, 0, 0, 0, TimeSpan.Zero );

        /// <summary>
        /// Converts the specified 64-bit Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTimeOffset Convert( long value )
        {
            return UnixTimeStart.AddSeconds( value ).ToLocalTime();
        }

        /// <summary>
        /// Converts the specified DateTime to a 64-bit Unix timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override long ConvertBack( DateTimeOffset value )
        {
            return (long) Math.Round( ( value.ToUniversalTime() - UnixTimeStart ).TotalSeconds );
        }
    }

    /// <summary>
    /// Converts 64-bit integer timestamps to <see cref="DateTimeOffset" /> using the Java format, 
    /// i.e. the number of milliseconds since Jan. 1, 1970 00:00:00.000 UTC.
    /// </summary>
    public sealed class ThriftJavaDateOffsetConverter : ThriftValueConverter<long, DateTimeOffset>
    {
        // The Java time start: Jan. 1, 1970 00:00:00 UTC.
        private static readonly DateTimeOffset JavaTimeStart = new DateTimeOffset( 1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero );

        /// <summary>
        /// Converts the specified 64-bit Java timestamp to a DateTime.
        /// </summary>
        /// <param name="value">The timestamp.</param>
        /// <returns>The resulting DateTime.</returns>
        protected internal override DateTimeOffset Convert( long value )
        {
            return JavaTimeStart.AddMilliseconds( value ).ToLocalTime();
        }

        /// <summary>
        /// Converts the specified DateTime to a 64-bit Java timestamp.
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>The resulting timestamp.</returns>
        protected internal override long ConvertBack( DateTimeOffset value )
        {
            return (long) Math.Round( ( value.ToUniversalTime() - JavaTimeStart ).TotalMilliseconds );
        }
    }
}