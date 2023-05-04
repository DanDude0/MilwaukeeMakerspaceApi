/*
SQLyog Community
MySQL - 10.5.18-MariaDB-0+deb11u1-log : Database - area_funding
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`area_funding` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;

/*Table structure for table `bank_statement` */

CREATE TABLE `bank_statement` (
  `bank_statement_id` int(11) NOT NULL AUTO_INCREMENT,
  `account_name` varchar(255) NOT NULL,
  `time` datetime NOT NULL,
  `starting_balance` decimal(10,2) NOT NULL,
  `ending_balance` decimal(10,2) NOT NULL,
  `income` decimal(10,2) NOT NULL,
  `spending` decimal(10,2) NOT NULL,
  `transfers` decimal(10,2) NOT NULL,
  `fees` decimal(10,2) NOT NULL,
  PRIMARY KEY (`bank_statement_id`)
) ENGINE=InnoDB AUTO_INCREMENT=218 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `funds` */

CREATE TABLE `funds` (
  `funds_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `month` datetime NOT NULL,
  `building_purchase` decimal(6,2) NOT NULL,
  `threed_printer` decimal(6,2) NOT NULL,
  `bicycle_repair` decimal(6,2) NOT NULL,
  `casting` decimal(6,2) NOT NULL,
  `ceramic` decimal(6,2) NOT NULL,
  `cnc` decimal(6,2) NOT NULL,
  `cosplay` decimal(6,2) NOT NULL,
  `craft` decimal(6,2) NOT NULL,
  `dalek` decimal(6,2) NOT NULL,
  `electronic` decimal(6,2) NOT NULL,
  `finishing` decimal(6,2) NOT NULL,
  `forge` decimal(6,2) NOT NULL,
  `ham_radio` decimal(6,2) NOT NULL,
  `jewelry` decimal(6,2) NOT NULL,
  `lampworking` decimal(6,2) NOT NULL,
  `laser` decimal(6,2) NOT NULL,
  `leather` decimal(6,2) NOT NULL,
  `long_arm` decimal(6,2) NOT NULL,
  `makerfaire` decimal(6,2) NOT NULL,
  `metal` decimal(6,2) NOT NULL,
  `neon` decimal(6,2) NOT NULL,
  `paint` decimal(6,2) NOT NULL,
  `power_wheel` decimal(6,2) NOT NULL,
  `print` decimal(6,2) NOT NULL,
  `soda` decimal(6,2) NOT NULL,
  `stained_glass` decimal(6,2) NOT NULL,
  `tiger_lily` decimal(6,2) NOT NULL,
  `vacuum` decimal(6,2) NOT NULL,
  `welding` decimal(6,2) NOT NULL,
  `wood` decimal(6,2) NOT NULL,
  `total` decimal(6,2) NOT NULL,
  `members` int(11) NOT NULL,
  `general` int(11) NOT NULL,
  `family` int(11) NOT NULL,
  PRIMARY KEY (`funds_id`),
  UNIQUE KEY `month` (`month`)
) ENGINE=MyISAM AUTO_INCREMENT=1295084 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `spending` */

CREATE TABLE `spending` (
  `spending_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `time` datetime NOT NULL DEFAULT current_timestamp(),
  `reason` varchar(255) NOT NULL,
  `threed_printer` decimal(6,2) NOT NULL DEFAULT 0.00,
  `bicycle_repair` decimal(6,2) NOT NULL DEFAULT 0.00,
  `casting` decimal(6,2) NOT NULL DEFAULT 0.00,
  `ceramic` decimal(6,2) NOT NULL DEFAULT 0.00,
  `cnc` decimal(6,2) NOT NULL DEFAULT 0.00,
  `cosplay` decimal(6,2) NOT NULL DEFAULT 0.00,
  `craft` decimal(6,2) NOT NULL DEFAULT 0.00,
  `dalek` decimal(6,2) NOT NULL DEFAULT 0.00,
  `electronic` decimal(6,2) NOT NULL DEFAULT 0.00,
  `finishing` decimal(6,2) NOT NULL DEFAULT 0.00,
  `forge` decimal(6,2) NOT NULL DEFAULT 0.00,
  `ham_radio` decimal(6,2) NOT NULL DEFAULT 0.00,
  `jewelry` decimal(6,2) NOT NULL DEFAULT 0.00,
  `lampworking` decimal(6,2) NOT NULL DEFAULT 0.00,
  `laser` decimal(6,2) NOT NULL DEFAULT 0.00,
  `leather` decimal(6,2) NOT NULL DEFAULT 0.00,
  `long_arm` decimal(6,2) NOT NULL DEFAULT 0.00,
  `makerfaire` decimal(6,2) NOT NULL DEFAULT 0.00,
  `metal` decimal(6,2) NOT NULL DEFAULT 0.00,
  `neon` decimal(6,2) NOT NULL DEFAULT 0.00,
  `paint` decimal(6,2) NOT NULL DEFAULT 0.00,
  `power_wheel` decimal(6,2) NOT NULL DEFAULT 0.00,
  `print` decimal(6,2) NOT NULL DEFAULT 0.00,
  `soda` decimal(6,2) NOT NULL DEFAULT 0.00,
  `stained_glass` decimal(6,2) NOT NULL DEFAULT 0.00,
  `tiger_lily` decimal(6,2) NOT NULL DEFAULT 0.00,
  `vacuum` decimal(6,2) NOT NULL DEFAULT 0.00,
  `welding` decimal(6,2) NOT NULL DEFAULT 0.00,
  `wood` decimal(6,2) NOT NULL DEFAULT 0.00,
  PRIMARY KEY (`spending_id`),
  UNIQUE KEY `month` (`time`)
) ENGINE=MyISAM AUTO_INCREMENT=1985 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `current` */

