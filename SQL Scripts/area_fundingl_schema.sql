/*
SQLyog Community
MySQL - 10.1.26-MariaDB-0+deb9u1 : Database - area_funding
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`area_funding` /*!40100 DEFAULT CHARACTER SET latin1 */;

/*Table structure for table `funds` */

CREATE TABLE `funds` (
  `funds_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `month` datetime NOT NULL,
  `building_purchase` decimal(6,2) NOT NULL,
  `threed_printer` decimal(6,2) NOT NULL,
  `annodizing` decimal(6,2) NOT NULL,
  `forge` decimal(6,2) NOT NULL,
  `casting` decimal(6,2) NOT NULL,
  `ceramic` decimal(6,2) NOT NULL,
  `cnc` decimal(6,2) NOT NULL,
  `cosplay` decimal(6,2) NOT NULL,
  `craft` decimal(6,2) NOT NULL,
  `dalek` decimal(6,2) NOT NULL,
  `digital` decimal(6,2) NOT NULL DEFAULT '0.00',
  `electronic` decimal(6,2) NOT NULL,
  `finishing` decimal(6,2) NOT NULL,
  `jewelry` decimal(6,2) NOT NULL,
  `laser` decimal(6,2) NOT NULL,
  `leather` decimal(6,2) NOT NULL,
  `makerfaire` decimal(6,2) NOT NULL,
  `metal` decimal(6,2) NOT NULL,
  `paint` decimal(6,2) NOT NULL,
  `power_wheel` decimal(6,2) NOT NULL,
  `print` decimal(6,2) NOT NULL,
  `soda` decimal(6,2) NOT NULL,
  `vacuum` decimal(6,2) NOT NULL,
  `welding` decimal(6,2) NOT NULL,
  `wood` decimal(6,2) NOT NULL,
  `total` decimal(6,2) NOT NULL,
  `members` int(11) NOT NULL,
  `general` int(11) NOT NULL,
  `family` int(11) NOT NULL,
  PRIMARY KEY (`funds_id`),
  UNIQUE KEY `month` (`month`)
) ENGINE=MyISAM AUTO_INCREMENT=1012693 DEFAULT CHARSET=latin1;

/*Table structure for table `spending` */

CREATE TABLE `spending` (
  `spending_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `time` datetime NOT NULL,
  `reason` varchar(255) NOT NULL,
  `threed_printer` decimal(6,2) NOT NULL,
  `annodizing` decimal(6,2) NOT NULL,
  `forge` decimal(6,2) NOT NULL,
  `casting` decimal(6,2) NOT NULL,
  `ceramic` decimal(6,2) NOT NULL,
  `cnc` decimal(6,2) NOT NULL,
  `cosplay` decimal(6,2) NOT NULL,
  `craft` decimal(6,2) NOT NULL,
  `dalek` decimal(6,2) NOT NULL,
  `digital` decimal(6,2) NOT NULL,
  `electronic` decimal(6,2) NOT NULL,
  `finishing` decimal(6,2) NOT NULL,
  `jewelry` decimal(6,2) NOT NULL,
  `laser` decimal(6,2) NOT NULL,
  `leather` decimal(6,2) NOT NULL,
  `makerfaire` decimal(6,2) NOT NULL,
  `metal` decimal(6,2) NOT NULL,
  `paint` decimal(6,2) NOT NULL,
  `power_wheel` decimal(6,2) NOT NULL,
  `print` decimal(6,2) NOT NULL,
  `soda` decimal(6,2) NOT NULL,
  `vacuum` decimal(6,2) NOT NULL,
  `welding` decimal(6,2) NOT NULL,
  `wood` decimal(6,2) NOT NULL,
  PRIMARY KEY (`spending_id`),
  UNIQUE KEY `month` (`time`)
) ENGINE=MyISAM AUTO_INCREMENT=237 DEFAULT CHARSET=latin1;

/*Table structure for table `balances` */

DROP TABLE IF EXISTS `balances`;

/*!50001 CREATE TABLE  `balances`(
 `threed_printer` decimal(29,2) ,
 `annodizing` decimal(29,2) ,
 `forge` decimal(29,2) ,
 `casting` decimal(29,2) ,
 `ceramic` decimal(29,2) ,
 `cnc` decimal(29,2) ,
 `cosplay` decimal(29,2) ,
 `craft` decimal(29,2) ,
 `dalek` decimal(29,2) ,
 `digital` decimal(29,2) ,
 `electronic` decimal(29,2) ,
 `finishing` decimal(29,2) ,
 `jewelry` decimal(29,2) ,
 `laser` decimal(29,2) ,
 `leather` decimal(29,2) ,
 `makerfaire` decimal(29,2) ,
 `metal` decimal(29,2) ,
 `paint` decimal(29,2) ,
 `power_wheel` decimal(29,2) ,
 `print` decimal(29,2) ,
 `soda` decimal(29,2) ,
 `vacuum` decimal(29,2) ,
 `welding` decimal(29,2) ,
 `wood` decimal(29,2) 
)*/;

