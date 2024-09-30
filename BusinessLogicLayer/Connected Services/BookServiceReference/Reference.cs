﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookServiceReference
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Book", Namespace="http://tempuri.org/")]
    public partial class Book : object
    {
        
        private int IdField;
        
        private string TitleField;
        
        private string DescriptionField;
        
        private string ImageUrlField;
        
        private BookServiceReference.Chapter[] ChaptersField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                this.TitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string ImageUrl
        {
            get
            {
                return this.ImageUrlField;
            }
            set
            {
                this.ImageUrlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public BookServiceReference.Chapter[] Chapters
        {
            get
            {
                return this.ChaptersField;
            }
            set
            {
                this.ChaptersField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Chapter", Namespace="http://tempuri.org/")]
    public partial class Chapter : object
    {
        
        private int IndexField;
        
        private string TitleField;
        
        private string ContentField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Index
        {
            get
            {
                return this.IndexField;
            }
            set
            {
                this.IndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                this.TitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Content
        {
            get
            {
                return this.ContentField;
            }
            set
            {
                this.ContentField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BookServiceReference.BookServiceSoap")]
    public interface BookServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetBook", ReplyAction="*")]
        System.Threading.Tasks.Task<BookServiceReference.GetBookResponse> GetBookAsync(BookServiceReference.GetBookRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetAllBooks", ReplyAction="*")]
        System.Threading.Tasks.Task<BookServiceReference.GetAllBooksResponse> GetAllBooksAsync(BookServiceReference.GetAllBooksRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CreateBook", ReplyAction="*")]
        System.Threading.Tasks.Task<BookServiceReference.CreateBookResponse> CreateBookAsync(BookServiceReference.CreateBookRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UpdateBook", ReplyAction="*")]
        System.Threading.Tasks.Task<BookServiceReference.UpdateBookResponse> UpdateBookAsync(BookServiceReference.UpdateBookRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DeleteBook", ReplyAction="*")]
        System.Threading.Tasks.Task DeleteBookAsync(int id);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBookRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBook", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.GetBookRequestBody Body;
        
        public GetBookRequest()
        {
        }
        
        public GetBookRequest(BookServiceReference.GetBookRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetBookRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int id;
        
        public GetBookRequestBody()
        {
        }
        
        public GetBookRequestBody(int id)
        {
            this.id = id;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBookResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBookResponse", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.GetBookResponseBody Body;
        
        public GetBookResponse()
        {
        }
        
        public GetBookResponse(BookServiceReference.GetBookResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetBookResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public BookServiceReference.Book GetBookResult;
        
        public GetBookResponseBody()
        {
        }
        
        public GetBookResponseBody(BookServiceReference.Book GetBookResult)
        {
            this.GetBookResult = GetBookResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetAllBooksRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetAllBooks", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.GetAllBooksRequestBody Body;
        
        public GetAllBooksRequest()
        {
        }
        
        public GetAllBooksRequest(BookServiceReference.GetAllBooksRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class GetAllBooksRequestBody
    {
        
        public GetAllBooksRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetAllBooksResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetAllBooksResponse", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.GetAllBooksResponseBody Body;
        
        public GetAllBooksResponse()
        {
        }
        
        public GetAllBooksResponse(BookServiceReference.GetAllBooksResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetAllBooksResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public BookServiceReference.Book[] GetAllBooksResult;
        
        public GetAllBooksResponseBody()
        {
        }
        
        public GetAllBooksResponseBody(BookServiceReference.Book[] GetAllBooksResult)
        {
            this.GetAllBooksResult = GetAllBooksResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CreateBookRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CreateBook", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.CreateBookRequestBody Body;
        
        public CreateBookRequest()
        {
        }
        
        public CreateBookRequest(BookServiceReference.CreateBookRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CreateBookRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public BookServiceReference.Book newBook;
        
        public CreateBookRequestBody()
        {
        }
        
        public CreateBookRequestBody(BookServiceReference.Book newBook)
        {
            this.newBook = newBook;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CreateBookResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CreateBookResponse", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.CreateBookResponseBody Body;
        
        public CreateBookResponse()
        {
        }
        
        public CreateBookResponse(BookServiceReference.CreateBookResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class CreateBookResponseBody
    {
        
        public CreateBookResponseBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UpdateBookRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UpdateBook", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.UpdateBookRequestBody Body;
        
        public UpdateBookRequest()
        {
        }
        
        public UpdateBookRequest(BookServiceReference.UpdateBookRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class UpdateBookRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public BookServiceReference.Book updatedBook;
        
        public UpdateBookRequestBody()
        {
        }
        
        public UpdateBookRequestBody(BookServiceReference.Book updatedBook)
        {
            this.updatedBook = updatedBook;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UpdateBookResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UpdateBookResponse", Namespace="http://tempuri.org/", Order=0)]
        public BookServiceReference.UpdateBookResponseBody Body;
        
        public UpdateBookResponse()
        {
        }
        
        public UpdateBookResponse(BookServiceReference.UpdateBookResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class UpdateBookResponseBody
    {
        
        public UpdateBookResponseBody()
        {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface BookServiceSoapChannel : BookServiceReference.BookServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class BookServiceSoapClient : System.ServiceModel.ClientBase<BookServiceReference.BookServiceSoap>, BookServiceReference.BookServiceSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public BookServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(BookServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), BookServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BookServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(BookServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BookServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(BookServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BookServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BookServiceReference.GetBookResponse> BookServiceReference.BookServiceSoap.GetBookAsync(BookServiceReference.GetBookRequest request)
        {
            return base.Channel.GetBookAsync(request);
        }
        
        public System.Threading.Tasks.Task<BookServiceReference.GetBookResponse> GetBookAsync(int id)
        {
            BookServiceReference.GetBookRequest inValue = new BookServiceReference.GetBookRequest();
            inValue.Body = new BookServiceReference.GetBookRequestBody();
            inValue.Body.id = id;
            return ((BookServiceReference.BookServiceSoap)(this)).GetBookAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BookServiceReference.GetAllBooksResponse> BookServiceReference.BookServiceSoap.GetAllBooksAsync(BookServiceReference.GetAllBooksRequest request)
        {
            return base.Channel.GetAllBooksAsync(request);
        }
        
        public System.Threading.Tasks.Task<BookServiceReference.GetAllBooksResponse> GetAllBooksAsync()
        {
            BookServiceReference.GetAllBooksRequest inValue = new BookServiceReference.GetAllBooksRequest();
            inValue.Body = new BookServiceReference.GetAllBooksRequestBody();
            return ((BookServiceReference.BookServiceSoap)(this)).GetAllBooksAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BookServiceReference.CreateBookResponse> BookServiceReference.BookServiceSoap.CreateBookAsync(BookServiceReference.CreateBookRequest request)
        {
            return base.Channel.CreateBookAsync(request);
        }
        
        public System.Threading.Tasks.Task<BookServiceReference.CreateBookResponse> CreateBookAsync(BookServiceReference.Book newBook)
        {
            BookServiceReference.CreateBookRequest inValue = new BookServiceReference.CreateBookRequest();
            inValue.Body = new BookServiceReference.CreateBookRequestBody();
            inValue.Body.newBook = newBook;
            return ((BookServiceReference.BookServiceSoap)(this)).CreateBookAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BookServiceReference.UpdateBookResponse> BookServiceReference.BookServiceSoap.UpdateBookAsync(BookServiceReference.UpdateBookRequest request)
        {
            return base.Channel.UpdateBookAsync(request);
        }
        
        public System.Threading.Tasks.Task<BookServiceReference.UpdateBookResponse> UpdateBookAsync(BookServiceReference.Book updatedBook)
        {
            BookServiceReference.UpdateBookRequest inValue = new BookServiceReference.UpdateBookRequest();
            inValue.Body = new BookServiceReference.UpdateBookRequestBody();
            inValue.Body.updatedBook = updatedBook;
            return ((BookServiceReference.BookServiceSoap)(this)).UpdateBookAsync(inValue);
        }
        
        public System.Threading.Tasks.Task DeleteBookAsync(int id)
        {
            return base.Channel.DeleteBookAsync(id);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BookServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.BookServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BookServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("https://localhost:44383/BookService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.BookServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("https://localhost:44383/BookService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            BookServiceSoap,
            
            BookServiceSoap12,
        }
    }
}
