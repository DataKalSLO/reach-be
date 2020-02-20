
drop table if exists GraphSeries ;
drop table if exists Graph ;
drop type if exists SeriesType ;

CREATE TYPE SeriesType AS ENUM ('INDEPENDANT', 'DEPENDANT');

create table Graph (
	GraphID varchar(36),
	title varchar(500),
	primary key (GraphID)
) ; 

create table GraphSeries (
	GraphID varchar(36),
	TableName varchar(500),
	ColumnName varchar(500),
	SeriesType SeriesType,
	foreign key (TableName) references DatasetMetaData(TableName),
	foreign key (GraphID) references Graph(GraphID),
	primary key (GraphID, TableName, ColumnName, SeriesType)
) ; 