DROP TABLE IF EXISTS `current`;

/*!50001 CREATE TABLE  `current`(
 `month` datetime ,
 `building_purchase` decimal(6,2) ,
 `threed_printer` decimal(6,2) ,
 `bicycle_repair` decimal(6,2) ,
 `casting` decimal(6,2) ,
 `ceramic` decimal(6,2) ,
 `cnc` decimal(6,2) ,
 `cosplay` decimal(6,2) ,
 `craft` decimal(6,2) ,
 `dalek` decimal(6,2) ,
 `electronic` decimal(6,2) ,
 `finishing` decimal(6,2) ,
 `forge` decimal(6,2) ,
 `ham_radio` decimal(6,2) ,
 `jewelry` decimal(6,2) ,
 `lampworking` decimal(6,2) ,
 `laser` decimal(6,2) ,
 `leather` decimal(6,2) ,
 `long_arm` decimal(6,2) ,
 `makerfaire` decimal(6,2) ,
 `metal` decimal(6,2) ,
 `neon` decimal(6,2) ,
 `paint` decimal(6,2) ,
 `power_wheel` decimal(6,2) ,
 `print` decimal(6,2) ,
 `soda` decimal(6,2) ,
 `stained_glass` decimal(6,2) ,
 `tiger_lily` decimal(6,2) ,
 `vacuum` decimal(6,2) ,
 `welding` decimal(6,2) ,
 `wood` decimal(6,2) ,
 `total` decimal(6,2) ,
 `members` int(11) ,
 `general` int(11) ,
 `family` int(11) 
)*/;

/*Table structure for table `history` */

DROP TABLE IF EXISTS `history`;

/*!50001 CREATE TABLE  `history`(
 `month` datetime ,
 `building_purchase` decimal(6,2) ,
 `threed_printer` decimal(6,2) ,
 `bicycle_repair` decimal(6,2) ,
 `casting` decimal(6,2) ,
 `ceramic` decimal(6,2) ,
 `cnc` decimal(6,2) ,
 `cosplay` decimal(6,2) ,
 `craft` decimal(6,2) ,
 `dalek` decimal(6,2) ,
 `electronic` decimal(6,2) ,
 `finishing` decimal(6,2) ,
 `forge` decimal(6,2) ,
 `ham_radio` decimal(6,2) ,
 `jewelry` decimal(6,2) ,
 `lampworking` decimal(6,2) ,
 `laser` decimal(6,2) ,
 `leather` decimal(6,2) ,
 `long_arm` decimal(6,2) ,
 `makerfaire` decimal(6,2) ,
 `metal` decimal(6,2) ,
 `neon` decimal(6,2) ,
 `paint` decimal(6,2) ,
 `power_wheel` decimal(6,2) ,
 `print` decimal(6,2) ,
 `soda` decimal(6,2) ,
 `stained_glass` decimal(6,2) ,
 `tiger_lily` decimal(6,2) ,
 `vacuum` decimal(6,2) ,
 `welding` decimal(6,2) ,
 `wood` decimal(6,2) ,
 `total` decimal(6,2) ,
 `members` int(11) ,
 `general` int(11) ,
 `family` int(11) 
)*/;

/*Table structure for table `ledger` */

DROP TABLE IF EXISTS `ledger`;

