-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2025-02-10 12:03:36
-- --------------------------------------
-- Server version 10.4.28-MariaDB mariadb.org binary distribution


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of jogok
-- 

DROP TABLE IF EXISTS `jogok`;
CREATE TABLE IF NOT EXISTS `jogok` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Szint` int(1) NOT NULL,
  `Nev` varchar(64) NOT NULL,
  `Leiras` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Szint` (`Szint`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table jogok
-- 

/*!40000 ALTER TABLE `jogok` DISABLE KEYS */;
INSERT INTO `jogok`(`Id`,`Szint`,`Nev`,`Leiras`) VALUES(1,9,'Admin','Adminisztrátori szint');
/*!40000 ALTER TABLE `jogok` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FelhasznaloNev` varchar(100) NOT NULL,
  `TeljesNev` varchar(60) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Jogosultsag` int(1) NOT NULL,
  `Aktiv` int(1) NOT NULL,
  `RegisztracioDatuma` datetime DEFAULT current_timestamp(),
  `ProfilKepUtvonal` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `FelhasznaloNev` (`FelhasznaloNev`),
  UNIQUE KEY `Email` (`Email`),
  KEY `Email_2` (`Email`),
  KEY `Jogosultsag` (`Jogosultsag`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`Jogosultsag`) REFERENCES `jogok` (`Szint`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`Id`,`FelhasznaloNev`,`TeljesNev`,`SALT`,`HASH`,`Email`,`Jogosultsag`,`Aktiv`,`RegisztracioDatuma`,`ProfilKepUtvonal`) VALUES(1,'a','Kerényi Róbert','6qjpgYNTMMxN8O89WrqDQsgtmdmNf5GxSoDqvQS8x23jGw5bhtcucnoNkyQWY8jf','89cc9a5f2b18afcebbd73c6472d259076d27b4cc84968e88bf800c6dae4a2577','kerenyir@kkszki.hu',9,1,'2025-02-09 00:00:00','cat.jpg'),(10,'Robi','Kerényi Róbert','gHvDqLPWsXyOUxIzi73ktMcw8Kt3x0QocgT6x8nEkiAFSten3Guk7aznL3ZUvBGE','7c86369c5e5566eef439622b803e3688ea168277fbbec8ef8df58842f5657913','t@t.hu',9,1,'2025-02-08 00:00:00','default.jpg'),(16,'string','string','TZIGHC2rqf1CmLRSJdoXj8YyghKs7CLl6vz3Kd5iRd4zyLynrapRBBIMMjU7Ehmn','e26611a5e73123cf64f5de0c7b46d0d916c1ff3cfa2ddd1ae543b51fcb7d5bbd','string',9,1,'2025-02-08 00:00:00','default.jpg'),(17,'r','rttttt','fJGrlHo9kFySWm5wYP7HtILxQeI0kiX1qWI2Fvc5TC5Ff2lU0A09waIpdUhu0rGI','c8ff891d241701b729ef1ef5ab1860e299fffc2684d390ff25b419664501b809','ruszkaii@agysz-miskolc.hu',9,1,'2025-02-07 00:00:00','default.jpg');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2025-02-10 12:03:36
-- Total time: 0:0:0:0:121 (d:h:m:s:ms)
