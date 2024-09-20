using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {
    public interface IBookService {
        Task<Book> GetByIdAsync(int id);
        Task CreateAsync();
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }
}
