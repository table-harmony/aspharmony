﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AspHarmonyServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://aspharmony-production.up.railway.app/", ConfigurationName="AspHarmonyServiceReference.AspHarmonyPortType")]
    public interface AspHarmonyPortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://aspharmony-production.up.railway.app/GetServiceInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.GetServiceInfoResponse1> GetServiceInfoAsync(AspHarmonyServiceReference.GetServiceInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://aspharmony-production.up.railway.app/AddNumbers", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.AddNumbersResponse1> AddNumbersAsync(AspHarmonyServiceReference.AddNumbersRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://aspharmony-production.up.railway.app/GenerateJoke", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.GenerateJokeResponse1> GenerateJokeAsync(AspHarmonyServiceReference.GenerateJokeRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://aspharmony-production.up.railway.app/")]
    public partial class GetServiceInfoResponse
    {
        
        private string serviceField;
        
        private string versionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetServiceInfoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetServiceInfoRequest", Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public object GetServiceInfoRequest1;
        
        public GetServiceInfoRequest()
        {
        }
        
        public GetServiceInfoRequest(object GetServiceInfoRequest1)
        {
            this.GetServiceInfoRequest1 = GetServiceInfoRequest1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetServiceInfoResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public AspHarmonyServiceReference.GetServiceInfoResponse GetServiceInfoResponse;
        
        public GetServiceInfoResponse1()
        {
        }
        
        public GetServiceInfoResponse1(AspHarmonyServiceReference.GetServiceInfoResponse GetServiceInfoResponse)
        {
            this.GetServiceInfoResponse = GetServiceInfoResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://aspharmony-production.up.railway.app/")]
    public partial class AddNumbersRequest
    {
        
        private int aField;
        
        private int bField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int a
        {
            get
            {
                return this.aField;
            }
            set
            {
                this.aField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public int b
        {
            get
            {
                return this.bField;
            }
            set
            {
                this.bField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://aspharmony-production.up.railway.app/")]
    public partial class AddNumbersResponse
    {
        
        private int resultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddNumbersRequest1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public AspHarmonyServiceReference.AddNumbersRequest AddNumbersRequest;
        
        public AddNumbersRequest1()
        {
        }
        
        public AddNumbersRequest1(AspHarmonyServiceReference.AddNumbersRequest AddNumbersRequest)
        {
            this.AddNumbersRequest = AddNumbersRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddNumbersResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public AspHarmonyServiceReference.AddNumbersResponse AddNumbersResponse;
        
        public AddNumbersResponse1()
        {
        }
        
        public AddNumbersResponse1(AspHarmonyServiceReference.AddNumbersResponse AddNumbersResponse)
        {
            this.AddNumbersResponse = AddNumbersResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://aspharmony-production.up.railway.app/")]
    public partial class GenerateJokeRequest
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://aspharmony-production.up.railway.app/")]
    public partial class GenerateJokeResponse
    {
        
        private string jokeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string joke
        {
            get
            {
                return this.jokeField;
            }
            set
            {
                this.jokeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerateJokeRequest1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public AspHarmonyServiceReference.GenerateJokeRequest GenerateJokeRequest;
        
        public GenerateJokeRequest1()
        {
        }
        
        public GenerateJokeRequest1(AspHarmonyServiceReference.GenerateJokeRequest GenerateJokeRequest)
        {
            this.GenerateJokeRequest = GenerateJokeRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerateJokeResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://aspharmony-production.up.railway.app/", Order=0)]
        public AspHarmonyServiceReference.GenerateJokeResponse GenerateJokeResponse;
        
        public GenerateJokeResponse1()
        {
        }
        
        public GenerateJokeResponse1(AspHarmonyServiceReference.GenerateJokeResponse GenerateJokeResponse)
        {
            this.GenerateJokeResponse = GenerateJokeResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface AspHarmonyPortTypeChannel : AspHarmonyServiceReference.AspHarmonyPortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class AspHarmonyPortTypeClient : System.ServiceModel.ClientBase<AspHarmonyServiceReference.AspHarmonyPortType>, AspHarmonyServiceReference.AspHarmonyPortType
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public AspHarmonyPortTypeClient() : 
                base(AspHarmonyPortTypeClient.GetDefaultBinding(), AspHarmonyPortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.AspHarmonyPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AspHarmonyPortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(AspHarmonyPortTypeClient.GetBindingForEndpoint(endpointConfiguration), AspHarmonyPortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AspHarmonyPortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(AspHarmonyPortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AspHarmonyPortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(AspHarmonyPortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AspHarmonyPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.GetServiceInfoResponse1> AspHarmonyServiceReference.AspHarmonyPortType.GetServiceInfoAsync(AspHarmonyServiceReference.GetServiceInfoRequest request)
        {
            return base.Channel.GetServiceInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<AspHarmonyServiceReference.GetServiceInfoResponse1> GetServiceInfoAsync(object GetServiceInfoRequest1)
        {
            AspHarmonyServiceReference.GetServiceInfoRequest inValue = new AspHarmonyServiceReference.GetServiceInfoRequest();
            inValue.GetServiceInfoRequest1 = GetServiceInfoRequest1;
            return ((AspHarmonyServiceReference.AspHarmonyPortType)(this)).GetServiceInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.AddNumbersResponse1> AspHarmonyServiceReference.AspHarmonyPortType.AddNumbersAsync(AspHarmonyServiceReference.AddNumbersRequest1 request)
        {
            return base.Channel.AddNumbersAsync(request);
        }
        
        public System.Threading.Tasks.Task<AspHarmonyServiceReference.AddNumbersResponse1> AddNumbersAsync(AspHarmonyServiceReference.AddNumbersRequest AddNumbersRequest)
        {
            AspHarmonyServiceReference.AddNumbersRequest1 inValue = new AspHarmonyServiceReference.AddNumbersRequest1();
            inValue.AddNumbersRequest = AddNumbersRequest;
            return ((AspHarmonyServiceReference.AspHarmonyPortType)(this)).AddNumbersAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AspHarmonyServiceReference.GenerateJokeResponse1> AspHarmonyServiceReference.AspHarmonyPortType.GenerateJokeAsync(AspHarmonyServiceReference.GenerateJokeRequest1 request)
        {
            return base.Channel.GenerateJokeAsync(request);
        }
        
        public System.Threading.Tasks.Task<AspHarmonyServiceReference.GenerateJokeResponse1> GenerateJokeAsync(AspHarmonyServiceReference.GenerateJokeRequest GenerateJokeRequest)
        {
            AspHarmonyServiceReference.GenerateJokeRequest1 inValue = new AspHarmonyServiceReference.GenerateJokeRequest1();
            inValue.GenerateJokeRequest = GenerateJokeRequest;
            return ((AspHarmonyServiceReference.AspHarmonyPortType)(this)).GenerateJokeAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.AspHarmonyPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.AspHarmonyPort))
            {
                return new System.ServiceModel.EndpointAddress("https://aspharmony-production.up.railway.app/service");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return AspHarmonyPortTypeClient.GetBindingForEndpoint(EndpointConfiguration.AspHarmonyPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return AspHarmonyPortTypeClient.GetEndpointAddress(EndpointConfiguration.AspHarmonyPort);
        }
        
        public enum EndpointConfiguration
        {
            
            AspHarmonyPort,
        }
    }
}
