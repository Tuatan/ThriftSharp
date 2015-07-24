﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ThriftSharp.Benchmarking
{
    public sealed class BenchmarkResult
    {
        public string Name { get; }
        public TimeSpan ThriftSharpReadTime { get; }
        public TimeSpan ThriftSharpWriteTime { get; }
        public TimeSpan ThriftWriteTime { get; }
        public TimeSpan ThriftReadTime { get; }

        public BenchmarkResult( string name, TimeSpan thriftSharpRead, TimeSpan thriftSharpWrite, TimeSpan thriftRead, TimeSpan thriftWrite )
        {
            Name = name;
            ThriftSharpReadTime = thriftSharpRead;
            ThriftSharpWriteTime = thriftSharpWrite;
            ThriftReadTime = thriftRead;
            ThriftWriteTime = thriftWrite;
        }

        public override string ToString()
        {
            return string.Format( CultureInfo.InvariantCulture,
                                  "{0,-20} | {1,8:F2} | {2,8:F2} | {3,8:F2} | {4,8:F2}",
                                  Name,
                                  ThriftSharpReadTime.TotalMilliseconds, ThriftSharpWriteTime.TotalMilliseconds,
                                  ThriftReadTime.TotalMilliseconds, ThriftWriteTime.TotalMilliseconds );
        }

        public static BenchmarkResult Average( IEnumerable<BenchmarkResult> results )
        {
            return new BenchmarkResult(
                results.First().Name,
                Average( results.Select( r => r.ThriftSharpReadTime ) ),
                Average( results.Select( r => r.ThriftSharpWriteTime ) ),
                Average( results.Select( r => r.ThriftReadTime ) ),
                Average( results.Select( r => r.ThriftWriteTime ) )
            );
        }

        private static TimeSpan Average( IEnumerable<TimeSpan> times )
        {
            return new TimeSpan( times.Sum( t => t.Ticks ) / times.LongCount() );
        }
    }
}