
drop table if exists StoryCategory;
drop table if exists Category ;
drop table if exists StoryBlock;
drop table if exists TextBlock ;
drop table if exists GraphBlock ;
drop table if exists Story ;
drop type if exists PublicationStatus ;

CREATE TYPE PublicationStatus AS ENUM ('DRAFT', 'REVIEW', 'PUBLISHED');

create table Story (
    StoryID varchar(36),
	UserID varchar(36),
	PublicationStatus PublicationStatus,
	Title varchar(280),
	Description varchar(500),
	DateCreated timestamp,
	DateLastEdited timestamp,
    primary key (StoryID),
	foreign key (UserID) references HourglassUser(UserID)
) ; 
create table TextBlock (
	BlockID varchar(36),
	EditorState json,
	primary key (BlockID)
) ; 

create table GraphBlock (
	BlockID varchar(36),
	GraphID varchar(36),
	primary key(BlockID),
	foreign key (GraphID) references Graph(GraphID)
) ; 

/* FD: BlockID -> StoryID, Position */
create table StoryBlock (
	BlockID varchar(36),
	StoryID varchar(36),
	BlockPosition int,
	primary key (BlockID),
	foreign key (StoryID) references Story(StoryID),
	unique (StoryID, BlockPosition) /* Does this voilate 5NF? */
) ; 

create table Category (
	CategoryName varchar(500),
	CategoryDescription varchar(500),
	primary key (CategoryName)
) ; 

create table StoryCategory (
	StoryID varchar(36),
	CategoryName varchar(500),
	foreign key (StoryID) references Story(StoryID),
	foreign key (CategoryName) references Category(CategoryName),
	primary key (StoryID, CategoryName)
) ; 



