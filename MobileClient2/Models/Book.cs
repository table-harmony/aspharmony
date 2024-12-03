using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MobileClient2.Models;

public class Book{
    public int Id { get; set; }
    public BookMetadata Metadata { get; set; }
    public Author Author { get; set; }
    public ServerType Server { get; set; }
}

public class BookMetadata {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
}

public class Author {
    public string Id { get; set; }
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    public string Email { get; set; }
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}

public enum ServerType {
    Aether,
    Atlas,
    Dummy,
    Echo,
    Harmony,
    [Display(Name = "Nimbus 1.0")]
    Nimbus1,
    [Display(Name = "Nimbus 2.0")]
    Nimbus2,
    Orion,
    Solace,
    [Display(Name = "Stegan 1.0")]
    Stegan1,
    [Display(Name = "Stegan 2.0")]
    Stegan2,
}