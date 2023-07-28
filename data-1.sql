-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: fplms_management
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `class`
--

DROP TABLE IF EXISTS `class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `enroll_key` varchar(45) DEFAULT NULL,
  `cycle_duration` int DEFAULT NULL,
  `is_disable` tinyint DEFAULT '0',
  `SUBJECT_id` int NOT NULL,
  `LECTURER_id` int NOT NULL,
  `SEMESTER_code` varchar(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_CLASS_SUBJECT1_idx` (`SUBJECT_id`),
  KEY `fk_CLASS_LECTURER1_idx` (`LECTURER_id`),
  KEY `fk_class_SEMESTER1_idx` (`SEMESTER_code`),
  CONSTRAINT `fk_CLASS_LECTURER1` FOREIGN KEY (`LECTURER_id`) REFERENCES `lecturer` (`id`),
  CONSTRAINT `fk_class_SEMESTER1` FOREIGN KEY (`SEMESTER_code`) REFERENCES `semester` (`code`),
  CONSTRAINT `fk_CLASS_SUBJECT1` FOREIGN KEY (`SUBJECT_id`) REFERENCES `subject` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class`
--

LOCK TABLES `class` WRITE;
/*!40000 ALTER TABLE `class` DISABLE KEYS */;
INSERT INTO `class` VALUES (1,'SE1633','abc',2,0,1,2,'SU23'),(2,'SE1626','abc',2,0,1,2,'SU23'),(3,'SE1630','abc',2,0,1,1,'SU23'),(4,'SE1614','abc',2,0,6,1,'SP22'),(5,'SE1524','abc',2,0,5,1,'SP22');
/*!40000 ALTER TABLE `class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cycle_report`
--

DROP TABLE IF EXISTS `cycle_report`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cycle_report` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` text,
  `content` text,
  `cycle_number` int NOT NULL,
  `resource_link` text,
  `feedback` text,
  `mark` float DEFAULT NULL,
  `GROUP_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_PROGRESS_REPORT_copy1_GROUP1_idx` (`GROUP_id`),
  CONSTRAINT `fk_PROGRESS_REPORT_copy1_GROUP1` FOREIGN KEY (`GROUP_id`) REFERENCES `group` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=182 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cycle_report`
--

