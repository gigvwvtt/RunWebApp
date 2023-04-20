using Microsoft.AspNetCore.Identity;
using RunWebApp.Data.Enum;
using RunWebApp.Models;

namespace RunWebApp.Data;

public class Seed
{
	public static void SeedData(IApplicationBuilder applicationBuilder)
	{
		using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
		{
			var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

			context.Database.EnsureCreated();

			if (!context.Clubs.Any())
			{
				context.Clubs.AddRange(new List<Club>()
				{
					new Club()
					{
						Title = "Running Club 1",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first cinema",
						ClubCategory = ClubCategory.City,
						Address = new Address()
						{
							Street = "ulitsa Malysheva",
							City = "Yekaterinburg",
							State = "Sverdlovsk Oblast"
						}
					 },
					new Club()
					{
						Title = "Running Club 2",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first cinema",
						ClubCategory = ClubCategory.Endurance,
						Address = new Address()
						{
							Street = "prospekt Lenina",
							City = "Yekaterinburg",
							State = "Sverdlovsk Oblast"
						}
					},
					new Club()
					{
						Title = "Running Club 3",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first club",
						ClubCategory = ClubCategory.Trail,
						Address = new Address()
						{
							Street = "prospekt Lenina",
							City = "Yekaterinburg",
							State = "Sverdlovsk Oblast"
						}
					},
					new Club()
					{
						Title = "Running Club 3",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first club",
						ClubCategory = ClubCategory.City,
						Address = new Address()
						{
							Street = "prospekt Lenina",
							City = "Nizhny Tagil",
							State = "Sverdlovsk Oblast"
						}
					}
				});
				context.SaveChanges();
			}
			//Races
			if (!context.Races.Any())
			{
				context.Races.AddRange(new List<Race>()
				{
					new Race()
					{
						Title = "Running Race 1",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first race",
						RaceCategory = RaceCategory.Marathon,
						Address = new Address()
						{
							Street = "prospekt Mira",
							City = "Yekaterinburg",
							State = "Sverdlovsk Oblast"
						}
					},
					new Race()
					{
						Title = "Running Race 2",
						Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
						Description = "This is the description of the first race",
						RaceCategory = RaceCategory.Ultra,
						AddressId = 5,
						Address = new Address()
						{
							Street = "prospekt Mira",
							City = "Nizhny Tagil",
							State = "Sverdlovsk Oblast"
						}
					}
				});
				context.SaveChanges();
			}
		}
	}

	public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
	{
		using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
		{
			//Roles
			var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
				await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
			if (!await roleManager.RoleExistsAsync(UserRoles.User))
				await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			//Users
			var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
			string adminUserEmail = "developer@example.com";

			var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
			if (adminUser == null)
			{
				var newAdminUser = new AppUser()
				{
					UserName = "developer",
					Email = adminUserEmail,
					EmailConfirmed = true,
					Address = new Address()
					{
						Street = "Yevropeyskaya ulitsa",
						City = "Sovkhoznyy",
						State = "Sverdlovsk Oblast"
					}
				};
				await userManager.CreateAsync(newAdminUser, "Devpass@1234?");
				await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
			}

			string appUserEmail = "user@etickets.com";

			var appUser = await userManager.FindByEmailAsync(appUserEmail);
			if (appUser == null)
			{
				var newAppUser = new AppUser()
				{
					UserName = "app-user",
					Email = appUserEmail,
					EmailConfirmed = true,
					Address = new Address()
					{
						Street = "Yevropeyskaya ulitsa",
						City = "Sovkhoznyy",
						State = "Sverdlovsk Oblast"
					}
				};
				await userManager.CreateAsync(newAppUser, "Devpass@1234?");
				await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
			}
		}
	}
}