/*Table structure for table `current` */

DROP TABLE IF EXISTS `current`;

/*!50001 CREATE TABLE  `current`(
 `month` datetime ,
 `building_purchase` decimal(6,2) ,
 `threed_printer` decimal(6,2) ,
 `annodizing` decimal(6,2) ,
 `forge` decimal(6,2) ,
 `casting` decimal(6,2) ,
 `ceramic` decimal(6,2) ,
 `cnc` decimal(6,2) ,
 `cosplay` decimal(6,2) ,
 `craft` decimal(6,2) ,
 `dalek` decimal(6,2) ,
 `digital` decimal(6,2) ,
 `electronic` decimal(6,2) ,
 `finishing` decimal(6,2) ,
 `jewelry` decimal(6,2) ,
 `laser` decimal(6,2) ,
 `leather` decimal(6,2) ,
 `makerfaire` decimal(6,2) ,
 `metal` decimal(6,2) ,
 `paint` decimal(6,2) ,
 `power_wheel` decimal(6,2) ,
 `print` decimal(6,2) ,
 `soda` decimal(6,2) ,
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
 `annodizing` decimal(6,2) ,
 `forge` decimal(6,2) ,
 `casting` decimal(6,2) ,
 `ceramic` decimal(6,2) ,
 `cnc` decimal(6,2) ,
 `cosplay` decimal(6,2) ,
 `craft` decimal(6,2) ,
 `dalek` decimal(6,2) ,
 `digital` decimal(6,2) ,
 `electronic` decimal(6,2) ,
 `finishing` decimal(6,2) ,
 `jewelry` decimal(6,2) ,
 `laser` decimal(6,2) ,
 `leather` decimal(6,2) ,
 `makerfaire` decimal(6,2) ,
 `metal` decimal(6,2) ,
 `paint` decimal(6,2) ,
 `power_wheel` decimal(6,2) ,
 `print` decimal(6,2) ,
 `soda` decimal(6,2) ,
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
 `time` datetime ,
 `reason` varchar(255) ,
 `threed_printer` decimal(7,2) ,
 `annodizing` decimal(7,2) ,
 `forge` decimal(7,2) ,
 `casting` decimal(7,2) ,
 `ceramic` decimal(7,2) ,
 `cnc` decimal(7,2) ,
 `cosplay` decimal(7,2) ,
 `craft` decimal(7,2) ,
 `dalek` decimal(7,2) ,
 `digital` decimal(7,2) ,
 `electronic` decimal(7,2) ,
 `finishing` decimal(7,2) ,
 `jewelry` decimal(7,2) ,
 `laser` decimal(7,2) ,
 `leather` decimal(7,2) ,
 `makerfaire` decimal(7,2) ,
 `metal` decimal(7,2) ,
 `paint` decimal(7,2) ,
 `power_wheel` decimal(7,2) ,
 `print` decimal(7,2) ,
 `soda` decimal(7,2) ,
 `vacuum` decimal(7,2) ,
 `welding` decimal(7,2) ,
 `wood` decimal(7,2) 
)*/;

/*Table structure for table `total_committed` */

DROP TABLE IF EXISTS `total_committed`;

/*!50001 CREATE TABLE  `total_committed`(
 `total` decimal(52,2) 
)*/;

/*View structure for view balances */

/*!50001 DROP TABLE IF EXISTS `balances` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `balances` AS select sum(`ledger`.`threed_printer`) AS `threed_printer`,sum(`ledger`.`annodizing`) AS `annodizing`,sum(`ledger`.`forge`) AS `forge`,sum(`ledger`.`casting`) AS `casting`,sum(`ledger`.`ceramic`) AS `ceramic`,sum(`ledger`.`cnc`) AS `cnc`,sum(`ledger`.`cosplay`) AS `cosplay`,sum(`ledger`.`craft`) AS `craft`,sum(`ledger`.`dalek`) AS `dalek`,sum(`ledger`.`digital`) AS `digital`,sum(`ledger`.`electronic`) AS `electronic`,sum(`ledger`.`finishing`) AS `finishing`,sum(`ledger`.`jewelry`) AS `jewelry`,sum(`ledger`.`laser`) AS `laser`,sum(`ledger`.`leather`) AS `leather`,sum(`ledger`.`makerfaire`) AS `makerfaire`,sum(`ledger`.`metal`) AS `metal`,sum(`ledger`.`paint`) AS `paint`,sum(`ledger`.`power_wheel`) AS `power_wheel`,sum(`ledger`.`print`) AS `print`,sum(`ledger`.`soda`) AS `soda`,sum(`ledger`.`vacuum`) AS `vacuum`,sum(`ledger`.`welding`) AS `welding`,sum(`ledger`.`wood`) AS `wood` from `ledger` */;

/*View structure for view current */