/*!50001 CREATE TABLE  `ledger`(
 `time` datetime /* mariadb-5.3 */ ,
 `reason` varchar(255) ,
 `threed_printer` decimal(6,2) ,
 `bicycle_repair` decimal(6,2) ,
 `casting` decimal(6,2) ,
 `ceramic` decimal(6,2) ,
 `cnc` decimal(6,2) ,
 `cosplay` decimal(6,2) ,
 `craft` decimal(6,2) ,
 `dalek` decimal(6,2) ,
 `electronic` decimal(6,2) ,
 `finishing` decimal(6,2) ,
 `forge` decimal(6,2) ,
 `ham_radio` decimal(6,2) ,
 `jewelry` decimal(6,2) ,
 `lampworking` decimal(6,2) ,
 `laser` decimal(6,2) ,
 `leather` decimal(6,2) ,
 `long_arm` decimal(6,2) ,
 `makerfaire` decimal(6,2) ,
 `metal` decimal(6,2) ,
 `neon` decimal(6,2) ,
 `paint` decimal(6,2) ,
 `power_wheel` decimal(6,2) ,
 `print` decimal(6,2) ,
 `soda` decimal(6,2) ,
 `stained_glass` decimal(6,2) ,
 `tiger_lily` decimal(6,2) ,
 `vacuum` decimal(6,2) ,
 `welding` decimal(6,2) ,
 `wood` decimal(6,2) 
)*/;

/*Table structure for table `balances` */

DROP TABLE IF EXISTS `balances`;

/*!50001 CREATE TABLE  `balances`(
 `threed_printer` decimal(28,2) ,
 `bicycle_repair` decimal(28,2) ,
 `casting` decimal(28,2) ,
 `ceramic` decimal(28,2) ,
 `cnc` decimal(28,2) ,
 `cosplay` decimal(28,2) ,
 `craft` decimal(28,2) ,
 `dalek` decimal(28,2) ,
 `electronic` decimal(28,2) ,
 `finishing` decimal(28,2) ,
 `forge` decimal(28,2) ,
 `ham_radio` decimal(28,2) ,
 `jewelry` decimal(28,2) ,
 `lampworking` decimal(28,2) ,
 `laser` decimal(28,2) ,
 `leather` decimal(28,2) ,
 `long_arm` decimal(28,2) ,
 `makerfaire` decimal(28,2) ,
 `metal` decimal(28,2) ,
 `neon` decimal(28,2) ,
 `paint` decimal(28,2) ,
 `power_wheel` decimal(28,2) ,
 `print` decimal(28,2) ,
 `soda` decimal(28,2) ,
 `stained_glass` decimal(28,2) ,
 `tiger_lily` decimal(28,2) ,
 `vacuum` decimal(28,2) ,
 `welding` decimal(28,2) ,
 `wood` decimal(28,2) 
)*/;

/*Table structure for table `total_committed` */

DROP TABLE IF EXISTS `total_committed`;

/*!50001 CREATE TABLE  `total_committed`(
 `total` decimal(56,2) 
)*/;

/*View structure for view current */

/*!50001 DROP TABLE IF EXISTS `current` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `current` AS (select `funds`.`month` AS `month`,`funds`.`building_purchase` AS `building_purchase`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`bicycle_repair` AS `bicycle_repair`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`forge` AS `forge`,`funds`.`ham_radio` AS `ham_radio`,`funds`.`jewelry` AS `jewelry`,`funds`.`lampworking` AS `lampworking`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`long_arm` AS `long_arm`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`neon` AS `neon`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`stained_glass` AS `stained_glass`,`funds`.`tiger_lily` AS `tiger_lily`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood`,`funds`.`total` AS `total`,`funds`.`members` AS `members`,`funds`.`general` AS `general`,`funds`.`family` AS `family` from `funds` order by `funds`.`funds_id` desc limit 1) */;

/*View structure for view history */

/*!50001 DROP TABLE IF EXISTS `history` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `history` AS (select `funds`.`month` AS `month`,`funds`.`building_purchase` AS `building_purchase`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`bicycle_repair` AS `bicycle_repair`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`forge` AS `forge`,`funds`.`ham_radio` AS `ham_radio`,`funds`.`jewelry` AS `jewelry`,`funds`.`lampworking` AS `lampworking`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`long_arm` AS `long_arm`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`neon` AS `neon`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`stained_glass` AS `stained_glass`,`funds`.`tiger_lily` AS `tiger_lily`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood`,`funds`.`total` AS `total`,`funds`.`members` AS `members`,`funds`.`general` AS `general`,`funds`.`family` AS `family` from `funds` where dayofmonth(`funds`.`month`) = 1 and hour(`funds`.`month`) = 0 group by year(`funds`.`month`),month(`funds`.`month`) order by year(`funds`.`month`) desc,month(`funds`.`month`) desc,dayofmonth(`funds`.`month`),hour(`funds`.`month`),minute(`funds`.`month`)) */;

/*View structure for view ledger */

