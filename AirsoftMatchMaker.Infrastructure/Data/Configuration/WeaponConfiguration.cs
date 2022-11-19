using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Net.WebRequestMethods;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
    {
        private List<int> playerIds = new List<int>();
        public void Configure(EntityTypeBuilder<Weapon> builder)
        {
            builder.HasData(CreateWeapons());
        }
        private List<Weapon> CreateWeapons()
        {
            for (int i = 1; i <= 6; i++)
            {
                playerIds.Add(i);
            }
            List<Weapon> weapons = new List<Weapon>()
            {
                new Weapon()
                {
                    Id = 1,
                    Name = "Glock 17",
                    Description = "Small pistol",
                    ImageUrl = "https://arms-bg.com/wp-content/uploads/imported/2.6412_17_links_2000_1125_0-600x600.jpg",
                    FireRate = 300,
                    FeetPerSecond = 1.20m,
                    AverageAmmoExpendedPerGame = 40,
                    WeaponType = WeaponType.Pistol,
                    Price = 20,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon(),
                    
                },
                new Weapon()
                {
                    Id = 2,
                    Name = "Benelli M3",
                    Description = "Shotgun",
                    ImageUrl = "https://www.airsoft.bg/products/1334236938_160704__031226700_1656_02022011.jpg",
                    FireRate = 100,
                    FeetPerSecond = 1.30m,
                    AverageAmmoExpendedPerGame = 15,
                    WeaponType = WeaponType.Shotgun,
                    Price = 50,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()

                },
                new Weapon()
                {
                    Id = 3,
                    Name = "M4A1",
                    Description = "Popular Assault Rifle",
                    ImageUrl = "https://arms-bg.com/wp-content/uploads/2021/11/cyma-cm002a1-600x600.jpg",
                    FireRate = 666,
                    FeetPerSecond = 1.45m,
                    AverageAmmoExpendedPerGame = 90,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 100,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = 4,
                    Name = "AWP",
                    Description = "Sniper Rifle",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152193374_1.jpg",
                    FireRate = 20,
                    FeetPerSecond = 1.60m,
                    AverageAmmoExpendedPerGame = 15,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 130,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = 5,
                    Name = "Mp5",
                    Description = "Good Smg",
                    ImageUrl = "https://nelo-mill.com/wp-content/uploads/2019/07/2.6311_MP5A5_links_ret_613_400_0.jpg",
                    FireRate = 700,
                    FeetPerSecond = 1.10m,
                    AverageAmmoExpendedPerGame = 120,
                    WeaponType = WeaponType.SubmachineGun,
                    Price = 70,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = 6,
                    Name = "FAMAS",
                    Description = "Very fast fire rate",
                    ImageUrl = "https://shop.crgroup.bg/media/t44s4/2543.jpg",
                    FireRate = 1200,
                    FeetPerSecond = 1.25m,
                    AverageAmmoExpendedPerGame = 100,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 80,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = 7,
                    Name = "M249",
                    Description = "Machine gun with good fire rate and good accuracy",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152226012_6.jpg",
                    FireRate = 900,
                    FeetPerSecond = 1.35m,
                    AverageAmmoExpendedPerGame = 180,
                    WeaponType = WeaponType.Heavy,
                    Price = 150,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = 8,
                    Name = "Kar98k",
                    Description = "Old fashioned sniper rifle for classy people",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152190150_3.jpg",
                    FireRate = 15,
                    FeetPerSecond = 1.45m,
                    AverageAmmoExpendedPerGame = 20,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 110,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = 9,
                    Name = "AKM",
                    Description = "Versatile assault rifle with good accuracy",
                    ImageUrl = "https://www.airsoft.bg/products/1333793093_Kalashnikov-AKM-AEG_CG120914_airsoft_zm.jpg",
                    FireRate = 600,
                    FeetPerSecond = 1.30m,
                    AverageAmmoExpendedPerGame = 60,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 90,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = 10,
                    Name = "Minigun",
                    Description = "Overkill",
                    ImageUrl = "https://www.evike.com/images/large/34905.jpg",
                    FireRate = 3000,
                    FeetPerSecond = 1.50m,
                    AverageAmmoExpendedPerGame = 300,
                    WeaponType = WeaponType.Heavy,
                    Price = 250,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    VendorId = 1
                },
            };
            return weapons;
        }
        private int AddPlayerIdToWeapon()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, playerIds.Count - 1);
            int playerId = playerIds[randomIndex];
            playerIds.RemoveAt(randomIndex);
            return playerId;
        }
    }
}
