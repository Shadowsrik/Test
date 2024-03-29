/**
 * Autogenerated by Thrift Compiler (0.9.1)
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

public partial class SeqThriftService {
  public interface Iface {
    string startAlignment(Dictionary<string, Dictionary<string, string>> forAlignment, string jobid);
    #if SILVERLIGHT
    IAsyncResult Begin_startAlignment(AsyncCallback callback, object state, Dictionary<string, Dictionary<string, string>> forAlignment, string jobid);
    string End_startAlignment(IAsyncResult asyncResult);
    #endif
  }

  public class Client : IDisposable, Iface {
    public Client(TProtocol prot) : this(prot, prot)
    {
    }

    public Client(TProtocol iprot, TProtocol oprot)
    {
      iprot_ = iprot;
      oprot_ = oprot;
    }

    protected TProtocol iprot_;
    protected TProtocol oprot_;
    protected int seqid_;

    public TProtocol InputProtocol
    {
      get { return iprot_; }
    }
    public TProtocol OutputProtocol
    {
      get { return oprot_; }
    }


    #region " IDisposable Support "
    private bool _IsDisposed;

    // IDisposable
    public void Dispose()
    {
      Dispose(true);
    }
    

    protected virtual void Dispose(bool disposing)
    {
      if (!_IsDisposed)
      {
        if (disposing)
        {
          if (iprot_ != null)
          {
            ((IDisposable)iprot_).Dispose();
          }
          if (oprot_ != null)
          {
            ((IDisposable)oprot_).Dispose();
          }
        }
      }
      _IsDisposed = true;
    }
    #endregion


    
    #if SILVERLIGHT
    public IAsyncResult Begin_startAlignment(AsyncCallback callback, object state, Dictionary<string, Dictionary<string, string>> forAlignment, string jobid)
    {
      return send_startAlignment(callback, state, forAlignment, jobid);
    }

    public string End_startAlignment(IAsyncResult asyncResult)
    {
      oprot_.Transport.EndFlush(asyncResult);
      return recv_startAlignment();
    }

    #endif

    public string startAlignment(Dictionary<string, Dictionary<string, string>> forAlignment, string jobid)
    {
      #if !SILVERLIGHT
      send_startAlignment(forAlignment, jobid);
      return recv_startAlignment();

      #else
      var asyncResult = Begin_startAlignment(null, null, forAlignment, jobid);
      return End_startAlignment(asyncResult);

      #endif
    }
    #if SILVERLIGHT
    public IAsyncResult send_startAlignment(AsyncCallback callback, object state, Dictionary<string, Dictionary<string, string>> forAlignment, string jobid)
    #else
    public void send_startAlignment(Dictionary<string, Dictionary<string, string>> forAlignment, string jobid)
    #endif
    {
      oprot_.WriteMessageBegin(new TMessage("startAlignment", TMessageType.Call, seqid_));
      startAlignment_args args = new startAlignment_args();
      args.ForAlignment = forAlignment;
      args.Jobid = jobid;
      args.Write(oprot_);
      oprot_.WriteMessageEnd();
      #if SILVERLIGHT
      return oprot_.Transport.BeginFlush(callback, state);
      #else
      oprot_.Transport.Flush();
      #endif
    }

    public string recv_startAlignment()
    {
      TMessage msg = iprot_.ReadMessageBegin();
      if (msg.Type == TMessageType.Exception) {
        TApplicationException x = TApplicationException.Read(iprot_);
        iprot_.ReadMessageEnd();
        throw x;
      }
      startAlignment_result result = new startAlignment_result();
      result.Read(iprot_);
      iprot_.ReadMessageEnd();
      if (result.__isset.success) {
        return result.Success;
      }
      throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "startAlignment failed: unknown result");
    }

  }
  public class Processor : TProcessor {
    public Processor(Iface iface)
    {
      iface_ = iface;
      processMap_["startAlignment"] = startAlignment_Process;
    }

    protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
    private Iface iface_;
    protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

    public bool Process(TProtocol iprot, TProtocol oprot)
    {
      try
      {
        TMessage msg = iprot.ReadMessageBegin();
        ProcessFunction fn;
        processMap_.TryGetValue(msg.Name, out fn);
        if (fn == null) {
          TProtocolUtil.Skip(iprot, TType.Struct);
          iprot.ReadMessageEnd();
          TApplicationException x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
          oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
          x.Write(oprot);
          oprot.WriteMessageEnd();
          oprot.Transport.Flush();
          return true;
        }
        fn(msg.SeqID, iprot, oprot);
      }
      catch (IOException)
      {
        return false;
      }
      return true;
    }

    public void startAlignment_Process(int seqid, TProtocol iprot, TProtocol oprot)
    {
      startAlignment_args args = new startAlignment_args();
      args.Read(iprot);
      iprot.ReadMessageEnd();
      startAlignment_result result = new startAlignment_result();
      result.Success = iface_.startAlignment(args.ForAlignment, args.Jobid);
      oprot.WriteMessageBegin(new TMessage("startAlignment", TMessageType.Reply, seqid)); 
      result.Write(oprot);
      oprot.WriteMessageEnd();
      oprot.Transport.Flush();
    }

  }


  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class startAlignment_args : TBase
  {
    private Dictionary<string, Dictionary<string, string>> _forAlignment;
    private string _jobid;

    public Dictionary<string, Dictionary<string, string>> ForAlignment
    {
      get
      {
        return _forAlignment;
      }
      set
      {
        __isset.forAlignment = true;
        this._forAlignment = value;
      }
    }

    public string Jobid
    {
      get
      {
        return _jobid;
      }
      set
      {
        __isset.jobid = true;
        this._jobid = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool forAlignment;
      public bool jobid;
    }

    public startAlignment_args() {
    }

    public void Read (TProtocol iprot)
    {
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
            if (field.Type == TType.Map) {
              {
                ForAlignment = new Dictionary<string, Dictionary<string, string>>();
                TMap _map0 = iprot.ReadMapBegin();
                for( int _i1 = 0; _i1 < _map0.Count; ++_i1)
                {
                  string _key2;
                  Dictionary<string, string> _val3;
                  _key2 = iprot.ReadString();
                  {
                    _val3 = new Dictionary<string, string>();
                    TMap _map4 = iprot.ReadMapBegin();
                    for( int _i5 = 0; _i5 < _map4.Count; ++_i5)
                    {
                      string _key6;
                      string _val7;
                      _key6 = iprot.ReadString();
                      _val7 = iprot.ReadString();
                      _val3[_key6] = _val7;
                    }
                    iprot.ReadMapEnd();
                  }
                  ForAlignment[_key2] = _val3;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Jobid = iprot.ReadString();
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
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("startAlignment_args");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ForAlignment != null && __isset.forAlignment) {
        field.Name = "forAlignment";
        field.Type = TType.Map;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.Map, ForAlignment.Count));
          foreach (string _iter8 in ForAlignment.Keys)
          {
            oprot.WriteString(_iter8);
            {
              oprot.WriteMapBegin(new TMap(TType.String, TType.String, ForAlignment[_iter8].Count));
              foreach (string _iter9 in ForAlignment[_iter8].Keys)
              {
                oprot.WriteString(_iter9);
                oprot.WriteString(ForAlignment[_iter8][_iter9]);
              }
              oprot.WriteMapEnd();
            }
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (Jobid != null && __isset.jobid) {
        field.Name = "jobid";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Jobid);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("startAlignment_args(");
      sb.Append("ForAlignment: ");
      sb.Append(ForAlignment);
      sb.Append(",Jobid: ");
      sb.Append(Jobid);
      sb.Append(")");
      return sb.ToString();
    }

  }


  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class startAlignment_result : TBase
  {
    private string _success;

    public string Success
    {
      get
      {
        return _success;
      }
      set
      {
        __isset.success = true;
        this._success = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool success;
    }

    public startAlignment_result() {
    }

    public void Read (TProtocol iprot)
    {
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
          case 0:
            if (field.Type == TType.String) {
              Success = iprot.ReadString();
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
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("startAlignment_result");
      oprot.WriteStructBegin(struc);
      TField field = new TField();

      if (this.__isset.success) {
        if (Success != null) {
          field.Name = "Success";
          field.Type = TType.String;
          field.ID = 0;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Success);
          oprot.WriteFieldEnd();
        }
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("startAlignment_result(");
      sb.Append("Success: ");
      sb.Append(Success);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