/*!50001 DROP TABLE IF EXISTS `ledger` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `ledger` AS select `funds`.`month` AS `time`,'Monthly Funding' AS `reason`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`bicycle_repair` AS `bicycle_repair`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`forge` AS `forge`,`funds`.`ham_radio` AS `ham_radio`,`funds`.`jewelry` AS `jewelry`,`funds`.`lampworking` AS `lampworking`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`long_arm` AS `long_arm`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`neon` AS `neon`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`stained_glass` AS `stained_glass`,`funds`.`tiger_lily` AS `tiger_lily`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood` from `funds` where dayofmonth(`funds`.`month`) = 1 and hour(`funds`.`month`) = 0 group by year(`funds`.`month`),month(`funds`.`month`) union select `spending`.`time` AS `time`,`spending`.`reason` AS `reason`,-`spending`.`threed_printer` AS `threed_printer`,-`spending`.`bicycle_repair` AS `bicycle_repair`,-`spending`.`casting` AS `casting`,-`spending`.`ceramic` AS `ceramic`,-`spending`.`cnc` AS `cnc`,-`spending`.`cosplay` AS `cosplay`,-`spending`.`craft` AS `craft`,-`spending`.`dalek` AS `dalek`,-`spending`.`electronic` AS `electronic`,-`spending`.`finishing` AS `finishing`,-`spending`.`forge` AS `forge`,-`spending`.`ham_radio` AS `ham_radio`,-`spending`.`jewelry` AS `jewelry`,-`spending`.`lampworking` AS `lampworking`,-`spending`.`laser` AS `laser`,-`spending`.`leather` AS `leather`,-`spending`.`long_arm` AS `long_arm`,-`spending`.`makerfaire` AS `makerfaire`,-`spending`.`metal` AS `metal`,-`spending`.`neon` AS `neon`,-`spending`.`paint` AS `paint`,-`spending`.`power_wheel` AS `power_wheel`,-`spending`.`print` AS `print`,-`spending`.`soda` AS `soda`,-`spending`.`stained_glass` AS `stained_glass`,-`spending`.`tiger_lily` AS `tiger_lily`,-`spending`.`vacuum` AS `vacuum`,-`spending`.`welding` AS `welding`,-`spending`.`wood` AS `wood` from `spending` order by `time` */;

/*View structure for view balances */

/*!50001 DROP TABLE IF EXISTS `balances` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `balances` AS select sum(`ledger`.`threed_printer`) AS `threed_printer`,sum(`ledger`.`bicycle_repair`) AS `bicycle_repair`,sum(`ledger`.`casting`) AS `casting`,sum(`ledger`.`ceramic`) AS `ceramic`,sum(`ledger`.`cnc`) AS `cnc`,sum(`ledger`.`cosplay`) AS `cosplay`,sum(`ledger`.`craft`) AS `craft`,sum(`ledger`.`dalek`) AS `dalek`,sum(`ledger`.`electronic`) AS `electronic`,sum(`ledger`.`finishing`) AS `finishing`,sum(`ledger`.`forge`) AS `forge`,sum(`ledger`.`ham_radio`) AS `ham_radio`,sum(`ledger`.`jewelry`) AS `jewelry`,sum(`ledger`.`lampworking`) AS `lampworking`,sum(`ledger`.`laser`) AS `laser`,sum(`ledger`.`leather`) AS `leather`,sum(`ledger`.`long_arm`) AS `long_arm`,sum(`ledger`.`makerfaire`) AS `makerfaire`,sum(`ledger`.`metal`) AS `metal`,sum(`ledger`.`neon`) AS `neon`,sum(`ledger`.`paint`) AS `paint`,sum(`ledger`.`power_wheel`) AS `power_wheel`,sum(`ledger`.`print`) AS `print`,sum(`ledger`.`soda`) AS `soda`,sum(`ledger`.`stained_glass`) AS `stained_glass`,sum(`ledger`.`tiger_lily`) AS `tiger_lily`,sum(`ledger`.`vacuum`) AS `vacuum`,sum(`ledger`.`welding`) AS `welding`,sum(`ledger`.`wood`) AS `wood` from `ledger` */;

/*View structure for view total_committed */

/*!50001 DROP TABLE IF EXISTS `total_committed` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `total_committed` AS (select `balances`.`threed_printer` + `balances`.`bicycle_repair` + `balances`.`casting` + `balances`.`ceramic` + `balances`.`cnc` + `balances`.`cosplay` + `balances`.`craft` + `balances`.`dalek` + `balances`.`electronic` + `balances`.`finishing` + `balances`.`forge` + `balances`.`ham_radio` + `balances`.`jewelry` + `balances`.`lampworking` + `balances`.`laser` + `balances`.`leather` + `balances`.`long_arm` + `balances`.`makerfaire` + `balances`.`metal` + `balances`.`neon` + `balances`.`paint` + `balances`.`power_wheel` + `balances`.`print` + `balances`.`soda` + `balances`.`stained_glass` + `balances`.`tiger_lily` + `balances`.`vacuum` + `balances`.`welding` + `balances`.`wood` AS `total` from `balances`) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