/*!50001 DROP TABLE IF EXISTS `current` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `current` AS (select `funds`.`month` AS `month`,`funds`.`building_purchase` AS `building_purchase`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`annodizing` AS `annodizing`,`funds`.`forge` AS `forge`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`digital` AS `digital`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`jewelry` AS `jewelry`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood`,`funds`.`total` AS `total`,`funds`.`members` AS `members`,`funds`.`general` AS `general`,`funds`.`family` AS `family` from `funds` order by `funds`.`funds_id` desc limit 1) */;

/*View structure for view history */

/*!50001 DROP TABLE IF EXISTS `history` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `history` AS (select `funds`.`month` AS `month`,`funds`.`building_purchase` AS `building_purchase`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`annodizing` AS `annodizing`,`funds`.`forge` AS `forge`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`digital` AS `digital`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`jewelry` AS `jewelry`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood`,`funds`.`total` AS `total`,`funds`.`members` AS `members`,`funds`.`general` AS `general`,`funds`.`family` AS `family` from `funds` where ((dayofmonth(`funds`.`month`) = 1) and (hour(`funds`.`month`) = 0)) group by year(`funds`.`month`),month(`funds`.`month`) order by year(`funds`.`month`) desc,month(`funds`.`month`) desc,dayofmonth(`funds`.`month`),hour(`funds`.`month`),minute(`funds`.`month`)) */;

/*View structure for view ledger */

/*!50001 DROP TABLE IF EXISTS `ledger` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `ledger` AS select `funds`.`month` AS `time`,'Monthly Funding' AS `reason`,`funds`.`threed_printer` AS `threed_printer`,`funds`.`annodizing` AS `annodizing`,`funds`.`forge` AS `forge`,`funds`.`casting` AS `casting`,`funds`.`ceramic` AS `ceramic`,`funds`.`cnc` AS `cnc`,`funds`.`cosplay` AS `cosplay`,`funds`.`craft` AS `craft`,`funds`.`dalek` AS `dalek`,`funds`.`digital` AS `digital`,`funds`.`electronic` AS `electronic`,`funds`.`finishing` AS `finishing`,`funds`.`jewelry` AS `jewelry`,`funds`.`laser` AS `laser`,`funds`.`leather` AS `leather`,`funds`.`makerfaire` AS `makerfaire`,`funds`.`metal` AS `metal`,`funds`.`paint` AS `paint`,`funds`.`power_wheel` AS `power_wheel`,`funds`.`print` AS `print`,`funds`.`soda` AS `soda`,`funds`.`vacuum` AS `vacuum`,`funds`.`welding` AS `welding`,`funds`.`wood` AS `wood` from `funds` where ((dayofmonth(`funds`.`month`) = 1) and (hour(`funds`.`month`) = 0)) group by year(`funds`.`month`),month(`funds`.`month`) union select `spending`.`time` AS `time`,`spending`.`reason` AS `reason`,-(`spending`.`threed_printer`) AS `threed_printer`,-(`spending`.`annodizing`) AS `annodizing`,-(`spending`.`forge`) AS `forge`,-(`spending`.`casting`) AS `casting`,-(`spending`.`ceramic`) AS `ceramic`,-(`spending`.`cnc`) AS `cnc`,-(`spending`.`cosplay`) AS `cosplay`,-(`spending`.`craft`) AS `craft`,-(`spending`.`dalek`) AS `dalek`,-(`spending`.`digital`) AS `digital`,-(`spending`.`electronic`) AS `electronic`,-(`spending`.`finishing`) AS `finishing`,-(`spending`.`jewelry`) AS `jewelry`,-(`spending`.`laser`) AS `laser`,-(`spending`.`leather`) AS `leather`,-(`spending`.`makerfaire`) AS `makerfaire`,-(`spending`.`metal`) AS `metal`,-(`spending`.`paint`) AS `paint`,-(`spending`.`power_wheel`) AS `power_wheel`,-(`spending`.`print`) AS `print`,-(`spending`.`soda`) AS `soda`,-(`spending`.`vacuum`) AS `vacuum`,-(`spending`.`welding`) AS `welding`,-(`spending`.`wood`) AS `wood` from `spending` order by `time` */;

/*View structure for view total_committed */

/*!50001 DROP TABLE IF EXISTS `total_committed` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`djonke`@`%` SQL SECURITY DEFINER VIEW `total_committed` AS (select (((((((((((((((((((((((`balances`.`threed_printer` + `balances`.`annodizing`) + `balances`.`forge`) + `balances`.`casting`) + `balances`.`ceramic`) + `balances`.`cnc`) + `balances`.`cosplay`) + `balances`.`craft`) + `balances`.`dalek`) + `balances`.`digital`) + `balances`.`electronic`) + `balances`.`finishing`) + `balances`.`jewelry`) + `balances`.`laser`) + `balances`.`leather`) + `balances`.`makerfaire`) + `balances`.`metal`) + `balances`.`paint`) + `balances`.`power_wheel`) + `balances`.`print`) + `balances`.`soda`) + `balances`.`vacuum`) + `balances`.`welding`) + `balances`.`wood`) AS `total` from `balances`) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
