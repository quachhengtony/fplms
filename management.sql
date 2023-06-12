-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

CREATE SCHEMA IF NOT EXISTS `fplms_management` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `fplms_management` ;

-- -----------------------------------------------------
-- Table `fplms_management`.`lecturer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`lecturer` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `image_url` TEXT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`subject`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`subject` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`class`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`class` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `semester` VARCHAR(45) NULL DEFAULT NULL,
  `enroll_key` VARCHAR(45) NULL DEFAULT NULL,
  `is_disable` TINYINT NULL DEFAULT '0',
  `SUBJECT_id` INT NOT NULL,
  `LECTURER_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_CLASS_SUBJECT1_idx` (`SUBJECT_id` ASC) VISIBLE,
  INDEX `fk_CLASS_LECTURER1_idx` (`LECTURER_id` ASC) VISIBLE,
  CONSTRAINT `fk_CLASS_LECTURER1`
    FOREIGN KEY (`LECTURER_id`)
    REFERENCES `fplms_management`.`lecturer` (`id`),
  CONSTRAINT `fk_CLASS_SUBJECT1`
    FOREIGN KEY (`SUBJECT_id`)
    REFERENCES `fplms_management`.`subject` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 112
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`project`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`project` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `theme` TEXT NULL DEFAULT NULL,
  `name` TEXT NULL DEFAULT NULL,
  `problem` TEXT NULL DEFAULT NULL,
  `context` TEXT NULL DEFAULT NULL,
  `actors` TEXT NULL DEFAULT NULL,
  `requirements` TEXT NULL DEFAULT NULL,
  `SUBJECT_id` INT NOT NULL,
  `LECTURER_id` INT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`, `SUBJECT_id`),
  INDEX `fk_PROJECT_SUBJECT1_idx` (`SUBJECT_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROJECT_SUBJECT1`
    FOREIGN KEY (`SUBJECT_id`)
    REFERENCES `fplms_management`.`subject` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 96
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`group` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `number` INT NULL DEFAULT NULL,
  `member_quantity` INT NULL DEFAULT NULL,
  `enroll_time` TIMESTAMP NULL DEFAULT NULL,
  `PROJECT_id` INT NULL DEFAULT NULL,
  `CLASS_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_GROUP_PROJECT1_idx` (`PROJECT_id` ASC) VISIBLE,
  INDEX `fk_GROUP_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  CONSTRAINT `fk_GROUP_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms_management`.`class` (`id`),
  CONSTRAINT `fk_GROUP_PROJECT1`
    FOREIGN KEY (`PROJECT_id`)
    REFERENCES `fplms_management`.`project` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`cycle_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`cycle_report` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` TEXT NULL DEFAULT NULL,
  `content` TEXT NULL DEFAULT NULL,
  `report_time` TIMESTAMP NULL DEFAULT NULL,
  `feedback` TEXT NULL DEFAULT NULL,
  `resource_link` TEXT NULL DEFAULT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_PROGRESS_REPORT_copy1_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROGRESS_REPORT_copy1_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms_management`.`group` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`meeting`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`meeting` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(255) NULL DEFAULT NULL,
  `link` TEXT NULL DEFAULT NULL,
  `feedback` TEXT NULL DEFAULT NULL,
  `schedule_time` TIMESTAMP NULL DEFAULT NULL,
  `LECTURER_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_MEETING_LECTURER1_idx` (`LECTURER_id` ASC) VISIBLE,
  INDEX `fk_MEETING_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  CONSTRAINT `fk_MEETING_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms_management`.`group` (`id`),
  CONSTRAINT `fk_MEETING_LECTURER1`
    FOREIGN KEY (`LECTURER_id`)
    REFERENCES `fplms_management`.`lecturer` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`student`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`student` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(10) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `image_url` TEXT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`progress_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`progress_report` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` TEXT NULL DEFAULT NULL,
  `content` TEXT NULL DEFAULT NULL,
  `report_time` TIMESTAMP NULL DEFAULT NULL,
  `STUDENT_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_PROGRESS_REPORT_STUDENT_idx` (`STUDENT_id` ASC) VISIBLE,
  INDEX `fk_PROGRESS_REPORT_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROGRESS_REPORT_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms_management`.`group` (`id`),
  CONSTRAINT `fk_PROGRESS_REPORT_STUDENT`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms_management`.`student` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 101
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`student_class`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`student_class` (
  `STUDENT_id` INT NOT NULL,
  `CLASS_id` INT NOT NULL,
  PRIMARY KEY (`STUDENT_id`, `CLASS_id`),
  INDEX `fk_STUDENT_has_CLASS_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_has_CLASS_STUDENT1_idx` (`STUDENT_id` ASC) VISIBLE,
  CONSTRAINT `fk_STUDENT_has_CLASS_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms_management`.`class` (`id`),
  CONSTRAINT `fk_STUDENT_has_CLASS_STUDENT1`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms_management`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms_management`.`student_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms_management`.`student_group` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `STUDENT_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  `CLASS_id` INT NOT NULL,
  `is_leader` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_STUDENT_has_GROUP_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_has_GROUP_STUDENT1_idx` (`STUDENT_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_GROUP_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  CONSTRAINT `fk_STUDENT_GROUP_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms_management`.`class` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms_management`.`group` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_STUDENT1`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms_management`.`student` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 102
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
