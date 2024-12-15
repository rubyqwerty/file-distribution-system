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


#pragma warning disable IDE0079  // remove unnecessary pragmas
#pragma warning disable IDE0017  // object init can be simplified
#pragma warning disable IDE0028  // collection init can be simplified
#pragma warning disable IDE1006  // parts of the code use IDL spelling
#pragma warning disable CA1822   // empty DeepCopy() methods still non-static
#pragma warning disable IDE0083  // pattern matching "that is not SomeType" requires net5.0 but we still support earlier versions

namespace distribution_service
{
  public static class distribution_serviceExtensions
  {
    public static bool Equals(this List<global::distribution_service.Chunk> instance, object that)
    {
      if (!(that is List<global::distribution_service.Chunk> other)) return false;
      if (ReferenceEquals(instance, other)) return true;

      return TCollections.Equals(instance, other);
    }


    public static int GetHashCode(this List<global::distribution_service.Chunk> instance)
    {
      return TCollections.GetHashCode(instance);
    }


    public static List<global::distribution_service.Chunk> DeepCopy(this List<global::distribution_service.Chunk> source)
    {
      if (source == null)
        return null;

      var tmp60 = new List<global::distribution_service.Chunk>(source.Count);
      foreach (var elem in source)
        tmp60.Add((elem != null) ? elem.DeepCopy() : null);
      return tmp60;
    }


    public static bool Equals(this List<global::distribution_service.Replication> instance, object that)
    {
      if (!(that is List<global::distribution_service.Replication> other)) return false;
      if (ReferenceEquals(instance, other)) return true;

      return TCollections.Equals(instance, other);
    }


    public static int GetHashCode(this List<global::distribution_service.Replication> instance)
    {
      return TCollections.GetHashCode(instance);
    }


    public static List<global::distribution_service.Replication> DeepCopy(this List<global::distribution_service.Replication> source)
    {
      if (source == null)
        return null;

      var tmp61 = new List<global::distribution_service.Replication>(source.Count);
      foreach (var elem in source)
        tmp61.Add((elem != null) ? elem.DeepCopy() : null);
      return tmp61;
    }


  }
}