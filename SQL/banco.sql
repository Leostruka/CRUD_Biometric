CREATE DATABASE IF NOT EXISTS `crud_users_db` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `crud_users_db`;

-- Copiando estrutura para tabela crud_users_db.user
CREATE TABLE IF NOT EXISTS `user` (
  `id` int NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Copiando estrutura para tabela crud_users_db.fir
CREATE TABLE IF NOT EXISTS `fir` (
  `id` int NOT NULL,
  `hash` text NOT NULL,
  `sample` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`sample`) USING BTREE,
  CONSTRAINT `fk_userid` FOREIGN KEY (`id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
