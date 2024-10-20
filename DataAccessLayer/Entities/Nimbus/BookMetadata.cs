﻿namespace DataAccessLayer.Entities.Nimbus
{
    public class BookMetadata
    {

        public int Id { get; set; }  // Primary key

        public int BookId { get; set; }  // Foreign key

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
