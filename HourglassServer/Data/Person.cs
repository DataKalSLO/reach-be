namespace HourglassServer.Data
{
   public partial class Person
   {
      public string Email { get; set; }
      public string Password { get; set; }
      public string Name { get; set; }
      public int Role { get; set; }
      public string Salt { get; set; }
      public string PasswordHash { get; set; }
   }
}
