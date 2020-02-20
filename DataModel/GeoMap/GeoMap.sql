
create table GeoMap (
	GeoMapID varchar(36),
	primary key (GeoMapID)
) ; 

create table GeoMapTables (
	GeoMapID varchar(36),
	TableName varchar(500),
	foreign key (GeoMapID) references GeoMap(GeoMapID),
	foreign key (TableName) references DatasetMetaData(TableName)
) ; 

