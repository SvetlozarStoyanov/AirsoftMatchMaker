# AirsoftMatchMaker

The site's objective is to mimic a matchmaking service.

There are 5 type of users - Admin, Matchmaker, Vendor, Player and Guest User.

-- Guest User
This user can only bet on games and access information about users, shop items(ammo boxes, clothes and weapons) , games (games, maps and gamemodes) and players.
 
--Matchmaker
Can create games by matching two teams together. Collects entry fee from games when they are finished. 
Can also add maps and game modes.
Finalises games he created after they are finished.
Cannot bet on games.

--Vendor
Imports items and sells them. Pays half of the price of each item he imports. Can also bet on games.

--Player
Participates in games and can also bet (not if his team is in the game).
Buys items from the shop (ammo boxes, clothes and weapons).
Can request to join/leave teams.

--Administrator
Grants or denies requested roles (Matchmaker,Player,Vendor).
Can also finalise games(regardless of who created them.)

Each user has his own area.
The site's currency is credits.
Bets are in the american betting system style => https://www.legalsportsreport.com/sports-betting/american-odds/
After they are created games have their betting odds calculated and updated when necessary.
A game's betting line moves after bets are placed by users and when players, in given game, buy weapons.
Bets are paid out after a game is finished.


For demo purposes each user's password is password and special requirements for passwords have been disabled.
Unit tests and site documentation are unfinished.

!!!! On first startup delete existing migrations and add new migration default project should be AirsoftMatchMaker.Infrastructure



