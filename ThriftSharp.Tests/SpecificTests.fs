﻿// Copyright (c) 2014-15 Solal Pirelli
// This code is licensed under the MIT License (see Licence.txt for details).

module ThriftSharp.Tests.``Specific tests``

open System.Reflection
open ThriftSharp
open ThriftSharp.Internals

[<ThriftStruct("Simple")>]
type Simple() =
    [<ThriftField(1s, true, "field")>]
    member val Field = "" with get, set

(* TestClass *)
type MemoryLeakTests() =
    (* Test *)
    member x.``[Regression] No reference is kept to returned objects``() =
        let prot = MemoryProtocol([MessageHeader ("Test", ThriftMessageType.Reply)
                                   StructHeader ""
                                   FieldHeader (0s, "", tid 12)
                                   StructHeader "Simple"
                                   FieldHeader (1s, "field", tid 11)
                                   String "Hello"
                                   FieldEnd
                                   FieldStop
                                   StructEnd
                                   FieldEnd
                                   FieldStop
                                   StructEnd
                                   MessageEnd])
        let meth = ThriftMethod("test", false, ThriftReturnValue(typeof<Simple>.GetTypeInfo(), null), [| |], [| |])
        let resultRef = System.WeakReference(ThriftClientMessageReader.Read(meth, prot))
        System.GC.Collect()
        resultRef.IsAlive <=> false