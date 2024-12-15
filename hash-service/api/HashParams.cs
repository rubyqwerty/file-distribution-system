/**
 * <auto-generated>
 * Autogenerated by Thrift Compiler (0.19.0)
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 * </auto-generated>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Protocol.Entities;
using Thrift.Protocol.Utilities;
using Thrift.Transport;
using Thrift.Transport.Client;
using Thrift.Transport.Server;
using Thrift.Processor;


#pragma warning disable IDE0079  // remove unnecessary pragmas
#pragma warning disable IDE0017  // object init can be simplified
#pragma warning disable IDE0028  // collection init can be simplified
#pragma warning disable IDE1006  // parts of the code use IDL spelling
#pragma warning disable CA1822   // empty DeepCopy() methods still non-static
#pragma warning disable IDE0083  // pattern matching "that is not SomeType" requires net5.0 but we still support earlier versions

namespace hash_service
{

  public partial class HashParams : TBase
  {
    private string _data;
    private global::hash_service.Algorithms _algorithm;
    private global::hash_service.Format _outputFormat;
    private int _hashLength;
    private int _numberIteration;

    public string Data
    {
      get
      {
        return _data;
      }
      set
      {
        __isset.@data = true;
        this._data = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="global::hash_service.Algorithms"/>
    /// </summary>
    public global::hash_service.Algorithms Algorithm
    {
      get
      {
        return _algorithm;
      }
      set
      {
        __isset.@algorithm = true;
        this._algorithm = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="global::hash_service.Format"/>
    /// </summary>
    public global::hash_service.Format OutputFormat
    {
      get
      {
        return _outputFormat;
      }
      set
      {
        __isset.outputFormat = true;
        this._outputFormat = value;
      }
    }

    public int HashLength
    {
      get
      {
        return _hashLength;
      }
      set
      {
        __isset.hashLength = true;
        this._hashLength = value;
      }
    }

    public int NumberIteration
    {
      get
      {
        return _numberIteration;
      }
      set
      {
        __isset.numberIteration = true;
        this._numberIteration = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool @data;
      public bool @algorithm;
      public bool outputFormat;
      public bool hashLength;
      public bool numberIteration;
    }

    public HashParams()
    {
    }

    public HashParams DeepCopy()
    {
      var tmp0 = new HashParams();
      if((Data != null) && __isset.@data)
      {
        tmp0.Data = this.Data;
      }
      tmp0.__isset.@data = this.__isset.@data;
      if(__isset.@algorithm)
      {
        tmp0.Algorithm = this.Algorithm;
      }
      tmp0.__isset.@algorithm = this.__isset.@algorithm;
      if(__isset.outputFormat)
      {
        tmp0.OutputFormat = this.OutputFormat;
      }
      tmp0.__isset.outputFormat = this.__isset.outputFormat;
      if(__isset.hashLength)
      {
        tmp0.HashLength = this.HashLength;
      }
      tmp0.__isset.hashLength = this.__isset.hashLength;
      if(__isset.numberIteration)
      {
        tmp0.NumberIteration = this.NumberIteration;
      }
      tmp0.__isset.numberIteration = this.__isset.numberIteration;
      return tmp0;
    }

    public async global::System.Threading.Tasks.Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        await iprot.ReadStructBeginAsync(cancellationToken);
        while (true)
        {
          field = await iprot.ReadFieldBeginAsync(cancellationToken);
          if (field.Type == TType.Stop)
          {
            break;
          }

          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String)
              {
                Data = await iprot.ReadStringAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.I32)
              {
                Algorithm = (global::hash_service.Algorithms)await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 3:
              if (field.Type == TType.I32)
              {
                OutputFormat = (global::hash_service.Format)await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 4:
              if (field.Type == TType.I32)
              {
                HashLength = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 5:
              if (field.Type == TType.I32)
              {
                NumberIteration = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            default: 
              await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              break;
          }

          await iprot.ReadFieldEndAsync(cancellationToken);
        }

        await iprot.ReadStructEndAsync(cancellationToken);
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public async global::System.Threading.Tasks.Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
    {
      oprot.IncrementRecursionDepth();
      try
      {
        var tmp1 = new TStruct("HashParams");
        await oprot.WriteStructBeginAsync(tmp1, cancellationToken);
        var tmp2 = new TField();
        if((Data != null) && __isset.@data)
        {
          tmp2.Name = "data";
          tmp2.Type = TType.String;
          tmp2.ID = 1;
          await oprot.WriteFieldBeginAsync(tmp2, cancellationToken);
          await oprot.WriteStringAsync(Data, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.@algorithm)
        {
          tmp2.Name = "algorithm";
          tmp2.Type = TType.I32;
          tmp2.ID = 2;
          await oprot.WriteFieldBeginAsync(tmp2, cancellationToken);
          await oprot.WriteI32Async((int)Algorithm, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.outputFormat)
        {
          tmp2.Name = "outputFormat";
          tmp2.Type = TType.I32;
          tmp2.ID = 3;
          await oprot.WriteFieldBeginAsync(tmp2, cancellationToken);
          await oprot.WriteI32Async((int)OutputFormat, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.hashLength)
        {
          tmp2.Name = "hashLength";
          tmp2.Type = TType.I32;
          tmp2.ID = 4;
          await oprot.WriteFieldBeginAsync(tmp2, cancellationToken);
          await oprot.WriteI32Async(HashLength, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.numberIteration)
        {
          tmp2.Name = "numberIteration";
          tmp2.Type = TType.I32;
          tmp2.ID = 5;
          await oprot.WriteFieldBeginAsync(tmp2, cancellationToken);
          await oprot.WriteI32Async(NumberIteration, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        await oprot.WriteFieldStopAsync(cancellationToken);
        await oprot.WriteStructEndAsync(cancellationToken);
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override bool Equals(object that)
    {
      if (!(that is HashParams other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return ((__isset.@data == other.__isset.@data) && ((!__isset.@data) || (global::System.Object.Equals(Data, other.Data))))
        && ((__isset.@algorithm == other.__isset.@algorithm) && ((!__isset.@algorithm) || (global::System.Object.Equals(Algorithm, other.Algorithm))))
        && ((__isset.outputFormat == other.__isset.outputFormat) && ((!__isset.outputFormat) || (global::System.Object.Equals(OutputFormat, other.OutputFormat))))
        && ((__isset.hashLength == other.__isset.hashLength) && ((!__isset.hashLength) || (global::System.Object.Equals(HashLength, other.HashLength))))
        && ((__isset.numberIteration == other.__isset.numberIteration) && ((!__isset.numberIteration) || (global::System.Object.Equals(NumberIteration, other.NumberIteration))));
    }

    public override int GetHashCode() {
      int hashcode = 157;
      unchecked {
        if((Data != null) && __isset.@data)
        {
          hashcode = (hashcode * 397) + Data.GetHashCode();
        }
        if(__isset.@algorithm)
        {
          hashcode = (hashcode * 397) + Algorithm.GetHashCode();
        }
        if(__isset.outputFormat)
        {
          hashcode = (hashcode * 397) + OutputFormat.GetHashCode();
        }
        if(__isset.hashLength)
        {
          hashcode = (hashcode * 397) + HashLength.GetHashCode();
        }
        if(__isset.numberIteration)
        {
          hashcode = (hashcode * 397) + NumberIteration.GetHashCode();
        }
      }
      return hashcode;
    }

    public override string ToString()
    {
      var tmp3 = new StringBuilder("HashParams(");
      int tmp4 = 0;
      if((Data != null) && __isset.@data)
      {
        if(0 < tmp4++) { tmp3.Append(", "); }
        tmp3.Append("Data: ");
        Data.ToString(tmp3);
      }
      if(__isset.@algorithm)
      {
        if(0 < tmp4++) { tmp3.Append(", "); }
        tmp3.Append("Algorithm: ");
        Algorithm.ToString(tmp3);
      }
      if(__isset.outputFormat)
      {
        if(0 < tmp4++) { tmp3.Append(", "); }
        tmp3.Append("OutputFormat: ");
        OutputFormat.ToString(tmp3);
      }
      if(__isset.hashLength)
      {
        if(0 < tmp4++) { tmp3.Append(", "); }
        tmp3.Append("HashLength: ");
        HashLength.ToString(tmp3);
      }
      if(__isset.numberIteration)
      {
        if(0 < tmp4++) { tmp3.Append(", "); }
        tmp3.Append("NumberIteration: ");
        NumberIteration.ToString(tmp3);
      }
      tmp3.Append(')');
      return tmp3.ToString();
    }
  }

}
