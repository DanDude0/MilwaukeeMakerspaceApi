CREATE TABLE `attempt` (
  `attempt_id` ROWID,
  `keycode` TEXT NOT NULL DEFAULT '',
  `member_id` INTEGER NOT NULL,
  `reader_id` INTEGER NOT NULL,
  `access_granted` INTEGER NOT NULL DEFAULT 0,
  `login` INTEGER NOT NULL DEFAULT 0,
  `logout` INTEGER NOT NULL DEFAULT 0,
  `action` TEXT NOT NULL DEFAULT '',
  `attempt_time` TEXT NOT NULL,
  PRIMARY KEY (`attempt_id`)
);

CREATE TABLE `group` (
  `group_id` ROWID,
  `name` TEXT NOT NULL,
  PRIMARY KEY (`group_id`)
);

CREATE TABLE `group_member` (
  `group_id` INTEGER NOT NULL,
  `member_id` INTEGER NOT NULL,
  PRIMARY KEY (`member_id`)
);

CREATE TABLE `keycode` (
  `keycode_id` TEXT NOT NULL,
  `member_id` INTEGER NOT NULL,
  `updated` TEXT NOT NULL,
  PRIMARY KEY (`keycode_id`)
);

CREATE TABLE `member` (
  `member_id` INTEGER NOT NULL,
  `name` TEXT NOT NULL,
  `type` TEXT NOT NULL,
  `apricot_admin` INTEGER NOT NULL,
  `joined` TEXT NOT NULL,
  `expires` TEXT NOT NULL,
  `updated` TEXT NOT NULL,
  PRIMARY KEY (`member_id`)
);