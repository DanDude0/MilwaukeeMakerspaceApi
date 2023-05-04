/*
SQLyog Community
MySQL - 10.5.18-MariaDB-0+deb11u1-log : Database - billing
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`billing` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;

/*Table structure for table `event` */

CREATE TABLE `event` (
  `event_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `type` varchar(255) NOT NULL,
  `parameters` varchar(8192) NOT NULL,
  `wa_key` int(11) NOT NULL,
  PRIMARY KEY (`event_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `invoice` */

CREATE TABLE `invoice` (
  `invoice_id` bigint(20) NOT NULL,
  `document_number` varchar(255) NOT NULL,
  `invoice_date` datetime NOT NULL,
  `amount` decimal(8,2) NOT NULL,
  `paid_amount` decimal(8,2) NOT NULL,
  `is_paid` tinyint(1) NOT NULL,
  `type` varchar(255) NOT NULL,
  `private_notes` varchar(4096) NOT NULL,
  `public_notes` varchar(4096) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime NOT NULL,
  `voided_date` datetime NOT NULL,
  `contact_id` bigint(20) NOT NULL,
  `contact_name` varchar(255) NOT NULL,
  `creator_id` bigint(20) NOT NULL,
  `updater_id` bigint(20) NOT NULL,
  PRIMARY KEY (`invoice_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `invoice_line` */

CREATE TABLE `invoice_line` (
  `invoice_line_id` bigint(20) NOT NULL,
  `invoice_id` bigint(20) NOT NULL,
  `amount` decimal(8,2) NOT NULL,
  `type` varchar(255) NOT NULL,
  `notes` varchar(4096) NOT NULL,
  PRIMARY KEY (`invoice_line_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `makers_village_invoice` */

CREATE TABLE `makers_village_invoice` (
  `makers_village_invoice_id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `year` int(11) NOT NULL,
  `month` int(11) NOT NULL,
  `total_mms_billed` decimal(8,2) NOT NULL,
  `total_mms_prepaid` decimal(8,2) NOT NULL,
  `total_mms_paid` decimal(8,2) NOT NULL,
  `total_mms_outstanding` decimal(8,2) NOT NULL,
  `total_mv_owed` decimal(8,2) NOT NULL,
  `total_mv_prepaid` decimal(8,2) NOT NULL,
  `total_mv_adjustments` decimal(8,2) NOT NULL,
  `total_mv_paid` decimal(8,2) NOT NULL,
  `total_mv_outstanding` decimal(8,2) NOT NULL,
  `created_date` datetime NOT NULL,
  `details` text NOT NULL,
  PRIMARY KEY (`makers_village_invoice_id`)
) ENGINE=InnoDB AUTO_INCREMENT=196 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `makers_village_invoice_adjustments` */

CREATE TABLE `makers_village_invoice_adjustments` (
  `makers_village_invoice_adjustments_id` int(11) NOT NULL AUTO_INCREMENT,
  `date` datetime NOT NULL,
  `reason` varchar(4096) DEFAULT NULL,
  `amount` decimal(8,2) NOT NULL,
  PRIMARY KEY (`makers_village_invoice_adjustments_id`)
) ENGINE=InnoDB AUTO_INCREMENT=148 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `payment_allocation` */

CREATE TABLE `payment_allocation` (
  `payment_allocation_id` bigint(20) NOT NULL,
  `amount` decimal(8,2) NOT NULL,
  `invoice_id` bigint(20) NOT NULL,
  `payment_id` bigint(20) NOT NULL,
  `payment_date` datetime NOT NULL,
  PRIMARY KEY (`payment_allocation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Table structure for table `storage_notes` */

CREATE TABLE `storage_notes` (
  `storage_notes_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `invoice_id` bigint(20) NOT NULL,
  `contact_id` bigint(20) NOT NULL,
  `snapshot_date` datetime NOT NULL,
  `notes` varchar(4096) NOT NULL,
  PRIMARY KEY (`storage_notes_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9319 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