LOCK TABLES `cycle_report` WRITE;
/*!40000 ALTER TABLE `cycle_report` DISABLE KEYS */;
INSERT INTO `cycle_report` VALUES (1,'Cycle report group 1 cycle 1','This is cycle report content',1,'Resouce link','Good',8,1),(2,'Cycle report group 1 cycle 2','This is cycle report content',2,'Resouce link','Well done',9,1),(3,'Cycle report group 1 cycle 3','This is cycle report content',3,'Resouce link','Good',8,1),(4,'Cycle report group 1 cycle 4','This is cycle report content',4,'Resouce link','Incomplete feature ',7,1),(5,'Cycle report group 1 cycle 5','This is cycle report content',5,'Resouce link','Miss deadline',7,1),(6,'Cycle report group 1 cycle 6','This is cycle report content',6,'Resouce link','Not merge code in github ',7,1),(7,'Cycle report group 1 cycle 7','This is cycle report content',7,'Resouce link','Can\'t connect FE and BE',7,1),(8,'Cycle report group 1 cycle 8','This is cycle report content',8,'Resouce link','Can\'t connect FE and BE',7,1),(9,'Cycle report group 1 cycle 9','This is cycle report content',9,'Resouce link','Can\'t connect FE and BE',7,1),(10,'Cycle report group 1 cycle 10','This is cycle report content',10,'Resouce link','Well done',10,1),(11,'Cycle report group 2 cycle 1','This is cycle report content',1,'Resouce link','Good',8,2),(12,'Cycle report group 2 cycle 2','This is cycle report content',2,'Resouce link','Not merge code in github ',5,2),(13,'Cycle report group 2 cycle 3','This is cycle report content',3,'Resouce link','Can\'t connect FE and BE',7,2),(14,'Cycle report group 2 cycle 4','This is cycle report content',4,'Resouce link','Good',8,2),(15,'Cycle report group 2 cycle 5','This is cycle report content',5,'Resouce link','Miss deadline',5,2),(16,'Cycle report group 2 cycle 6','This is cycle report content',6,'Resouce link','Not merge code in github ',6,2),(17,'Cycle report group 2 cycle 7','This is cycle report content',7,'Resouce link','Miss deadline',4,2),(18,'Cycle report group 2 cycle 8','This is cycle report content',8,'Resouce link','Miss deadline',5,2),(19,'Cycle report group 2 cycle 9','This is cycle report content',9,'Resouce link','Not merge code in github ',2,2),(20,'Cycle report group 2 cycle 10','This is cycle report content',10,'Resouce link','Good',9,2),(21,'Cycle report group 3 cycle 1','This is cycle report content',1,'Resouce link','Good',9,3),(22,'Cycle report group 3 cycle 2','This is cycle report content',2,'Resouce link','Good',8,3),(23,'Cycle report group 3 cycle 3','This is cycle report content',3,'Resouce link','Miss deadline',5,3),(24,'Cycle report group 3 cycle 4','This is cycle report content',4,'Resouce link','Good',7,3),(25,'Cycle report group 3 cycle 5','This is cycle report content',5,'Resouce link','Miss deadline',5,3),(26,'Cycle report group 3 cycle 6','This is cycle report content',6,'Resouce link','Miss deadline',6,3),(27,'Cycle report group 3 cycle 7','This is cycle report content',7,'Resouce link','Not merge code in github ',5,3),(28,'Cycle report group 3 cycle 8','This is cycle report content',8,'Resouce link','Well done',8,3),(29,'Cycle report group 3 cycle 9','This is cycle report content',9,'Resouce link','Well done',8,3),(30,'Cycle report group 3 cycle 10','This is cycle report content',10,'Resouce link','Well done',8,3),(31,'Cycle report group 4 cycle 1','This is cycle report content',1,'Resouce link','Miss deadline',5,4),(32,'Cycle report group 4 cycle 2','This is cycle report content',2,'Resouce link','Miss deadline',6,4),(33,'Cycle report group 4 cycle 3','This is cycle report content',3,'Resouce link','Miss deadline',5,4),(34,'Cycle report group 4 cycle 4','This is cycle report content',4,'Resouce link','Miss deadline',6,4),(35,'Cycle report group 4 cycle 5','This is cycle report content',5,'Resouce link','Can\'t connect FE and BE, Miss deadline',7,4),(36,'Cycle report group 4 cycle 6','This is cycle report content',6,'Resouce link','Incomplete feature ',7,4),(37,'Cycle report group 4 cycle 7','This is cycle report content',7,'Resouce link','Incomplete feature ',7,4),(38,'Cycle report group 4 cycle 8','This is cycle report content',8,'Resouce link','Good',8,4),(39,'Cycle report group 4 cycle 9','This is cycle report content',9,'Resouce link','Good',8,4),(40,'Cycle report group 4 cycle 10','This is cycle report content',10,'Resouce link','Good',8,4),(41,'Cycle report group 5 cycle 1','This is cycle report content',1,'Resouce link','Good',8,5),(42,'Cycle report group 5 cycle 2','This is cycle report content',2,'Resouce link','Good',8,5),(43,'Cycle report group 5 cycle 3','This is cycle report content',3,'Resouce link','Good',8,5),(44,'Cycle report group 5 cycle 4','This is cycle report content',4,'Resouce link','Good',8,5),(45,'Cycle report group 5 cycle 5','This is cycle report content',5,'Resouce link','Well done',9,5),(46,'Cycle report group 5 cycle 6','This is cycle report content',6,'Resouce link','Well done',9,5),(47,'Cycle report group 5 cycle 7','This is cycle report content',7,'Resouce link','Well done',9,5),(48,'Cycle report group 5 cycle 8','This is cycle report content',8,'Resouce link','Well done',9,5),(49,'Cycle report group 5 cycle 9','This is cycle report content',9,'Resouce link','Well done',9,5),(50,'Cycle report group 5 cycle 10','This is cycle report content',10,'Resouce link','Well done',10,5),(51,'Cycle report group 6 cycle 1','This is cycle report content',1,'Resouce link','Miss deadline',5,6),(52,'Cycle report group 6 cycle 2','This is cycle report content',2,'Resouce link','Miss deadline',5,6),(53,'Cycle report group 6 cycle 3','This is cycle report content',3,'Resouce link','Miss deadline',5,6),(54,'Cycle report group 6 cycle 4','This is cycle report content',4,'Resouce link','Miss deadline',5,6),(55,'Cycle report group 6 cycle 5','This is cycle report content',5,'Resouce link','Incomplete feature ',6,6),(56,'Cycle report group 6 cycle 6','This is cycle report content',6,'Resouce link','Good',8,6),(57,'Cycle report group 6 cycle 7','This is cycle report content',7,'Resouce link','Incomplete feature ',7,6),(58,'Cycle report group 6 cycle 8','This is cycle report content',8,'Resouce link','Good',8,6),(59,'Cycle report group 6 cycle 9','This is cycle report content',9,'Resouce link','Good',8,6),(60,'Cycle report group 6 cycle 10','This is cycle report content',10,'Resouce link','Incomplete feature ',7,6),(61,'Cycle report group 7 cycle 1','This is cycle report content',1,'Resouce link','Good',8,7),(62,'Cycle report group 7 cycle 2','This is cycle report content',2,'Resouce link','Good',8,7),(63,'Cycle report group 7 cycle 3','This is cycle report content',3,'Resouce link','Good',8,7),(64,'Cycle report group 7 cycle 4','This is cycle report content',4,'Resouce link','Incomplete feature ',6,7),(65,'Cycle report group 7 cycle 5','This is cycle report content',5,'Resouce link','Can\'t connect FE and BE, Miss deadline',5,7),(66,'Cycle report group 7 cycle 6','This is cycle report content',6,'Resouce link','Incomplete feature ',7,7),(67,'Cycle report group 7 cycle 7','This is cycle report content',7,'Resouce link','Well done',9,7),(68,'Cycle report group 7 cycle 8','This is cycle report content',8,'Resouce link','Well done',9,7),(69,'Cycle report group 7 cycle 9','This is cycle report content',9,'Resouce link','Well done',9,7),(70,'Cycle report group 7 cycle 10','This is cycle report content',10,'Resouce link','Good',8,7),(71,'Cycle report group 8 cycle 1','This is cycle report content',1,'Resouce link','Good',8,8),(72,'Cycle report group 8 cycle 2','This is cycle report content',2,'Resouce link','Good',8,8),(73,'Cycle report group 8 cycle 3','This is cycle report content',3,'Resouce link','Good',8,8),(74,'Cycle report group 8 cycle 4','This is cycle report content',4,'Resouce link','Good',8,8),(75,'Cycle report group 8 cycle 5','This is cycle report content',5,'Resouce link','Good',8,8),(76,'Cycle report group 8 cycle 6','This is cycle report content',6,'Resouce link','Good',8,8),(77,'Cycle report group 8 cycle 7','This is cycle report content',7,'Resouce link','Good',8,8),(78,'Cycle report group 8 cycle 8','This is cycle report content',8,'Resouce link','Good',8,8),(79,'Cycle report group 8 cycle 9','This is cycle report content',9,'Resouce link','Good',8,8),(80,'Cycle report group 8 cycle 10','This is cycle report content',10,'Resouce link','Good',8,8),(81,'Cycle report group 9 cycle 1','This is cycle report content',1,'Resouce link','Good',7,9),(82,'Cycle report group 9 cycle 2','This is cycle report content',2,'Resouce link','Good',8,9),(83,'Cycle report group 9 cycle 3','This is cycle report content',3,'Resouce link','Good',9,9),(84,'Cycle report group 9 cycle 4','This is cycle report content',4,'Resouce link','Good',8,9),(85,'Cycle report group 9 cycle 5','This is cycle report content',5,'Resouce link','Good',5,9),(86,'Cycle report group 9 cycle 6','This is cycle report content',6,'Resouce link','Good',4,9),(87,'Cycle report group 9 cycle 7','This is cycle report content',7,'Resouce link','Good',7,9),(88,'Cycle report group 9 cycle 8','This is cycle report content',8,'Resouce link','Good',6,9),(89,'Cycle report group 9 cycle 9','This is cycle report content',9,'Resouce link','Good',2,9),(90,'Cycle report group 9 cycle 10','This is cycle report content',10,'Resouce link','Good',8,9),(91,'Cycle report group 8 cycle 1','This is cycle report content',1,'Resouce link','Well done',8,10),(92,'Cycle report group 8 cycle 2','This is cycle report content',2,'Resouce link','Well done',7,10),(93,'Cycle report group 8 cycle 3','This is cycle report content',3,'Resouce link','Well done',9,10),(94,'Cycle report group 8 cycle 4','This is cycle report content',4,'Resouce link','Well done',9,10),(95,'Cycle report group 8 cycle 5','This is cycle report content',5,'Resouce link','Well done',8,10),(96,'Cycle report group 8 cycle 6','This is cycle report content',6,'Resouce link','Well done',9,10),(97,'Cycle report group 8 cycle 7','This is cycle report content',7,'Resouce link','Well done',8,10),(98,'Cycle report group 8 cycle 8','This is cycle report content',8,'Resouce link','Well done',9,10),(99,'Cycle report group 8 cycle 9','This is cycle report content',9,'Resouce link','Well done',8,10),(100,'Cycle report group 8 cycle 10','This is cycle report content',10,'Resouce link','Well done',9,10);
/*!40000 ALTER TABLE `cycle_report` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group`
--

DROP TABLE IF EXISTS `group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `group` (
  `id` int NOT NULL AUTO_INCREMENT,
  `number` int DEFAULT NULL,
  `member_quantity` int DEFAULT NULL,
  `enroll_time` timestamp NULL DEFAULT NULL,
  `PROJECT_id` int DEFAULT NULL,
  `CLASS_id` int NOT NULL,
  `is_disable` tinyint DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `fk_GROUP_PROJECT1_idx` (`PROJECT_id`),
  KEY `fk_GROUP_CLASS1_idx` (`CLASS_id`),
  CONSTRAINT `fk_GROUP_CLASS1` FOREIGN KEY (`CLASS_id`) REFERENCES `class` (`id`),
  CONSTRAINT `fk_GROUP_PROJECT1` FOREIGN KEY (`PROJECT_id`) REFERENCES `project` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=114 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group`
--

LOCK TABLES `group` WRITE;
/*!40000 ALTER TABLE `group` DISABLE KEYS */;
INSERT INTO `group` VALUES (1,1,5,'2023-05-01 17:00:00',1,1,0),(2,2,5,'2023-05-01 17:00:00',2,1,0),(3,3,5,'2023-05-01 17:00:00',3,1,0),(4,4,5,'2023-05-01 17:00:00',1,1,0),(5,5,5,'2023-05-01 17:00:00',2,1,0),(6,6,5,'2023-05-01 17:00:00',3,1,0),(7,7,5,'2023-05-01 17:00:00',2,1,0),(8,1,5,'2023-01-02 17:00:00',4,4,0),(9,2,5,'2023-01-02 17:00:00',4,4,0),(10,3,5,'2023-01-02 17:00:00',4,4,0);
/*!40000 ALTER TABLE `group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lecturer`
--

DROP TABLE IF EXISTS `lecturer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lecturer` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `image_url` text,
  `is_disable` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lecturer`
--

LOCK TABLES `lecturer` WRITE;
/*!40000 ALTER TABLE `lecturer` DISABLE KEYS */;
INSERT INTO `lecturer` VALUES (1,'kienfplms.fe@gmail.com','Nguyễn Thành Kiên',NULL,0),(2,'huongntc2.fe@gmail','Nguyễn Thị Cẩm Hương',NULL,0);
/*!40000 ALTER TABLE `lecturer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meeting`
--

DROP TABLE IF EXISTS `meeting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meeting` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) DEFAULT NULL,
  `link` text,
  `feedback` text,
  `schedule_time` timestamp NULL DEFAULT NULL,
  `LECTURER_id` int NOT NULL,
  `GROUP_id` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `fk_MEETING_LECTURER1_idx` (`LECTURER_id`),
  KEY `fk_MEETING_GROUP1_idx` (`GROUP_id`),
  CONSTRAINT `fk_MEETING_GROUP1` FOREIGN KEY (`GROUP_id`) REFERENCES `group` (`id`),
  CONSTRAINT `fk_MEETING_LECTURER1` FOREIGN KEY (`LECTURER_id`) REFERENCES `lecturer` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meeting`
--

LOCK TABLES `meeting` WRITE;
/*!40000 ALTER TABLE `meeting` DISABLE KEYS */;
INSERT INTO `meeting` VALUES (1,'Meeting','link',NULL,'2023-05-31 17:00:00',2,1),(2,'Meeting','link',NULL,'2023-06-09 17:00:00',2,3),(3,'Meeting','link',NULL,'2023-06-14 17:00:00',2,5),(4,'Meeting','link',NULL,'2023-06-11 17:00:00',2,6),(5,'Meeting','link',NULL,'2023-06-08 17:00:00',2,7);
/*!40000 ALTER TABLE `meeting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `progress_report`
--

DROP TABLE IF EXISTS `progress_report`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `progress_report` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` text,
  `content` text,
  `report_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `STUDENT_id` int NOT NULL,
  `GROUP_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_PROGRESS_REPORT_STUDENT_idx` (`STUDENT_id`),
  KEY `fk_PROGRESS_REPORT_GROUP1_idx` (`GROUP_id`),
  CONSTRAINT `fk_PROGRESS_REPORT_GROUP1` FOREIGN KEY (`GROUP_id`) REFERENCES `group` (`id`),
  CONSTRAINT `fk_PROGRESS_REPORT_STUDENT` FOREIGN KEY (`STUDENT_id`) REFERENCES `student` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=124 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `progress_report`
--

LOCK TABLES `progress_report` WRITE;
/*!40000 ALTER TABLE `progress_report` DISABLE KEYS */;
INSERT INTO `progress_report` VALUES (1,'Daily report','This report belong to group 1','2023-07-20 09:21:05',1,1),(2,'Daily report','This report belong to group 1','2023-07-20 09:21:05',2,1),(3,'Daily report','This report belong to group 1','2023-07-20 09:21:05',3,1),(4,'Daily report','This report belong to group 1','2023-07-20 09:21:05',4,1),(5,'Daily report','This report belong to group 1','2023-07-20 09:21:05',5,1),(6,'Daily report','This report belong to group 1','2023-07-21 09:21:05',1,1),(7,'Daily report','This report belong to group 1','2023-07-21 09:21:05',2,1),(8,'Daily report','This report belong to group 1','2023-07-21 09:21:05',3,1),(9,'Daily report','This report belong to group 1','2023-07-21 09:21:05',4,1),(10,'Daily report','This report belong to group 1','2023-07-21 09:21:05',5,1),(11,'Daily report','This report belong to group 1','2023-07-22 09:21:05',1,1),(12,'Daily report','This report belong to group 1','2023-07-22 09:21:05',2,1),(13,'Daily report','This report belong to group 1','2023-07-22 09:21:05',3,1),(14,'Daily report','This report belong to group 1','2023-07-24 09:21:05',1,1),(15,'Daily report','This report belong to group 1','2023-07-24 09:21:05',4,1),(16,'Daily report','This report belong to group 1','2023-07-24 09:21:05',5,1),(17,'Daily report','This report belong to group 1','2023-07-25 09:21:05',3,1),(18,'Daily report','This report belong to group 1','2023-07-25 09:21:05',4,1),(19,'Daily report','This report belong to group 1','2023-07-26 09:21:05',2,1),(20,'Daily report','This report belong to group 1','2023-07-27 09:21:05',1,1),(21,'Daily report','This report belong to group 1','2023-07-27 09:21:05',4,1),(22,'Daily report','This report belong to group 1','2023-07-27 09:21:05',5,1),(23,'Daily report','This report belong to group 8','2023-02-20 09:21:05',1,8),(24,'Daily report','This report belong to group 8','2023-02-20 09:21:05',2,8),(25,'Daily report','This report belong to group 8','2023-02-20 09:21:05',3,8),(26,'Daily report','This report belong to group 8','2023-02-20 09:21:05',4,8),(27,'Daily report','This report belong to group 8','2023-02-20 09:21:05',5,8),(28,'Daily report','This report belong to group 8','2023-02-21 09:21:05',1,8),(29,'Daily report','This report belong to group 8','2023-02-21 09:21:05',2,8),(30,'Daily report','This report belong to group 8','2023-02-21 09:21:05',3,8),(31,'Daily report','This report belong to group 8','2023-02-21 09:21:05',4,8),(32,'Daily report','This report belong to group 8','2023-02-21 09:21:05',5,8),(33,'Daily report','This report belong to group 8','2023-02-22 09:21:05',1,8),(34,'Daily report','This report belong to group 8','2023-02-22 09:21:05',2,8),(35,'Daily report','This report belong to group 8','2023-02-22 09:21:05',3,8),(36,'Daily report','This report belong to group 8','2023-02-24 09:21:05',1,8),(37,'Daily report','This report belong to group 8','2023-02-24 09:21:05',4,8),(38,'Daily report','This report belong to group 8','2023-02-24 09:21:05',5,8),(39,'Daily report','This report belong to group 8','2023-02-25 09:21:05',3,8),(40,'Daily report','This report belong to group 8','2023-02-25 09:21:05',4,8),(41,'Daily report','This report belong to group 8','2023-02-26 09:21:05',2,8),(42,'Daily report','This report belong to group 8','2023-02-27 09:21:05',1,8),(43,'Daily report','This report belong to group 8','2023-02-27 09:21:05',4,8),(44,'Daily report','This report belong to group 8','2023-02-27 09:21:05',5,8);
/*!40000 ALTER TABLE `progress_report` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project` (
  `id` int NOT NULL AUTO_INCREMENT,
  `theme` text,
  `name` text,
  `problem` text,
  `context` text,
  `actors` text,
  `requirements` text,
  `SUBJECT_id` int DEFAULT NULL,
  `is_disable` tinyint NOT NULL DEFAULT '0',
  `LECTURER_id` int NOT NULL,
  `SEMESTER_code` varchar(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_PROJECT_SUBJECT1_idx` (`SUBJECT_id`),
  KEY `fk_project_lecturer1_idx` (`LECTURER_id`),
  KEY `fk_project_semester1_idx` (`SEMESTER_code`),
  CONSTRAINT `fk_project_lecturer1` FOREIGN KEY (`LECTURER_id`) REFERENCES `lecturer` (`id`),
  CONSTRAINT `fk_project_semester1` FOREIGN KEY (`SEMESTER_code`) REFERENCES `semester` (`code`),
  CONSTRAINT `fk_PROJECT_SUBJECT1` FOREIGN KEY (`SUBJECT_id`) REFERENCES `subject` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project`
--

LOCK TABLES `project` WRITE;
/*!40000 ALTER TABLE `project` DISABLE KEYS */;
INSERT INTO `project` VALUES (1,NULL,NULL,NULL,NULL,NULL,NULL,1,0,2,'SU23'),(2,NULL,NULL,NULL,NULL,NULL,NULL,1,0,2,'SU23'),(3,NULL,NULL,NULL,NULL,NULL,NULL,1,0,2,'SU23'),(4,NULL,NULL,NULL,NULL,NULL,NULL,6,0,1,'SP22');
/*!40000 ALTER TABLE `project` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `semester`
--

DROP TABLE IF EXISTS `semester`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `semester` (
  `code` varchar(10) NOT NULL,
  `start_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `semester`
--

LOCK TABLES `semester` WRITE;
/*!40000 ALTER TABLE `semester` DISABLE KEYS */;
INSERT INTO `semester` VALUES ('FA22','2022-09-01','2022-12-31'),('SP22','2022-01-01','2022-04-30'),('SU23','2023-05-01','2023-08-31'),('SU22','2022-05-01','2022-08-31');
/*!40000 ALTER TABLE `semester` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(10) NOT NULL,
  `email` varchar(45) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `image_url` text,
  `is_disable` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=138 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (1,'SE140352','hatthse140352@fpt.edu.vn','Trần Thị Hải Hà',NULL,0),(2,'SE140399','tanhnse140399@fpt.edu.vn','Huỳnh Nhật Tân',NULL,0),(3,'SE140534','thangpdse140534@fpt.edu.vn','Phạm Đức Thắng',NULL,0),(4,'SE141009','khoipqase141009@fpt.edu.vn','Phạm Quốc Anh Khôi',NULL,0),(5,'SE150116','tinntse150116@fpt.edu.vn','Nguyễn Trung Tín',NULL,0),(6,'SE150159','duyvttse150159@fpt.edu.vn','Võ Thị Tường Duy',NULL,0),(7,'SE150649','baovse150649@fpt.edu.vn','Vũ Bảo Bảo',NULL,0),(8,'SE150707','ductmse150707@fpt.edu.vn','Trần Minh Đức',NULL,0),(9,'SE150754','hanhchse150754@fpt.edu.vn','Cao Hồng Hạnh',NULL,0),(10,'SE150882','longthse150882@fpt.edu.vn','Trương Hoàng Long',NULL,0),(11,'SE150978','phupxse150978@fpt.edu.vn','Phạm Xuân Phú',NULL,0),(12,'SE151131','trungttse151131@fpt.edu.vn','Trương Thành Trung',NULL,0),(13,'SE151136','trungntse151136@fpt.edu.vn','Nguyễn Tấn Trung',NULL,0),(14,'SE151388','tanndse151388@fpt.edu.vn','Nguyễn Duy Tân',NULL,0),(15,'SE151459','hieuphtse151459@fpt.edu.vn','Phan Hoàng Trung Hiếu',NULL,0),(16,'SE151471','tuanhmse151471@fpt.edu.vn','Hoàng Minh Tuấn',NULL,0),(17,'SE160026','kienntse160026@fpt.edu.vn','Nguyễn Thành Kiên',NULL,0),(18,'SE160034','hoangtnse160034@fpt.edu.vn','Trần Nhật Hoàng',NULL,0),(19,'SE160037','thanhptse160037@fpt.edu.vn','Phạm Trọng Thành',NULL,0),(20,'SE160076','niqhtse160076@fpt.edu.vn','Quách Hêng TôNi',NULL,0),(21,'SE160098','hantse160098@fpt.edu.vn','Nguyễn Thanh Hà',NULL,0),(22,'SE160108','khanglnse160108@fpt.edu.vn','Lê Nguyên Khang',NULL,0),(23,'SE160120','nhanhtse160120@fpt.edu.vn','Hồ Trọng Nhân',NULL,0),(24,'SE160145','duclmse160145@fpt.edu.vn','Lê Minh Đức',NULL,0),(25,'SE160146','tungvtse160146@fpt.edu.vn','Vũ Thanh Tùng',NULL,0),(26,'SE160159','phuchbse160159@fpt.edu.vn','Huỳnh Bảo Phúc',NULL,0),(27,'SE160166','thientqse160166@fpt.edu.vn','Trần Quang Thiện',NULL,0),(28,'SE160169','nganthkse160169@fpt.edu.vn','Trần Hoàng Kim Ngân',NULL,0),(29,'SE160182','quangttse160182@fpt.edu.vn','Trần Thanh Quang',NULL,0),(30,'SE161002','anhtvkse161002@fpt.edu.vn','Trần Vũ Kim Anh',NULL,0),(31,'SE161187','phuongmtse161187@fpt.edu.vn','Mai Thanh Phương',NULL,0),(32,'SE63556','hoangndhse63556@fpt.edu.vn','Nguyễn Đỗ Huy Hoàng',NULL,0);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student_class`
--

DROP TABLE IF EXISTS `student_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_class` (
  `STUDENT_id` int NOT NULL,
  `CLASS_id` int NOT NULL,
  PRIMARY KEY (`STUDENT_id`,`CLASS_id`),
  KEY `fk_STUDENT_has_CLASS_CLASS1_idx` (`CLASS_id`),
  KEY `fk_STUDENT_has_CLASS_STUDENT1_idx` (`STUDENT_id`),
  CONSTRAINT `fk_STUDENT_has_CLASS_CLASS1` FOREIGN KEY (`CLASS_id`) REFERENCES `class` (`id`),
  CONSTRAINT `fk_STUDENT_has_CLASS_STUDENT1` FOREIGN KEY (`STUDENT_id`) REFERENCES `student` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student_class`
--

LOCK TABLES `student_class` WRITE;
/*!40000 ALTER TABLE `student_class` DISABLE KEYS */;
INSERT INTO `student_class` VALUES (1,1),(2,1),(3,1),(4,1),(5,1),(6,1),(7,1),(8,1),(9,1),(10,1),(11,1),(12,1),(13,1),(14,1),(15,1),(16,1),(17,1),(18,1),(19,1),(20,1),(21,1),(22,1),(23,1),(24,1),(25,1),(26,1),(27,1),(28,1),(29,1),(30,1),(31,1),(32,1),(1,4),(2,4),(3,4),(4,4),(5,4),(6,4),(7,4),(8,4),(9,4),(10,4),(11,4),(12,4),(13,4),(14,4),(15,4);
/*!40000 ALTER TABLE `student_class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student_group`
--

DROP TABLE IF EXISTS `student_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_group` (
  `id` int NOT NULL AUTO_INCREMENT,
  `STUDENT_id` int NOT NULL,
  `GROUP_id` int NOT NULL,
  `CLASS_id` int NOT NULL,
  `is_leader` tinyint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_STUDENT_has_GROUP_GROUP1_idx` (`GROUP_id`),
  KEY `fk_STUDENT_has_GROUP_STUDENT1_idx` (`STUDENT_id`),
  KEY `fk_STUDENT_GROUP_CLASS1_idx` (`CLASS_id`),
  CONSTRAINT `fk_STUDENT_GROUP_CLASS1` FOREIGN KEY (`CLASS_id`) REFERENCES `class` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_GROUP1` FOREIGN KEY (`GROUP_id`) REFERENCES `group` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_STUDENT1` FOREIGN KEY (`STUDENT_id`) REFERENCES `student` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=151 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student_group`
--

LOCK TABLES `student_group` WRITE;
/*!40000 ALTER TABLE `student_group` DISABLE KEYS */;
INSERT INTO `student_group` VALUES (1,1,1,1,1),(2,2,1,1,0),(3,3,1,1,0),(4,4,1,1,0),(5,5,1,1,0),(6,6,2,1,0),(7,7,2,1,1),(8,8,2,1,0),(9,9,2,1,0),(10,10,2,1,0),(11,11,3,1,0),(12,12,3,1,0),(13,13,3,1,1),(14,14,3,1,0),(15,15,3,1,0),(16,16,4,1,1),(17,17,4,1,0),(18,18,4,1,0),(19,19,4,1,0),(20,20,4,1,0),(21,21,5,1,1),(22,22,5,1,0),(23,23,5,1,0),(24,24,5,1,0),(25,25,5,1,0),(26,26,6,1,0),(27,27,6,1,1),(28,28,6,1,0),(29,29,6,1,0),(30,30,7,1,1),(31,31,7,1,0),(32,32,7,1,0),(33,1,8,4,0),(34,2,8,4,0),(35,3,8,4,1),(36,4,8,4,0),(37,5,8,4,0),(38,6,9,4,0),(39,7,9,4,0),(40,8,9,4,0),(41,9,9,4,0),(42,10,9,4,1),(43,11,10,4,0),(44,12,10,4,0),(45,13,10,4,1),(46,14,10,4,0),(47,15,10,4,0);
/*!40000 ALTER TABLE `student_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subject`
--

DROP TABLE IF EXISTS `subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subject` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `is_disable` tinyint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subject`
--

LOCK TABLES `subject` WRITE;
/*!40000 ALTER TABLE `subject` DISABLE KEYS */;
INSERT INTO `subject` VALUES (1,'SWP391',0),(2,'SWR302',0),(3,'SWT301',0),(4,'ITE301c',0),(5,'PRJ301',0),(6,'JPD123',0);
/*!40000 ALTER TABLE `subject` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-08-02 18:51:02
