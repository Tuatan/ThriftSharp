/**
 * Autogenerated by Thrift Compiler (0.9.2)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftSharp.Benchmarking.Models.Thrift
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Int32Container : TBase
  {

    public int Value { get; set; }

    public Int32Container() {
    }

    public Int32Container(int value) : this() {
      this.Value = value;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_value = false;
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              Value = iprot.ReadI32();
              isset_value = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
      if (!isset_value)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("Int32Container");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "value";
      field.Type = TType.I32;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteI32(Value);
      oprot.WriteFieldEnd();
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("Int32Container(");
      __sb.Append(", Value: ");
      __sb.Append(Value);
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
