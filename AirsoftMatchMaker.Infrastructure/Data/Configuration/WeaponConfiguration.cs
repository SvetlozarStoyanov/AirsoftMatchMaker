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
            for (int i = 1; i <= 15; i++)
            {
                playerIds.Add(i);
            }
            int index = 1;
            List<Weapon> weapons = new List<Weapon>()
            {
                new Weapon()
                {
                    Id = index++,
                    Name = "Glock 17",
                    Description = "Small pistol",
                    ImageUrl = "https://arms-bg.com/wp-content/uploads/imported/2.6412_17_links_2000_1125_0-600x600.jpg",
                    FireRate = 300,
                    FeetPerSecond = 120,
                    AverageAmmoExpendedPerGame = 40,
                    WeaponType = WeaponType.Pistol,
                    Price = 20,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Benelli M3",
                    Description = "Shotgun",
                    ImageUrl = "https://www.airsoft.bg/products/1334236938_160704__031226700_1656_02022011.jpg",
                    FireRate = 100,
                    FeetPerSecond = 150,
                    AverageAmmoExpendedPerGame = 15,
                    WeaponType = WeaponType.Shotgun,
                    Price = 50,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()

                },
                new Weapon()
                {
                    Id = index++,
                    Name = "M4A1",
                    Description = "Popular Assault Rifle",
                    ImageUrl = "https://arms-bg.com/wp-content/uploads/2021/11/cyma-cm002a1-600x600.jpg",
                    FireRate = 666,
                    FeetPerSecond = 300,
                    AverageAmmoExpendedPerGame = 90,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 100,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "AWP",
                    Description = "Sniper Rifle",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152193374_1.jpg",
                    FireRate = 20,
                    FeetPerSecond = 500,
                    AverageAmmoExpendedPerGame = 15,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 130,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Mp5",
                    Description = "Good Smg",
                    ImageUrl = "https://nelo-mill.com/wp-content/uploads/2019/07/2.6311_MP5A5_links_ret_613_400_0.jpg",
                    FireRate = 700,
                    FeetPerSecond = 110,
                    AverageAmmoExpendedPerGame = 120,
                    WeaponType = WeaponType.SubmachineGun,
                    Price = 70,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon()

                },
                new Weapon()
                {
                    Id = index++,
                    Name = "FAMAS",
                    Description = "Very fast fire rate",
                    ImageUrl = "https://shop.crgroup.bg/media/t44s4/2543.jpg",
                    FireRate = 1200,
                    FeetPerSecond = 280,
                    AverageAmmoExpendedPerGame = 100,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 80,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "M249",
                    Description = "Machine gun with good fire rate and good accuracy",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152226012_6.jpg",
                    FireRate = 900,
                    FeetPerSecond = 260,
                    AverageAmmoExpendedPerGame = 180,
                    WeaponType = WeaponType.Heavy,
                    Price = 150,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Kar98k",
                    Description = "Old fashioned sniper rifle for classy people",
                    ImageUrl = "https://cqb.bg/wp-content/uploads/1152190150_3.jpg",
                    FireRate = 15,
                    FeetPerSecond = 400,
                    AverageAmmoExpendedPerGame = 20,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 110,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "AKM",
                    Description = "Versatile assault rifle with good accuracy",
                    ImageUrl = "https://www.airsoft.bg/products/1333793093_Kalashnikov-AKM-AEG_CG120914_airsoft_zm.jpg",
                    FireRate = 600,
                    FeetPerSecond = 240,
                    AverageAmmoExpendedPerGame = 60,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 90,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Minigun",
                    Description = "Overkill",
                    ImageUrl = "https://www.evike.com/images/large/34905.jpg",
                    FireRate = 3000,
                    FeetPerSecond = 290,
                    AverageAmmoExpendedPerGame = 300,
                    WeaponType = WeaponType.Heavy,
                    Price = 250,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "USP",
                    Description = "Small pistol. Good in tight quarters.",
                    ImageUrl = "https://i.pinimg.com/736x/92/86/dc/9286dcbb94e7faf0d648e63dd199de2f--products-is-.jpg",
                    FireRate = 300,
                    FeetPerSecond = 110,
                    AverageAmmoExpendedPerGame = 40,
                    WeaponType = WeaponType.Pistol,
                    Price = 25,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Galil",
                    Description = "Assault rifle good in most ranges.",
                    ImageUrl = "https://iwi.net/wp-content/uploads/2021/08/ACE_22_IWI_3687.jpg",
                    FireRate = 666,
                    FeetPerSecond = 200,
                    AverageAmmoExpendedPerGame = 70,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 60,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Mosin Nagant",
                    Description = "Old sniper rifle frow WW2.",
                    ImageUrl = "https://static3.gunfire.com/eng_pl_Mosin-Nagant-1891-30-rifle-replica-with-PU-scope-1152227065_1.webp",
                    FireRate = 15,
                    FeetPerSecond = 240,
                    AverageAmmoExpendedPerGame = 10,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 50,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "L85A2",
                    Description = "British bullpup assault rifle",
                    ImageUrl = "https://static4.gunfire.com/eng_pl_L85A2-Assault-Rifle-Replica-1152213851_1.webp",
                    FireRate = 650,
                    FeetPerSecond = 400,
                    AverageAmmoExpendedPerGame = 90,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 80,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Weapon()
                {
                    Id = index++,//15
                    Name = "Glock 18 auto",
                    Description = "Small automatic pistol",
                    ImageUrl = "https://media.mwstatic.com/product-images/src/alt1/850/850613a1.jpg?imwidth=480",
                    FireRate = 900,
                    FeetPerSecond = 125,
                    AverageAmmoExpendedPerGame = 120,
                    WeaponType = WeaponType.Pistol,
                    Price = 30,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    PlayerId = AddPlayerIdToWeapon()

                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Winchester model 1887",
                    Description = "Only for terminators",
                    ImageUrl = "https://taylorsfirearms.com/media/catalog/product/cache/a309b6cb2676967c1a0c3ab51e5fa3c7/1/8/1887bl-l_2641_2_.jpg",
                    FireRate = 50,
                    FeetPerSecond = 140,
                    AverageAmmoExpendedPerGame = 20,
                    WeaponType = WeaponType.Shotgun,
                    Price = 70,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "M16A3",
                    Description = "Burst fire  assault rifle .",
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1333/2651/products/Copy_of_M16-A3-01_grande.jpg?v=1571467240",
                    FireRate = 400,
                    FeetPerSecond = 250,
                    AverageAmmoExpendedPerGame = 45,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 60,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    VendorId = 1

                },
                new Weapon()
                {
                    Id = index++,
                    Name = "M4A1-S",
                    Description = "Popular Silenced Assault Rifle",
                    ImageUrl = "https://esportzbet.com/wp-content/uploads/2019/05/dw1-min.png",
                    FireRate = 600,
                    FeetPerSecond = 310,
                    AverageAmmoExpendedPerGame = 75,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 110,
                    PreferedEngagementDistance = PreferedEngagementDistance.Medium,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "M40A5",
                    Description = "Sniper Rifle good for long range.",
                    ImageUrl = "https://i0.wp.com/cms.sofrep.com/wp-content/uploads/2013/07/M40A5.jpg?fit=562%2C198&ssl=1",
                    FireRate = 20,
                    FeetPerSecond = 450,
                    AverageAmmoExpendedPerGame = 15,
                    WeaponType = WeaponType.SniperRifle,
                    Price = 120,
                    PreferedEngagementDistance = PreferedEngagementDistance.Long,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = index++,
                    Name = "Kriss Vector",
                    Description = "Good Smg with insane fire rate",
                    ImageUrl = "https://static4.gunfire.com/eng_pl_KRISS-Vector-Submachine-Gun-Replica-Half-Tan-1152223174_6.webp",
                    FireRate = 1200,
                    FeetPerSecond = 100,
                    AverageAmmoExpendedPerGame = 150,
                    WeaponType = WeaponType.SubmachineGun,
                    Price = 100,
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
