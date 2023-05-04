/*
SQLyog Community
MySQL - 10.5.18-MariaDB-0+deb11u1-log : Database - access_control
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`access_control` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;

/*Table structure for table `attempt` */

CREATE TABLE `attempt` (
  `attempt_id` int(11) NOT NULL AUTO_INCREMENT,
  `keycode` varchar(255) NOT NULL DEFAULT '',
  `member_id` int(11) NOT NULL,
  `reader_id` int(11) NOT NULL,
  `access_granted` tinyint(1) NOT NULL DEFAULT 0,
  `login` tinyint(1) NOT NULL DEFAULT 0,
  `logout` tinyint(1) NOT NULL DEFAULT 0,
  `action` varchar(255) NOT NULL DEFAULT '',
  `attempt_time` datetime /* mariadb-5.3 */ NOT NULL,
  PRIMARY KEY (`attempt_id`)
) ENGINE=MyISAM AUTO_INCREMENT=1350525 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `charge` */

CREATE TABLE `charge` (
  `charge_id` int(11) NOT NULL AUTO_INCREMENT,
  `member_id` int(11) NOT NULL,
  `reader_id` int(11) NOT NULL,
  `charge_time` datetime NOT NULL,
  `amount` decimal(8,2) NOT NULL,
  `description` varchar(4096) NOT NULL,
  `invoice_id` bigint(20) DEFAULT NULL,
  `document_number` varchar(255) DEFAULT NULL,
  `invoice_line_id` bigint(20) DEFAULT NULL,
  `updated_time` datetime NOT NULL,
  PRIMARY KEY (`charge_id`)
) ENGINE=MyISAM AUTO_INCREMENT=212 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `group` */

CREATE TABLE `group` (
  `group_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`group_id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `group_member` */

CREATE TABLE `group_member` (
  `group_id` int(11) NOT NULL,
  `member_id` int(11) NOT NULL,
  PRIMARY KEY (`member_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `keycode` */

CREATE TABLE `keycode` (
  `keycode_id` varchar(32) NOT NULL,
  `member_id` int(11) NOT NULL,
  `updated` datetime /* mariadb-5.3 */ NOT NULL,
  PRIMARY KEY (`keycode_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `member` */

CREATE TABLE `member` (
  `member_id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `type` varchar(255) NOT NULL,
  `apricot_admin` tinyint(1) NOT NULL,
  `joined` datetime /* mariadb-5.3 */ NOT NULL,
  `expires` datetime /* mariadb-5.3 */ NOT NULL,
  `updated` datetime /* mariadb-5.3 */ NOT NULL,
  PRIMARY KEY (`member_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `network_event` */

CREATE TABLE `network_event` (
  `network_event_id` int(11) NOT NULL AUTO_INCREMENT,
  `reader_id` int(11) NOT NULL,
  `online` tinyint(1) NOT NULL,
  `event_time` datetime /* mariadb-5.3 */ NOT NULL,
  PRIMARY KEY (`network_event_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

/*Table structure for table `reader` */

CREATE TABLE `reader` (
  `reader_id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `timeout` int(11) NOT NULL DEFAULT 10,
  `enabled` tinyint(1) NOT NULL DEFAULT 1,
  `group_id` int(11) NOT NULL DEFAULT 0,
  `address` varchar(255) NOT NULL DEFAULT '',
  `version` varchar(255) NOT NULL DEFAULT '',
  `initialized` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `settings` varchar(8192) NOT NULL DEFAULT '',
  `status` varchar(4096) NOT NULL DEFAULT '',
  PRIMARY KEY (`reader_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
