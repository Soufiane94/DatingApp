using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {

        //constructor
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        
        //Dbset property, its conventional to pluralise the name of the entities

       
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users {get; set;}

    }
}