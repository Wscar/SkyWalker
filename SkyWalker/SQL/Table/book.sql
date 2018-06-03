CREATE TABLE `book` (
  `bookid` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `book_name` varchar(200) NOT NULL,
  `priface` varchar(500) DEFAULT NULL,
  `status` varchar(10) DEFAULT '公开',
  `create_time` datetime NOT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`bookid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
