using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime BirthDate { get; set; }
}
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=proc;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", IsActive = true, RegistrationDate = new DateTime(2023, 5, 10), BirthDate = new DateTime(1990, 3, 15) },
            new User { Id = 2, Name = "Alice Smith", Email = "alice@example.com", IsActive = true, RegistrationDate = new DateTime(2022, 8, 21), BirthDate = new DateTime(1995, 7, 20) },
            new User { Id = 3, Name = "Michael Johnson", Email = "michael@example.com", IsActive = false, RegistrationDate = new DateTime(2021, 2, 5), BirthDate = new DateTime(1985, 11, 30) }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 1, 10), TotalAmount = 120.50m },
            new Order { Id = 2, UserId = 2, OrderDate = new DateTime(2024, 2, 15), TotalAmount = 250.00m },
            new Order { Id = 3, UserId = 1, OrderDate = new DateTime(2024, 3, 5), TotalAmount = 75.30m }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Dell XPS 13 Laptop", Price = 1200.99m },
            new Product { Id = 2, Name = "Logitech MX Master 3 Mouse", Price = 99.99m },
            new Product { Id = 3, Name = "LG UltraGear 27\" Monitor", Price = 399.99m },
            new Product { Id = 4, Name = "Razer BlackWidow Keyboard", Price = 149.50m }
        );
    }
}


//1) Створити процедуру, що зберігається, яка приймає UserId і повертає інформацію про користувача.
//2) Створити процедуру, що зберігається, що повертає всіх активних користувачів.
//3) Створити процедуру, що зберігається, для додавання нового користувача.
//4) Створити процедуру оновлення email користувача.
//5) Створити процедуру видалення користувача UserId.
//6) Створити процедуру, яка повертає кількість активних користувачів.
//7) Створити процедуру, яка повертає користувачів, зареєстрованих у певному діапазоні дат.
//8) Створити процедуру, що повертає середній вік користувачів через вихідний параметр.
//9) Створити процедуру, яка повертає список замовлень певного користувача.
//10) Створити процедуру, яка повертає товари із можливою фільтрацією за ціною.
//11) Створити процедуру списку замовлень користувача (Id, OrderDate, TotalAmount).
//12) Створити процедуру, яка повертає інформацію про найдорожче замовлення з бази.




class Program 
{ 
    static void Main( string[] args)
    {
        using var context = new ApplicationContext();

        context.Database.EnsureCreated();


        var userId = new SqlParameter("@UserId", 1);
        var user = context.Users.FromSqlRaw("EXEC Sp_GetUserById @UserId", userId).ToList().FirstOrDefault();
        Console.WriteLine($"1) Information about User: {user?.Name} \nEmail: {user?.Email} \nIs active: {user?.IsActive} \nRegistration date: {user?.RegistrationDate} \nBirth date: {user?.BirthDate}");



        var activeUsers = context.Users.FromSqlRaw("EXEC Sp_GetActiveUsers").ToList();
        Console.WriteLine($"2) Number of active users: {activeUsers.Count}");
        foreach (var userr in activeUsers)
        {
            Console.WriteLine($"Active users: {userr.Name}");
        }



        var name = new SqlParameter("@Name", "Max Savchin");
        var email = new SqlParameter("@Email", "savchintim@gmail.com");
        var isActive = new SqlParameter("@IsActive", true);
        var regDate = new SqlParameter("@RegistrationDate", DateTime.Now);
        var birthDate = new SqlParameter("@BirthDate", new DateTime(2005, 10, 05));

        int rowsAffected = context.Database.ExecuteSqlRaw("EXEC Sp_CreateUser @Name, @Email, @IsActive, @RegistrationDate, @BirthDate", name, email, isActive, regDate, birthDate);
        Console.WriteLine($"3) Adding a new user: {rowsAffected}");
        Console.WriteLine($"Data: \nName: {name.Value} \nEmail: {email.Value} \nIs active: {isActive.Value} \nRegistration date: {regDate.Value} \nBirth date: {birthDate.Value}");




        var userToUpdate = context.Users.FirstOrDefault(u => u.Id == 1);
        string oldEmail = userToUpdate?.Email ?? "Not Found";

        var id = new SqlParameter("@UserId", 1);
        var newEmail = new SqlParameter("@NewEmail", "john.doe@gmail.com");

        context.Database.ExecuteSqlRaw("EXEC Sp_UpdateUserEmail @UserId, @NewEmail", id, newEmail);

        Console.WriteLine("4) User email updated successfully.");
        Console.WriteLine($"User's past mail: {oldEmail} and current: {newEmail.Value}");




        var deleteId = new SqlParameter("@UserId", 3);
        context.Database.ExecuteSqlRaw("EXEC Sp_DeleteUser @UserId", deleteId);
        Console.WriteLine("5) User with Id 3 has been deleted.");




        int count = context.Database.SqlQuery<int>($"EXEC Sp_GetActiveUsersCount").AsEnumerable().First();
        Console.WriteLine($"6) Number of active users: {count}");




        var start = new SqlParameter("@StartDate", new DateTime(2022, 1, 1));
        var end = new SqlParameter("@EndDate", new DateTime(2024, 1, 1));
        var filteredUsers = context.Users.FromSqlRaw("EXEC Sp_GetUsersByRegistrationDateRange @StartDate, @EndDate", start, end).ToList();
        Console.WriteLine($"7) Users registered from 2022 to 2024: {filteredUsers.Count}");




        int avgAge = context.Database.SqlQuery<int>($"EXEC Sp_GetAverageUserAge").AsEnumerable().First();
        Console.WriteLine($"8) Average age of users: {avgAge}");




        var orderUserId = new SqlParameter("@UserId", 1);
        var userOrders = context.Orders.FromSqlRaw("EXEC Sp_GetOrdersByUserId @UserId", orderUserId).ToList();
        Console.WriteLine($"9) Number of user orders Id 1: {userOrders.Count}");





        var minPrice = new SqlParameter("@MinPrice", 100.00m);
        var maxPrice = new SqlParameter("@MaxPrice", 500.00m);
        var filteredProducts = context.Products.FromSqlRaw("EXEC Sp_GetProductsByPrice @MinPrice, @MaxPrice", minPrice, maxPrice).ToList();
        Console.WriteLine($"10) Products in price range $100-$500: {filteredProducts.Count}");




        var summaryUserId = new SqlParameter("@UserId", 1);
        var ordersSummary = context.Orders.FromSqlRaw("EXEC Sp_GetUserOrdersSummary @UserId", summaryUserId).ToList();
        Console.WriteLine($"11) Abbreviated order report for user Id = 1. Records received: {ordersSummary.Count}");




        var mostExpensiveOrder = context.Orders.FromSqlRaw("EXEC Sp_GetMostExpensiveOrder").ToList().FirstOrDefault();
        Console.WriteLine($"12) The most expensive order: Id = {mostExpensiveOrder?.Id}, Sum = {mostExpensiveOrder?.TotalAmount} USD");
    }
}

