-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema fplms
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema fplms
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `fplms` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `fplms` ;

-- -----------------------------------------------------
-- Table `fplms`.`__EFMigrationsHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`__EFMigrationsHistory` (
  `MigrationId` VARCHAR(150) NOT NULL,
  `ProductVersion` VARCHAR(32) NOT NULL,
  PRIMARY KEY (`MigrationId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`student`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`student` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(10) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `image_url` TEXT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL,
  `Point` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE1` (`email` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`subject`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`subject` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `is_disable` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`question`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`question` (
  `Id` CHAR(36) NOT NULL,
  `Title` VARCHAR(250) NOT NULL,
  `Content` LONGTEXT NOT NULL,
  `Solved` TINYINT(1) NOT NULL,
  `CreatedDate` DATETIME(6) NOT NULL,
  `ModifiedDate` DATETIME(6) NULL DEFAULT NULL,
  `Removed` TINYINT(1) NOT NULL,
  `RemovedBy` LONGTEXT NULL DEFAULT NULL,
  `SubjectId` INT NOT NULL,
  `StudentId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_question_StudentId` (`StudentId` ASC) VISIBLE,
  INDEX `IX_question_SubjectId` (`SubjectId` ASC) VISIBLE,
  CONSTRAINT `FK_question_student_StudentId`
    FOREIGN KEY (`StudentId`)
    REFERENCES `fplms`.`student` (`id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_question_subject_SubjectId`
    FOREIGN KEY (`SubjectId`)
    REFERENCES `fplms`.`subject` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`answer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`answer` (
  `Id` CHAR(36) NOT NULL,
  `Content` VARCHAR(1000) NOT NULL,
  `CreatedDate` DATETIME(6) NOT NULL,
  `ModifiedDate` DATETIME(6) NULL DEFAULT NULL,
  `Accepted` TINYINT(1) NOT NULL,
  `Removed` TINYINT(1) NOT NULL,
  `RemovedBy` LONGTEXT NULL DEFAULT NULL,
  `StudentId` INT NOT NULL,
  `QuestionId` CHAR(36) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_answer_QuestionId` (`QuestionId` ASC) VISIBLE,
  INDEX `IX_answer_StudentId` (`StudentId` ASC) VISIBLE,
  CONSTRAINT `FK_answer_question_QuestionId`
    FOREIGN KEY (`QuestionId`)
    REFERENCES `fplms`.`question` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_answer_student_StudentId`
    FOREIGN KEY (`StudentId`)
    REFERENCES `fplms`.`student` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`lecturer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`lecturer` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `image_url` TEXT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`semester`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`semester` (
  `code` VARCHAR(10) NOT NULL,
  `start_date` DATE NULL DEFAULT NULL,
  `end_date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`code`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`class`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`class` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `enroll_key` VARCHAR(45) NULL DEFAULT NULL,
  `cycle_duration` INT NULL DEFAULT NULL,
  `is_disable` TINYINT NULL DEFAULT '0',
  `SUBJECT_id` INT NOT NULL,
  `LECTURER_id` INT NOT NULL,
  `SEMESTER_code` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_CLASS_LECTURER1_idx` (`LECTURER_id` ASC) VISIBLE,
  INDEX `fk_class_SEMESTER1_idx` (`SEMESTER_code` ASC) VISIBLE,
  INDEX `fk_CLASS_SUBJECT1_idx` (`SUBJECT_id` ASC) VISIBLE,
  CONSTRAINT `fk_CLASS_LECTURER1`
    FOREIGN KEY (`LECTURER_id`)
    REFERENCES `fplms`.`lecturer` (`id`),
  CONSTRAINT `fk_class_SEMESTER1`
    FOREIGN KEY (`SEMESTER_code`)
    REFERENCES `fplms`.`semester` (`code`),
  CONSTRAINT `fk_CLASS_SUBJECT1`
    FOREIGN KEY (`SUBJECT_id`)
    REFERENCES `fplms`.`subject` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`project`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`project` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `theme` TEXT NULL DEFAULT NULL,
  `name` TEXT NULL DEFAULT NULL,
  `problem` TEXT NULL DEFAULT NULL,
  `context` TEXT NULL DEFAULT NULL,
  `actors` TEXT NULL DEFAULT NULL,
  `requirements` TEXT NULL DEFAULT NULL,
  `SUBJECT_id` INT NULL DEFAULT NULL,
  `is_disable` TINYINT NOT NULL,
  `LECTURER_id` INT NOT NULL,
  `SEMESTER_code` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_project_lecturer1_idx` (`LECTURER_id` ASC) VISIBLE,
  INDEX `fk_project_semester1_idx` (`SEMESTER_code` ASC) VISIBLE,
  INDEX `fk_PROJECT_SUBJECT1_idx` (`SUBJECT_id` ASC) VISIBLE,
  CONSTRAINT `fk_project_lecturer1`
    FOREIGN KEY (`LECTURER_id`)
    REFERENCES `fplms`.`lecturer` (`id`),
  CONSTRAINT `fk_project_semester1`
    FOREIGN KEY (`SEMESTER_code`)
    REFERENCES `fplms`.`semester` (`code`),
  CONSTRAINT `fk_PROJECT_SUBJECT1`
    FOREIGN KEY (`SUBJECT_id`)
    REFERENCES `fplms`.`subject` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`group` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `number` INT NULL DEFAULT NULL,
  `member_quantity` INT NULL DEFAULT NULL,
  `enroll_time` TIMESTAMP NULL DEFAULT NULL,
  `PROJECT_id` INT NULL DEFAULT NULL,
  `CLASS_id` INT NOT NULL,
  `is_disable` TINYINT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  INDEX `fk_GROUP_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  INDEX `fk_GROUP_PROJECT1_idx` (`PROJECT_id` ASC) VISIBLE,
  CONSTRAINT `fk_GROUP_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms`.`class` (`id`),
  CONSTRAINT `fk_GROUP_PROJECT1`
    FOREIGN KEY (`PROJECT_id`)
    REFERENCES `fplms`.`project` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`cycle_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`cycle_report` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` TEXT NULL DEFAULT NULL,
  `content` TEXT NULL DEFAULT NULL,
  `cycle_number` INT NOT NULL,
  `resource_link` TEXT NULL DEFAULT NULL,
  `feedback` TEXT NULL DEFAULT NULL,
  `mark` FLOAT NULL DEFAULT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_PROGRESS_REPORT_copy1_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROGRESS_REPORT_copy1_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms`.`group` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`meeting`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`meeting` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(255) NULL DEFAULT NULL,
  `link` TEXT NULL DEFAULT NULL,
  `feedback` TEXT NULL DEFAULT NULL,
  `schedule_time` TIMESTAMP NULL DEFAULT NULL,
  `LECTURER_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `fk_MEETING_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  INDEX `fk_MEETING_LECTURER1_idx` (`LECTURER_id` ASC) VISIBLE,
  CONSTRAINT `fk_MEETING_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms`.`group` (`id`),
  CONSTRAINT `fk_MEETING_LECTURER1`
    FOREIGN KEY (`LECTURER_id`)
    REFERENCES `fplms`.`lecturer` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`progress_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`progress_report` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` TEXT NULL DEFAULT NULL,
  `content` TEXT NULL DEFAULT NULL,
  `report_time` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `STUDENT_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_PROGRESS_REPORT_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  INDEX `fk_PROGRESS_REPORT_STUDENT_idx` (`STUDENT_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROGRESS_REPORT_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms`.`group` (`id`),
  CONSTRAINT `fk_PROGRESS_REPORT_STUDENT`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`student_answer_upvote`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`student_answer_upvote` (
  `StudentId` INT NOT NULL,
  `AnswerId` CHAR(36) NOT NULL,
  PRIMARY KEY (`StudentId`, `AnswerId`),
  INDEX `IX_student_answer_upvote_AnswerId` (`AnswerId` ASC) VISIBLE,
  CONSTRAINT `FK_student_answer_upvote_answer_AnswerId`
    FOREIGN KEY (`AnswerId`)
    REFERENCES `fplms`.`answer` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_student_answer_upvote_student_StudentId`
    FOREIGN KEY (`StudentId`)
    REFERENCES `fplms`.`student` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`student_class`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`student_class` (
  `STUDENT_id` INT NOT NULL,
  `CLASS_id` INT NOT NULL,
  PRIMARY KEY (`STUDENT_id`, `CLASS_id`),
  INDEX `fk_STUDENT_has_CLASS_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_has_CLASS_STUDENT1_idx` (`STUDENT_id` ASC) VISIBLE,
  CONSTRAINT `fk_STUDENT_has_CLASS_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms`.`class` (`id`),
  CONSTRAINT `fk_STUDENT_has_CLASS_STUDENT1`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`student_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`student_group` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `STUDENT_id` INT NOT NULL,
  `GROUP_id` INT NOT NULL,
  `CLASS_id` INT NOT NULL,
  `is_leader` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_STUDENT_GROUP_CLASS1_idx` (`CLASS_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_has_GROUP_GROUP1_idx` (`GROUP_id` ASC) VISIBLE,
  INDEX `fk_STUDENT_has_GROUP_STUDENT1_idx` (`STUDENT_id` ASC) VISIBLE,
  CONSTRAINT `fk_STUDENT_GROUP_CLASS1`
    FOREIGN KEY (`CLASS_id`)
    REFERENCES `fplms`.`class` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_GROUP1`
    FOREIGN KEY (`GROUP_id`)
    REFERENCES `fplms`.`group` (`id`),
  CONSTRAINT `fk_STUDENT_has_GROUP_STUDENT1`
    FOREIGN KEY (`STUDENT_id`)
    REFERENCES `fplms`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `fplms`.`student_upvote`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `fplms`.`student_upvote` (
  `StudentId` INT NOT NULL,
  `QuestionId` CHAR(36) NOT NULL,
  PRIMARY KEY (`StudentId`, `QuestionId`),
  INDEX `IX_student_upvote_QuestionId` (`QuestionId` ASC) VISIBLE,
  CONSTRAINT `FK_student_upvote_question_QuestionId`
    FOREIGN KEY (`QuestionId`)
    REFERENCES `fplms`.`question` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_student_upvote_student_StudentId`
    FOREIGN KEY (`StudentId`)
    REFERENCES `fplms`.`student` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
