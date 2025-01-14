using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Project.Models;

namespace Project.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public  DbSet<Account> Accounts { get; set; }

        public  DbSet<Role> Roles { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Discount> Discounts { get; set; }
		public DbSet<ContactUs> ContactUs { get; set; }
		public DbSet<Airport> Airports { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<FlightStatus> FlightStatuses { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Aircraft_Ticket> Aircraft_Tickets { get; set; }
     
        public DbSet<Aboutus> Aboutus { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Pilot> Pilots { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F8F5CF85F");

                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Fullname)
                    .HasMaxLength(100)
                    .HasColumnName("fullname");
                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");
                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Account>(entity =>
            {             
                entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role");

               entity.HasData(new Account[]
                {
                    new Account{
                        Id = 1,
                        Username="Admin",
                        RoleId=1,
                        Password="$2a$12$Hm559j/qjr.qUw74.94rHuH.Nsac1vfWXP.uRWL6CG2XnyRei4cjy",
                        Address="Ho Chi Minh City",
                        Gender=true,
                        Fullname="Nguyen Van A",
                        IsActive=true,
                        CreateDate = DateTime.Now,
                        Email = "admin@gmail.com",
                        PhoneNumber = "0837267048"
                    },
                     new Account{
                        Id = 2,
                        Username="User",
                        RoleId=2,
                        Password="$2a$12$Hm559j/qjr.qUw74.94rHuH.Nsac1vfWXP.uRWL6CG2XnyRei4cjy",
                        Address="Ha Noi",
                        Gender=true,
                        Fullname="Nguyen Van B",
                        IsActive=true,
                        CreateDate = DateTime.Now,
                        Email = "user@gmail.com",
                        PhoneNumber = "0353718834"
                    },
                       new Account{
                        Id = 3,
                        Username="User",
                        RoleId=2,
                        Password="$2a$12$Hm559j/qjr.qUw74.94rHuH.Nsac1vfWXP.uRWL6CG2XnyRei4cjy",
                        Address="Hai Phong",
                        Gender=true,
                        Fullname="Nguyen Van C",
                        IsActive=true,
                        CreateDate = DateTime.Now,
                        Email = "khaih8375@gmail.com",
                        PhoneNumber = "0886926492"
                    }

               });

            });
            //FlightStatus
            modelBuilder.Entity<FlightStatus>(entity =>
            {
                entity.HasData(new FlightStatus[]
               {
                         new FlightStatus{FlightStatusId = 1, FlightStatusName="Scheduled", Description="Has Created" },
                         new FlightStatus{FlightStatusId = 2, FlightStatusName="Booking", Description="Booking avaiable" },
                         new FlightStatus{FlightStatusId = 3, FlightStatusName="Check in", Description="User have booking can check in" },
                         new FlightStatus{FlightStatusId = 4, FlightStatusName="Delay", Description="Flight have delayed" },
                         new FlightStatus{FlightStatusId = 5, FlightStatusName="On Flying", Description="Flight are ..." },
                         new FlightStatus{FlightStatusId = 6, FlightStatusName="Finish", Description="Flight have finished" },
                         new FlightStatus{FlightStatusId = 7, FlightStatusName="Cancel", Description="Flight have canceled" }
               });
            });
            //Aircraft
            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.HasData(new Aircraft[]
               {
                         new Aircraft{AircraftID = 1, Model="Airbus A320",CabinCount=12},
                         new Aircraft{AircraftID = 2, Model="Airbus A321",CabinCount=22},
                          new Aircraft{AircraftID = 3, Model="Airbus A322",CabinCount=32},
                           new Aircraft{AircraftID = 4, Model="Airbus A323",CabinCount=42},
                            new Aircraft{AircraftID = 5, Model="Airbus A324",CabinCount=52}
               });
            });
            //Airport
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasData(new Airport[]
               {
                         new Airport{AirportID = 1, AirportName="AFF1",City="Da Lat",IsActive=true},
                         new Airport{AirportID = 2, AirportName="AFF2",City="HCM",IsActive=true},
                          new Airport{AirportID = 3, AirportName="AFF3",City="Ha Noi",IsActive=false}
               });
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F7FAF5CF6");

                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasData(new Role[]
                {
                         new Role{Id = 1, Name="Admin"}, new Role{Id = 2, Name="User"}
                });
             });
			


			modelBuilder.Entity<Flight>().HasOne(f => f.Airport).WithMany(t => t.Flights).HasForeignKey(f => f.FromAirportID);
			modelBuilder.Entity<Flight>().HasOne(f => f.FlightStatus).WithMany(t => t.Flights).HasForeignKey(f => f.FlightStatusId);
            modelBuilder.Entity<Flight>().HasOne(f => f.Aircraft).WithMany(a => a.Flights).HasForeignKey(f => f.AircraftId);

            modelBuilder.Entity<Cart>().HasOne(f => f.Account).WithMany(t => t.Carts).HasForeignKey(f => f.AccId);
			modelBuilder.Entity<Cart>().HasOne(f => f.Ticket).WithMany(t => t.Carts).HasForeignKey(f => f.TicketClassId);
			modelBuilder.Entity<Cart>().HasOne(f => f.Flight).WithMany(t => t.Carts).HasForeignKey(f => f.FlightId);

            modelBuilder.Entity<Booking>().HasOne(f => f.Account).WithMany(t => t.Bookings).HasForeignKey(f => f.AccId);
            modelBuilder.Entity<Booking>().HasOne(f => f.Ticket).WithMany(t => t.Bookings).HasForeignKey(f => f.TicketClassId);
            modelBuilder.Entity<Booking>().HasOne(f => f.Flight).WithMany(t => t.Bookings).HasForeignKey(f => f.FlightId);

            modelBuilder.Entity<Aircraft_Ticket>().HasOne(at => at.Aircraft).WithMany(a => a.Aircraft_Tickets).HasForeignKey(at => at.AircraftId);
            modelBuilder.Entity<Aircraft_Ticket>().HasOne(at => at.Ticket).WithMany(a => a.Aircraft_Tickets).HasForeignKey(at => at.TicketId);


            base.OnModelCreating(modelBuilder);
        }


    }
}
